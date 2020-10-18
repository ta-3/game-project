using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBallScript : MonoBehaviour, IMeltable {
    [SerializeField]
    float timeToExistOrigInv = 5.0f;
    float timeToExist = 5.0f;
    [SerializeField]
    float launchForce = 10f;

    [SerializeField]
    float distanceToCheck = 20f;

    [SerializeField]
    float forceDamageMultiplier = 1.0f;

    Vector3 scale;
    float strength = 1.0f;
    [SerializeField]
    GameObject prefab ;

    public void Initialise(float strength)
    {
        this.strength = strength;
        timeToExist *= strength;
        timeToExistOrigInv = 1/timeToExist;
        launchForce *= strength;
        forceDamageMultiplier *= strength;
        this.transform.localScale *= strength;
        scale = this.transform.localScale;
        Rigidbody r = this.gameObject.GetComponent<Rigidbody>();
        r.mass *= strength;
        r.AddForce(transform.forward * launchForce, ForceMode.Impulse);
        if (timeToExist < 0) { this.enabled = false; }
    }

    private void Update()
    {
        timeToExist -= Time.deltaTime;
        this.transform.localScale = scale * (timeToExist * timeToExistOrigInv);
        if (timeToExist < 0) { Destroy(this.gameObject); }

    }
    void OnCollisionEnter(Collision Col)
    {
        if (Col.gameObject.tag == "Player") { return; }
        IIgnitable o = Col.gameObject.GetComponent<IIgnitable>();
        IDestroyable d = Col.gameObject.GetComponent<IDestroyable>();
        if ((o != null)&&(o.IsOnFire())) { o.Extinguish(); Melt(); }
        if (d!=null) { d.Damage(Vector3.Dot(Col.contacts[0].normal, Col.relativeVelocity) * gameObject.GetComponent<Rigidbody>().mass*forceDamageMultiplier, damageTypes.Force); }
    }

    public void Melt()
    {
        Debug.Log("Melting");
        RaycastHit hit;
        GameObject hitObject;
        IIgnitable Torch;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, distanceToCheck))
        {
            hitObject = hit.transform.gameObject;
            
            Torch = (hitObject.GetComponent<IIgnitable>());
            if ((Torch == null)&&(hitObject.transform.parent!=null)) { Torch = hitObject.transform.parent.gameObject.GetComponentInChildren<IIgnitable>(); }
            if (Torch != null) { Debug.Log("Found Torch"); Torch.Extinguish(); }
            else { Debug.Log("No Torch"); }
       
        }
        else
        {
            
            Debug.Log("Did not Hit");
        }
      GameObject particle =  GameObject.Instantiate(prefab);
        prefab.transform.localScale *= strength;
        particle.transform.position = this.transform.position;
        Destroy(particle, 2);
        Destroy(this.gameObject);
    }

}
