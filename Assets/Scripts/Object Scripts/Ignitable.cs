using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ignitable : MonoBehaviour, IIgnitable, ITrigger {

    private List<GameObject> collidedObjects = new List<GameObject>();
    [SerializeField]
    bool propogates = true;
    [SerializeField]
    float FirePropogationDelay = 4f;

    float timeFromLit = 0;
    [SerializeField]
    int fireDamagePerTick = 0;

    [SerializeField]
    float forceDrDamagePerTick = 0f;

    [SerializeField]
    float maxforceDrDamage = 0f;

    float forceDamage = 0f;
    [SerializeField]
    private bool destroyedOnBurnOut = true;

    [SerializeField]
    private bool reset = false;

    [SerializeField]
    private float resetTime = 10.0f;

    private float timeTillReset;
    [SerializeField]
    private float burnTime = 20f;

    [SerializeField]
    private bool infiniteBurn = false;

    [SerializeField]
    private bool Ignited = false;


    private bool ignited;

    [SerializeField]
    float audioVolume = 0.154f;
    AudioSource audioS;
    [SerializeField]
    public ParticleSystem ps;

    [SerializeField]
    Light l;

    IDestroyable health;
    void Start()
    {
        if (ps == null) { ps = (ParticleSystem)this.GetComponentInChildren<ParticleSystem>(); }
        if (l == null) { l = (Light)this.GetComponentInChildren<Light>(); }
        health = this.gameObject.GetComponent<IDestroyable>();
        timeTillReset = resetTime;
        ignited = Ignited;
        setEnabled();



        if (this.ignited)
        {
            
             ps.Play(); l.enabled = true;
            audioS = AudioManager.reference.playSoundFromObject("TorchNoise01",this.gameObject,false);
            audioS.volume = audioVolume;
        }
        else
        {
            ps.Stop(); l.enabled = false;
        }
    }

    void propogateFlames()
    {

             for (int i = 0; i < collidedObjects.Count; i++)
            {
          
            GameObject go = collidedObjects[i];
            if (go==null)
            {
                collidedObjects.RemoveAt(i);
                break;
            }


             
                IIgnitable o = collidedObjects[i].GetComponent<IIgnitable>();
                if (o != null) { o.Ignite(); }
                
                IMeltable m = collidedObjects[i].GetComponent<IMeltable>();
                if (m != null) { m.Melt(); }
            }
        
    }
    void Update()
    {
        if (ignited)
        {
            if (propogates)
            {
                if (timeFromLit > 0) { timeFromLit -= Time.deltaTime; }
                else { propogateFlames(); }
            }
            if (health != null)
            {
                
                health.Damage(fireDamagePerTick*Time.deltaTime, damageTypes.Fire);
                if (forceDamage < maxforceDrDamage) { health.setDamageMultiplierForce(health.GetDamageMultiplier() + (forceDrDamagePerTick)); forceDamage += forceDrDamagePerTick * Time.deltaTime; }
            }
            if (!infiniteBurn)
            {

                this.burnTime -= Time.deltaTime;
                if (burnTime < 0)
                {
                    this.ignited = false;
                    if (destroyedOnBurnOut) { Destroy(this.gameObject); }
                    else {  Destroy(this); }
                }
            }
        }
        

        if (reset&&(ignited!= Ignited))
        {
            timeTillReset -= Time.deltaTime;
            if (timeTillReset < 0)
            {
                if (Ignited) { Ignite(); }
                else { Extinguish(); }
            }
        }
    }

    private bool setEnabled()
    {
        if ((!infiniteBurn)||(reset && (ignited!=Ignited))||(health != null)||(propogates&&ignited)) { this.enabled = true; return true; }
        else { this.enabled = false; return false; }
    }
    public void Extinguish()
    {
        if (this.ignited)
        {
            if (audioS != null) { Destroy(audioS); }
            this.ignited = false;
            ps.Stop(); l.enabled = false;
            if (Ignited && reset) { timeTillReset = resetTime; }
            setEnabled();



        }
        
       
    }

    void OnTriggerEnter(Collider c)
    {

        collidedObjects.Add(c.gameObject);
    }
    void OnTriggerExit(Collider c)
    {
        collidedObjects.Remove(c.gameObject);
    }

    public void Ignite()
    {

        if (!ignited)
        {
            if (audioS == null) { audioS = AudioManager.reference.playSoundFromObject("TorchNoise01", this.gameObject, false);  audioS.volume = audioVolume; }
            WetEffect wetEffect = this.gameObject.GetComponentInChildren<WetEffect>();
            if (wetEffect != null)
            {
                wetEffect.Dry();
            }
            this.ignited = true;
            if (!Ignited)
            {
                if (this.reset) { timeTillReset = resetTime; }
                timeFromLit = FirePropogationDelay;

            }
            setEnabled();
            ps.Play(); l.enabled = true;
        }


        }

    public bool IsOnFire()
    {
        return ignited;
    }

    public bool Trigger()
    {
        return ignited;
    }
}
