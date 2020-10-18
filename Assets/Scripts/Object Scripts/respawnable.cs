using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnable : MonoBehaviour {

    Vector3 position;
	void Start () {
        position = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.position.y< -100.0f)
        {
            this.transform.position = position;
            this.GetComponent<Rigidbody>().velocity *= 0;
        }
	}
}
