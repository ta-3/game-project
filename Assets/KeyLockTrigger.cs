using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class KeyLockTrigger : MonoBehaviour, ITrigger {
    [SerializeField]
    private bool Locked = true;
    [SerializeField]
    private GameObject myTrigger;

    [SerializeField]
    private Collider key;

    [SerializeField]
    bool xAxisOrientated = false;
    private Interactable myTriggerBehaviour;
    int triggerHit = 0;
    int blockFlag = (int)RigidbodyConstraints.FreezeAll;
    
    public bool Trigger()
    {
        
        return isUnlocked();
    }

    private bool isUnlocked()
    {
        return !Locked;
    }
    // Use this for initialization
    void Start () {
        myTriggerBehaviour = myTrigger.GetComponent<Interactable>();
        if (xAxisOrientated) { blockFlag -= ((int)RigidbodyConstraints.FreezePositionX); }
        else { blockFlag -= ((int)RigidbodyConstraints.FreezePositionZ); }

    }
    private void OnTriggerEnter(Collider other)
    {
     
        if (other.Equals(key))
        {
           
            switch (triggerHit)
            {
               
                case 0:
                    Debug.Log("Key Entered key!");
                    myTriggerBehaviour.disableFeedback = true;
                    Vector3 pos = other.gameObject.transform.position;
                    pos= this.transform.InverseTransformPoint(pos);
                    pos.x = this.transform.localPosition.x; pos.y = this.transform.localPosition.y +0.1f;
                    pos.x = 0; pos.y = 0.1f;
                    pos = transform.TransformPoint(pos);

                    other.gameObject.transform.position =  pos;
                    other.gameObject.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints)blockFlag;
                    other.gameObject.transform.rotation = this.transform.rotation;
                   
                    break;
                case 1:
                    Debug.Log("Hit key!");
                    other.gameObject.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints)blockFlag;
                   
                    HapticFeedback.triggerHaptics(0f, 0.1f, 120f, .8f, myTrigger);
                    Locked = false;
                    other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    break;
                default:
                    break;
            }
            triggerHit++;
        }   
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.Equals(key))
        {
         
            switch (triggerHit)
            {

                case 1:
                    myTriggerBehaviour.disableFeedback = false ;
                    Debug.Log("Key Exited key!");
                    other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    break;
                case 2:
                    Debug.Log("Key Delocked key!");
                    
                    HapticFeedback.triggerHaptics(0f, 0.05f, 40f, .8f, myTrigger);
                    Locked = true;
                    break;
                default:
                    break;
            }
            triggerHit--;
          
         
          
        }
    }
    private void OnTriggerStay(Collider other)
    {
       
    }
    // Update is called once per frame
    void Update () {
		
	}
}
