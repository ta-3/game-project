using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollisionScript : MonoBehaviour {
    [SerializeField]
    float WetSize = 2.0f;

    [SerializeField]
    float deltaTimeModifer = 1.0f;

    [SerializeField]
    float deltaTimemax = 10.0f;

    public float dripTime = 1f;
    public bool infiniteDrip;
    private void OnParticleCollision(GameObject other)
    {
        // if (other = this.gameObject) { return; }
        if (!other.Equals(this.gameObject))
        {
            IIgnitable i = other.GetComponent<IIgnitable>();
            if (i != null)
            {
                if (i.IsOnFire()) i.Extinguish();

            }
            else
            {
                makeWet(other);
            }
        }
    }

    private void makeWet(GameObject go)
    {
        if (go.layer == 12) { return; }
        WetEffect we = go.GetComponent<WetEffect>();
        if (we == null)
        {

            we = go.AddComponent<WetEffect>();

        }

        we.setDripTime(Mathf.Min(deltaTimemax, we.dripTime+ (Time.deltaTime* deltaTimeModifer)));
    }
}
