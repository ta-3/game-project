using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TriggerLock : Lockable
{
    bool doorLocked = true;

    [SerializeField]
    List<Component> RequiredOnComp;
    [SerializeField]
    List<Component> RequiredOffComp;

    [SerializeField]
    bool playJingle = false;

    AudioSource s;
    Animator animator;
    public void Awake()
    {
        animator = GetComponent<Animator>();

 

    }
    public override bool isLocked()
    {

        ITrigger trigger;
        foreach (Component Ctrigger in RequiredOffComp)
        {

            if (Ctrigger != null)
            {
                trigger = (ITrigger)Ctrigger;
                if ((trigger != null) && (trigger.Trigger() == true)) { return true; }
            }
        }
        foreach (Component Ctrigger in RequiredOnComp)
        {
            if (Ctrigger != null)
            {
                trigger = (ITrigger)Ctrigger;
                if ((trigger == null) || (trigger.Trigger() == false)) { return true; }
            }
            else return true;
        }
        return false;
    }
    public void Update()
    {
        bool lockState = isLocked();
        if (lockState && (!doorLocked))//Door locked
        {
           
            animator.SetTrigger("Close");
            this.doorLocked = lockState;
            if (s != null) { s.Play(); }
            else { s = AudioManager.reference.playSoundFromObject("Door_Chains", this.gameObject, false); }

        }
        if ((!lockState) && (doorLocked))//Door unlocked
        {
            
            this.doorLocked = lockState;
            animator.SetTrigger("Open");
            if (playJingle) { AudioManager.reference.playSound("Success"); playJingle = false; }
            if (s != null) { s.Play(); }
            else { s = AudioManager.reference.playSoundFromObject("Door_Chains", this.gameObject, false); }
        }

    }

}
