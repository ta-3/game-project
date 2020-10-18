using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusMaterial : MonoBehaviour {
    [SerializeField]
    private Element element;
    [SerializeField]
    List<Component> RequiredOnComp;
    [SerializeField]
    List<Component> RequiredOffComp;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Element getElement()
    {
        if (isLocked()) { return Element.None; }
        else { return element; }
    }

    bool isLocked()
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
}

public enum Element {
    None,
    Air,
	Fire,
	Ice,
    Water,
      
}