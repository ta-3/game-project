using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spells/ProjectileSpell", order = 1)]
class ProjectileSpell : SpellTemplate {

    [SerializeField]
    GameObject projectilePrefab;
    [SerializeField]
    float spawnDistance = 1;

     
        public override void cast(SpellData sp)
    {

        Physics.IgnoreLayerCollision(9, 10);
        Transform castingHand = targettingMethod.getObject().transform;
        //Camera cam = pm.GetComponentInChildren<Camera>();

        Vector3 spawnPos = castingHand.position + (castingHand.forward) * spawnDistance;
        Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(castingHand.forward));
     

        

    
}
}
