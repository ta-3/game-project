using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a targetting method that finds the player game object. handy dandy.
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Targetting/TargetHandDirection", order = 1)]
public class TargetHandDirection : TargettingTemplate
{
    public override GameObject getObject()
    {
        return GameObject.Find("RightHand");
    }
}