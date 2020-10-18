using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Limiters/TimeLimiter", order = 1)]
public class TimeLimiter : FocusLimiter {
    [SerializeField]
    float Cooldown = 0f;

    [SerializeField]
    float timeFromLastCast = 0.0f;

    public void Start()
    {
        timeFromLastCast = 0;
    }
    public void Awake()
    {
        timeFromLastCast = 0;
    }
    public override bool canCastSpell()
    {
      
        if ((timeFromLastCast + Cooldown < Time.time))
        {
            timeFromLastCast = Time.time;
            return true;
        }
        return false;
    }


	

}
