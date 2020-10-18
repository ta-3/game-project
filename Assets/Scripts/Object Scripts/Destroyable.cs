using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum damageTypes
{
    Force,
    Fire,
}


public class Destroyable : MonoBehaviour, IDestroyable, ITrigger {
    [SerializeField]
    float InitialHp = 100;

    float localhp;

    [SerializeField]
    float InitialDamageMultiplier = 1.0f;

    float damageMultiplier;

    [SerializeField]
    float InitialDamageMultiplierForce = 1.0f;

    float damageMultiplierForce;

    [SerializeField]
    float InitialDamageMultiplierFire = 1.0f;

    float damageMultiplierFire;

    [SerializeField]
    GameObject smashedPrefab;
    [SerializeField]
    bool smashable=false;

    [SerializeField]
    float forceToSmash=10.0f;

    
     [SerializeField]
    GameObject prefab;
    public void Start()
    {
        damageMultiplier = InitialDamageMultiplier;
        damageMultiplierForce = InitialDamageMultiplierForce;
        damageMultiplierFire = InitialDamageMultiplierFire;
        localhp = InitialHp;


        
      
    }
    void destroySelf()
    {
          if (prefab != null) { prefab = (GameObject)Instantiate(prefab); }
        prefab.transform.position = this.transform.position;
        ParticleSystem ps = prefab.GetComponentInChildren<ParticleSystem>();
        ParticleSystem.ShapeModule psShape = ps.shape;
        MeshFilter mf = this.gameObject.GetComponent<MeshFilter>();
        if (mf == null)
        {
            mf = gameObject.GetComponentInChildren<MeshFilter>();
            if (mf == null) { Destroy(this); Destroy(prefab); return; }
        }
        Mesh emitter = (mf.mesh);
        if (emitter == null)
        {
            emitter = (this.gameObject.GetComponentInChildren<MeshFilter>().mesh);
            if (emitter == null)
            {
                emitter = (this.gameObject.GetComponentInParent<MeshFilter>().mesh);
                if (emitter == null) { Destroy(this); Destroy(prefab); return; }
            }
        }
        (psShape).mesh = emitter;
     //   prefab.transform.localScale = this.GetComponent<Renderer>().bounds.size;
       ps.Play();
        this.gameObject.GetComponent<Renderer>().enabled = false;
        Destroy(this.gameObject);
        Destroy(prefab, 3);
    }
    public void Update()
    {
        if (localhp < 0) { destroySelf(); }
    }
    public float Damage(float hp, damageTypes dt)
    {
       
        switch (dt)
        {
            case damageTypes.Force:
                localhp -= (hp * damageMultiplierForce );
               
                break;
            case damageTypes.Fire:
                localhp -= (hp * damageMultiplierFire);
                break;
            default:
                localhp -= (hp*damageMultiplier);
                break;
        }
        
        return localhp;

    }



    public float GetHealth()
    {
        return localhp;
    }

    public float Heal(float hp)
    {
        localhp += hp;
        return localhp;
    }

    public float GetDamageMultiplier()
    {
        return damageMultiplier;
    }
    public float setDamageMultiplier(float damageMultiplier)
    {
        
        this.damageMultiplier = damageMultiplier;
        return damageMultiplier;
    }

    public float GetDamageMultiplierForce()
    {
        return damageMultiplierForce;
    }
    public float setDamageMultiplierForce(float damageMultiplier)
    {
        this.damageMultiplierForce = damageMultiplier;
        return damageMultiplierForce;
    }

    public float GetDamageMultiplierFire()
    {
        return damageMultiplierFire;
    }
    public float setDamageMultiplierFire(float damageMultiplier)
    {
        this.damageMultiplierFire = damageMultiplier;
        return damageMultiplierFire;
    }

    public bool Trigger()
    {
        return localhp>0 ;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (smashable)
        {

            float force =(collision.relativeVelocity * this.GetComponent<Rigidbody>().mass).magnitude;
            if (force > forceToSmash)
            {
                if (smashedPrefab != null)
                {
                    smashedPrefab = (GameObject)Instantiate(smashedPrefab);
                    smashedPrefab.transform.position = this.transform.position;
                    smashedPrefab.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity;
                }
                Destroy(this.gameObject);
            }
        }
    }
}
