using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This is the main class we can search for inside the player object to reach other player componenets. Mostly for convenience right now.
public class PlayerMain : MonoBehaviour {
    
   public PlayerStatus playerStatus;
	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 10, 10), "");
    }
}
