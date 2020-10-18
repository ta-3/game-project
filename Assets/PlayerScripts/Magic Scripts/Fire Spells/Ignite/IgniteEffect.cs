using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Effects/Ignite", order = 1)]
public class IgniteEffect : cursorEffect
{

    public override bool performEffect(GameObject go)
    {
        IIgnitable i = go.GetComponent<IIgnitable>();
        if (i != null)
        {

            i.Ignite();
            return true;
        }

        return false;
    }

}
