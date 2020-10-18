using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchTrigger : Ignitable {
    [SerializeField]
    bool isOnFire = false;

    public void Awake()
    {
        if (isOnFire) { ps.Play(); }
    }

    new public void Extinguish()
    {
        isOnFire = false;
        ps.Stop();
    }

    new public void Ignite()
    {
        isOnFire = true;
        ps.Play();
    }

    new public bool IsOnFire()
    {
        return isOnFire;
    }
	/*
    public override bool trigger()
    {
        return isOnFire;
    }
*/

}
