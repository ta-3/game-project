using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterJet_Script : MonoBehaviour {

    [SerializeField]
    float WetSize = 2.0f;

    [SerializeField]
    float deltaTinmeAdjjustment = 4.0f;

    [SerializeField]
    float deltaTimemax = 10.0f;
    private void OnParticleCollision(GameObject other)
    {



        IIgnitable i = other.GetComponentInChildren<IIgnitable>();
            if (i != null)
            {
        ///    Debug.Log("Putting out!");
                if (i.IsOnFire()) i.Extinguish();
                else { makeWet(other); }
            }
            else
            {
                makeWet(other);
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
        we.setDripTime(Mathf.Min(deltaTimemax, we.dripTime + (Time.deltaTime * deltaTinmeAdjjustment)));
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

           // gameObject.GetComponentInChildren<ParticleSystem>().Stop();
           // Destroy(this.gameObject,10f);
        
	}
}
