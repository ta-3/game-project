using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Limiters/AlwaysTrue", order = 1)]
public class AlwayTrue : FocusLimiter
{
    public override bool canCastSpell()
    {
        return true;
    }
}
