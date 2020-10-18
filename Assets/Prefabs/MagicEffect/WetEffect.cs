using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WetEffect : MonoBehaviour {

    [SerializeField]
    GameObject prefab;

    [SerializeField]
    float deltaTimemax = 10.0f;

    [SerializeField]
    float deltaTimeModifer = 1.0f;
    public float dripTime = 1f;
    public bool infiniteDrip = false;
    Color c;
    Renderer mr;
    Shader shader;
    ParticleSystem ps;
    // Use this for initialization

    void Start () {

        
        prefab = (GameObject)Resources.Load("RuntimeLoad/WaterDrips");
       
        if (prefab != null) { prefab = (GameObject)Instantiate(prefab, this.transform);  }
        ps = prefab.GetComponentInChildren<ParticleSystem>();
        ParticleSystem.ShapeModule psShape = ps.shape;
        MeshFilter mf = this.gameObject.GetComponent<MeshFilter>();
        if (mf==null)
        {
            mf = gameObject.GetComponentInChildren<MeshFilter>();
            if (mf == null) { Destroy(this); Destroy(prefab); return; }
        }
        Mesh emitter = (mf.mesh);
        if (emitter==null)
        {
            emitter = (this.gameObject.GetComponentInChildren<MeshFilter>().mesh);
            if (emitter == null)
            {
                emitter = (this.gameObject.GetComponentInParent<MeshFilter>().mesh);
                if (emitter == null) { Destroy(this); Destroy(prefab); return; }
            }
        }
        (psShape).mesh = emitter;
        mr = this.gameObject.GetComponent<Renderer>();
        if (mr != null)
        {
            shader = mr.material.shader;
            mr.material.shader = Shader.Find("Standard");
            mr.material.SetFloat("Smoothness", 1f);
            c = mr.material.color;
           // float newBlue = Mathf.Min(1f, c.b + 0.2f);
            mr.material.color = new Color(c.r, c.g, c.b, c.a);
        }
        
        ps.Play();
	}
	
	// Update is called once per frame
	void Update () {
        dripTime -= Time.deltaTime;
        if ((!infiniteDrip)&&dripTime<0)
        {
            if (mr != null)
            { mr.material.color = c; mr.material.shader = shader; }
           
            ps.Stop();
            enabled = false;
            dripTime = 0.0f;
        }
	}

    public void Dry()
    {
        if (mr != null)
        { mr.material.color = c; mr.material.shader = shader; }
      
        if (prefab != null)
        {
            ParticleSystem ps = prefab.GetComponentInChildren<ParticleSystem>();
            if (ps != null) ps.Stop();
            enabled = false;
            dripTime = 0.0f;

        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (!other.Equals(this.gameObject))
        {


            IIgnitable i = other.GetComponent<IIgnitable>();
            if (i != null)
            {
                if (i.IsOnFire()) i.Extinguish();
                else { makeWet(other); }
            }
            else
            {
                makeWet(other);
            }
        }
    }
    private void makeWet(GameObject go)
    {
        if (go.layer == 12) { return; }
        WetEffect we = go.GetComponent<WetEffect>();
        if (we == null)
        {

            we = go.AddComponent<WetEffect>();

        }
        we.setDripTime(Mathf.Min(deltaTimemax,we.dripTime+ (Time.deltaTime*deltaTimeModifer)));
    }

    public void setDripSize(float size)
    {
        prefab.GetComponentInChildren<ParticleSystem>().startSize = size;
    }

    public void setDripTime(float f)
    {
        dripTime =f;
        enabled = true;
    }
}
