using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour {
    [SerializeField]
    float timeToExist = 5.0f;

    [SerializeField]
    float explosiveForce = 1500.0f;

    [SerializeField]
    float ForceDamage = 30.0f;

    [SerializeField]
    float explosiveSize = 5.0f;

    [SerializeField]
    float explosiveScale = 1.0f;

    [SerializeField]
    float ignitionSize = 1.0f;

    float scale = 1.0f;
    [SerializeField]
    GameObject explosion;

    public void scaleFireball(float scale)
    {
        this.scale = scale;
        this.gameObject.transform.localScale = this.gameObject.transform.localScale * scale ;
  
        ignitionSize *= scale;
        explosiveScale *= scale;
       // ForceDamage *= scale;
        explosiveForce *= scale;
        explosiveSize *= scale;
        this.timeToExist *= scale;
        this.gameObject.GetComponent<ConstantForce>().relativeForce *= (1 / scale);
        AudioManager.reference.playSoundFromObject("FireballWhoosh", this.gameObject, false);
    }
    private void Awake()
    {
        
    }
    private void Update()
    {
        timeToExist -= Time.deltaTime;
        if (timeToExist < 0) { Destroy(this.gameObject); }
    }
    void OnCollisionEnter(Collision Col)
    {
        if (Col.gameObject.layer == 9) { return; }
        WetEffect w = Col.gameObject.GetComponent<WetEffect>();
        if (w == null)
        {
            if (Col.gameObject.tag == "Player") { return; }
            IIgnitable o = Col.gameObject.GetComponent<IIgnitable>();
            if (o != null) { o.Ignite(); }
            IMeltable m = Col.gameObject.GetComponent<IMeltable>();
            if (m != null) { m.Melt(); }
        }
        else
        {
            w.Dry();
        }
        GameObject explosionObj = (GameObject) Instantiate(explosion, transform.position, transform.rotation);
       
        explosionObj.GetComponent<explosionScaling>().setScale(scale);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosiveSize);
        

        foreach (Collider collider in hitColliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            IDestroyable id = collider.GetComponent<IDestroyable>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosiveForce, transform.position, explosiveSize, 3.0F);
                
            }
            if (id != null)
            {
                id.Damage(ForceDamage, damageTypes.Force);
            }
        }
        Collider[]  IgColliders = Physics.OverlapSphere(transform.position, ignitionSize);
        foreach (Collider collider in IgColliders)
        {
            IIgnitable ig = collider.GetComponent<IIgnitable>();
            WetEffect water = collider.gameObject.GetComponent<WetEffect>();
            IMeltable m = collider.gameObject.GetComponent<IMeltable>();
            if (m != null) { m.Melt(); }
            else if (water != null)
            {
                water.Dry();
            }
            else if (ig != null)
            {
                ig.Ignite();
            }

        }

        Destroy(explosionObj, 5);
        Destroy(this.gameObject);
    }
}
