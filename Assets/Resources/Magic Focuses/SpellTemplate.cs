using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class will change a lot. Spells inherit from it which lets us set their name and targetting methods in the inspector (once we make them assets of course).
//As we find more template-able methods we can insert them here and that will save us code duplication.
abstract class SpellTemplate : ScriptableObject
{
    [SerializeField]
    protected string spellName;

    [SerializeField]
   protected TargettingTemplate targettingMethod;
    public abstract void cast(SpellData sp);
}
