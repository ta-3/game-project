using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a targetting method that finds the player game object. handy dandy.
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Targetting/TargetSelf", order = 1)]
public class TargetSelf : TargettingTemplate {
    public override GameObject getObject()
    {
        return GameObject.FindWithTag("Player");
    }
}
