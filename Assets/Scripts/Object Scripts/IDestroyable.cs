using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestroyable {
    float GetHealth();
    float Heal(float hp);
    float Damage(float hp, damageTypes dt);
    float GetDamageMultiplier();
    float setDamageMultiplier(float damageMultiplier);
    float GetDamageMultiplierForce();
    float setDamageMultiplierForce(float damageMultiplier);
    float GetDamageMultiplierFire();
    float setDamageMultiplierFire(float damageMultiplier);
}
