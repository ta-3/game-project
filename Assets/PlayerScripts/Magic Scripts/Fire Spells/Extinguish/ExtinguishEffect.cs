using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Effects/Extinguish", order = 1)]
public class ExtinguishEffect : cursorEffect {

    public override bool performEffect(GameObject go)
    {
        IIgnitable i = go.GetComponent<IIgnitable>();
        if (i != null)
        {
      
            i.Extinguish();
            return true;
        }

        return false;
    }

}
