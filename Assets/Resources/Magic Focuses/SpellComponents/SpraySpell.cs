using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spells/SpraySpell", order = 1)]
class SpraySpell : SpellTemplate
{

    [SerializeField]
    GameObject projectilePrefab;


    public override void cast(SpellData sp)
    {

        Physics.IgnoreLayerCollision(9, 10);
        GameObject castingHand = targettingMethod.getObject();

        Instantiate(projectilePrefab, castingHand.transform,false);
       





    }
}

