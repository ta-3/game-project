using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Our first spell and its a healing one for ease.
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spells/Debug", order = 1)]
class debugSpell : SpellTemplate {

    public override void cast(SpellData sp)
    {

        Debug.Log(spellName + " Cast ");
    }

}
