using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Valve.VR.InteractionSystem;
public class AccioEffect : MonoBehaviour {
    float toleranceLevel = 0.35f;
 
    Transform targetHand;
    float strength;
    float timeToStopGravity = 2.0f;
   // bool grav = true;
    Rigidbody goR;
    float timeUsed = 0.0f;
    bool hitHand =false;
    Interactable interactable;
    public void Initialize(Transform targetHand, float strength)
    {
        this.targetHand = targetHand;
        this.strength = strength;
        interactable = this.GetComponent<Interactable>();

        goR = gameObject.GetComponent<Rigidbody>();
        goR.velocity = Vector3.zero;
        goR.velocity = (targetHand.position - transform.position).normalized * strength*5;
        timeToStopGravity = 2.0f;
        goR.useGravity = false;
       // Destroy(this, 5);
    }

    // Use this for initialization
    void Start () {
        Rigidbody goR = gameObject.GetComponent<Rigidbody>();
      //  grav = goR.useGravity;
    }
	
	// Update is called once per frame
	void Update () {
        

        timeUsed += Time.deltaTime;

        //if (timeUsed > 0.5f) {
        //	Debug.Break();
        //}	

        if (!hitHand)
        {
            if (Vector3.Distance(targetHand.position, transform.position) < toleranceLevel)
            {
                goR.velocity *= 0;
                hitHand = true;
            }
            else
            {
                Vector3 dir = (targetHand.position - transform.position).normalized * strength*5;
                //goR.velocity = (goR.velocity - ((targetHand.position - transform.position) / (transportTime-timeUsed)))*.1f   ;
                goR.velocity = dir;
            }
        }

        if (hitHand|| timeUsed>5.0f)
        {
            timeToStopGravity -= Time.deltaTime;
            if (timeToStopGravity < 0)
            {
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                Destroy(this);
            }
        }
        if(interactable!=null&&interactable.attachedToHand!=null)
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            Destroy(this);
        }


    }
}
