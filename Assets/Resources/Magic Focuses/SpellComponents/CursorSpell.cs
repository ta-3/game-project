using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class cursorEffect: ScriptableObject
{
    public abstract bool performEffect(GameObject go);
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spells/CursorSpell", order = 1)]
class CursorSpell : SpellTemplate
{
    
    [SerializeField]
    cursorEffect effect;

    public override void cast(SpellData sp)
    {



 
        GameObject go = targettingMethod.getObject();
        if (go != null)
        {
   
            effect.performEffect(go);
        }





    }
}
