using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meltable : MonoBehaviour, IMeltable {
    [SerializeField]
    ParticleSystem ps;

    [SerializeField]
    GameObject prefab;
    [SerializeField]
    float distanceToCheck;

    float meltY = 0;
    void OnCollisionEnter(Collision Col)
    {
        if (Col.gameObject.tag == "Player") { return; }
        IIgnitable o = Col.gameObject.GetComponent<IIgnitable>();
        if ((o != null)&&(o.IsOnFire()))
        {
            o.Extinguish();
            RaycastHit hit;
            GameObject hitObject;
            IIgnitable Torch;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, distanceToCheck))
            {
                hitObject = hit.transform.gameObject;

                Torch = (hitObject.GetComponent<IIgnitable>());
                if ((Torch == null) && (hitObject.transform.parent != null)) { Torch = hitObject.transform.parent.gameObject.GetComponentInChildren<IIgnitable>(); }
                if (Torch != null) { Debug.Log("Found Torch"); Torch.Extinguish(); }
                else { Debug.Log("No Torch"); }

            }
            Destroy(this.gameObject);
        }

     

    }
    public void Melt()
    {

        RaycastHit hit;
        GameObject hitObject;
        IIgnitable Torch;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, distanceToCheck))
        {
            hitObject = hit.transform.gameObject;

            Torch = (hitObject.GetComponent<IIgnitable>());
            if ((Torch == null) && (hitObject.transform.parent != null)) { Torch = hitObject.transform.parent.gameObject.GetComponentInChildren<IIgnitable>(); }
            if (Torch != null) { Debug.Log("Found Torch"); Torch.Extinguish(); }
            else { Debug.Log("No Torch"); }

        }
        else
        {

            Debug.Log("Did not Hit");
        }
        if (prefab != null)
        {
            GameObject particle = GameObject.Instantiate(prefab);
        //    prefab.transform.localScale *= this.gameObject.GetComponent<Renderer>().bounds.size.magnitude;
            particle.transform.position = this.transform.position;
         
            Destroy(particle, 2);
            Destroy(this.gameObject);
        }
    }
}
