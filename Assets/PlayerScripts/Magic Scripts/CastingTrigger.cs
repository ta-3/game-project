using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Gives us a class to inherit different magic triggers from.
[CreateAssetMenu()]
abstract class CastingTrigger : ScriptableObject
{
    public abstract int detectCastTrigger(SpellCasting sp);
    public abstract int detectCastChange(SpellCasting sp);
    public abstract void init();
    
}
