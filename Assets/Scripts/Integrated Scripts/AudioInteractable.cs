using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
public class AudioInteractable : Interactable {
    [SerializeField]
    string soundEffect;

    [SerializeField]
    float forceToGiveFeedback = 1.0f;


    private new void Start()
    {
        base.Start();
        am = AudioManager.reference;
    }
    AudioManager am = AudioManager.reference;
    AudioSource soundEffectClip;

    private void OnCollisionEnter(Collision collision)
    {
        
        if (disableFeedback) { return; }
        float force = (collision.relativeVelocity * this.GetComponent<Rigidbody>().mass).magnitude;
        if (force > forceToGiveFeedback)
        {
         
                 am.playSoundFromObject(soundEffect, this.gameObject, true,force);
            
            
             //   soundEffectClip.Play();
            
            if (attachedToHand != null)
            {

                if (collision.collider.isTrigger) { return; }
                attachedToHand.TriggerHapticPulse(0.05f, 140f, .3f);
            }
        }
    }
}
