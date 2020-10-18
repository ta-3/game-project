using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//I made a judgement we could probably reduce all targetting to gameobjects (even if invisible ones) I did this to reduce the complexity of the spell structures.
public abstract class TargettingTemplate : ScriptableObject {
    public abstract GameObject getObject();

}
