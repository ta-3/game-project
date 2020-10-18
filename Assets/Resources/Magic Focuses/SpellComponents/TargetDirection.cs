using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a targetting method that finds the player game object. handy dandy.
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Targetting/TargetDirection", order = 1)]
public class TargetDirection : TargettingTemplate
{
    public override GameObject getObject()
    {
        return GameObject.FindWithTag("Player");
    }
}