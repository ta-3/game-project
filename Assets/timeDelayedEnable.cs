using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeDelayedEnable : MonoBehaviour {

    [SerializeField]
    float time = 0.5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        time -= Time.deltaTime;
        if (time < 0) { this.gameObject.SetActive(true); }
        Destroy(this);
	}
}
