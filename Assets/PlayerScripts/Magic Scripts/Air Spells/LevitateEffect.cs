using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LevitateEffect : MonoBehaviour {


    float maxHeight = .2f;
    private float headStart;
    private GameObject head;

    //private float velocity = 0.0f;
    public void resetPos()
    {
        Debug.Log("reset pos");
        transform.Translate(transform.position.y * Vector3.down);
        DestroyImmediate(this);
    }
    public void Initialize(float strength, float fadeTime, float airTimeMultiplier,float maxHeight)
    {
        Debug.Log("Initialized");

        head = GameObject.Find("VRCamera");
        headStart = (head.transform.localPosition -head.transform.forward*0.075f).y;
        this.maxHeight = maxHeight;
        //SteamVR_Fade.View(Color.black, fadeTime);
        //gameObject.transform.Translate(strength * Vector3.up);//position = gameObject.transform.position + (2 * Vector3.up);
        //SteamVR_Fade.View(Color.clear, fadeTime);
        //Debug.Log(gameObject.name);
        //timeTillDeath *= airTimeMultiplier;
        //this.fadeTime = fadeTime;
        //this.strength = strength;
    }
    // Use this for initialization
    void Start ()
    {
		
    }
	
	// Update is called once per frame
	void Update () {
     float position = (head.transform.localPosition - head.transform.forward * 0.075f).y;
        if (head.transform.position.y < maxHeight && position > headStart+0.06f)
        {
            transform.Translate(0.02f * Vector3.up);
            if (transform.position.y > maxHeight)
            {
                transform.position.Set(transform.position.x, maxHeight, transform.position.z);
          
            }
        }

      /*  if (transform.position.y > 0.0f && head.transform.localPosition.y < headStart.y - 0.08f)
        {
            transform.Translate(0.02f * Vector3.down);
            if (transform.position.y < 0.0f)
            {
                transform.position.Set(transform.position.x, 0, transform.position.z);
                Destroy(this);
            }
        }*/

        /*
        timeTillDeath -= Time.deltaTime;
        if (timeTillDeath<0)
        {
            SteamVR_Fade.View(Color.black, fadeTime);
            this.gameObject.transform.position = this.gameObject.transform.position + (strength * Vector3.down);
            SteamVR_Fade.View(Color.clear, fadeTime);
            Destroy(this);
        }
        */
	}
}

/*
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LevitateEffect : MonoBehaviour {
    public float timeTillDeath = 1.0f;
    float strength = 1.0f;
    float fadeTime = .2f;
    public void Initialize(float strength, float fadeTime, float airTimeMultiplier)
    {
        Debug.Log("Initialized");
        SteamVR_Fade.View(Color.black, fadeTime);
        gameObject.transform.Translate(strength * Vector3.up);//position = gameObject.transform.position + (2 * Vector3.up);
        SteamVR_Fade.View(Color.clear, fadeTime);
        Debug.Log(gameObject.name);
        timeTillDeath *= airTimeMultiplier;
        this.fadeTime = fadeTime;
        this.strength = strength;
    }
    // Use this for initialization
    void Start ()
    {
		
    }
	
	// Update is called once per frame
	void Update () {
        timeTillDeath -= Time.deltaTime;
        if (timeTillDeath<0)
        {
            SteamVR_Fade.View(Color.black, fadeTime);
            this.gameObject.transform.position = this.gameObject.transform.position + (strength * Vector3.down);
            SteamVR_Fade.View(Color.clear, fadeTime);
            Destroy(this);
        }
	}
}

     
     */
