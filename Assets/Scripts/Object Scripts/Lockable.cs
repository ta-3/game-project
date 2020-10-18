using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Lockable:MonoBehaviour  {

   public abstract bool isLocked();
}
