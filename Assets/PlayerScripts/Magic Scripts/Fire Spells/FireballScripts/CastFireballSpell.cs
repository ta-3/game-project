using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spells/FireballSpell", order = 1)]
class CastFireballSpell : SpellTemplate
{
    [SerializeField]
    GameObject fireball;


    public override void cast(SpellData sp)
    {

        Physics.IgnoreLayerCollision(9, 10);
        //Transform castingHand = targettingMethod.getObject().transform;
        //Camera cam = pm.GetComponentInChildren<Camera>();

        Vector3 spawnPos = sp.Position;
		GameObject go = (GameObject)Instantiate(fireball, spawnPos, Quaternion.LookRotation(sp.Normal));
        go.GetComponentInChildren<FireballScript>().scaleFireball(sp.Scale);
        
    }

}

