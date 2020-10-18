using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyconHand : MonoBehaviour {

	private List<Joycon> joycons;

	// Values made available via Unity
	public float[] stick;
	public Vector3 gyro;
	public Vector3 accel;
	//public int jc_ind = 0;
	public Quaternion orientation;
	public Quaternion facing;
	public Transform handTransform;

	public Element focusElement = Element.Air;

	public bool isLeft;

	public Joycon j;

	private Vector2 joystick;

	void Awake ()
	{
		handTransform = transform;
		joystick = Vector2.zero;

		gyro = new Vector3(0, 0, 0);
		accel = new Vector3(0, 0, 0);
		// get the public Joycon array attached to the JoyconManager in scene
		joycons = JoyconManager.Instance.j;

		if (joycons.Count == 0) {
			string handedString = isLeft ? "Left" : "Right";
			Debug.Log (handedString + " hand Joycon has found no hardware Joycons!");
		}

		for (int i = 0; i < joycons.Count; i++) {
			if (isLeft && joycons [i].isLeft) {
				j = joycons [i];
				if (!(j != null))
					Debug.Log ("Left joycon is null");
			} else if (!isLeft && !joycons [i].isLeft) {
				j = joycons [i];
				if (!(j != null))
					Debug.Log ("Right joycon is null");
			}
		}
	}

	public bool GetDpadUp(){
		return j.GetButtonDown (Joycon.Button.DPAD_UP);
	}

	public bool GetDpadDown(){
		return j.GetButtonDown (Joycon.Button.DPAD_DOWN);
	}

	public bool GetDpadLeft(){
		return j.GetButtonDown (Joycon.Button.DPAD_LEFT);
	}

	public bool GetDpadRight(){
		return j.GetButtonDown (Joycon.Button.DPAD_RIGHT);
	}

	// Update is called once per frame
	void Update () {
		// make sure the Joycon only gets checked if attached
		if (joycons.Count > 0)
		{


			if (j.GetButtonDown(Joycon.Button.SHOULDER_2))
			{
				// Joycon has no magnetometer, so it cannot accurately determine its yaw value. Joycon.Recenter allows the user to reset the yaw value.
				j.Recenter ();
			}


			stick = j.GetStick();

			//Transform playerTransform = transform.parent;
			//playerTransform.Translate (stick [0], 0.0f, stick [1]);


			// Gyro values: x, y, z axis values (in radians per second)
			gyro = j.GetGyro();

			// Accel values:  x, y, z axis values (in Gs)
			accel = j.GetAccel();

			orientation = j.GetVectorPrime();
			facing = j.GetVector ();
			gameObject.transform.parent.rotation = gameObject.transform.parent.parent.rotation * orientation;

			//Debug.DrawRay (transform.position, facing * transform.forward, Color.green);
			//Debug.DrawRay (transform.position, orientation * transform.forward, Color.red);
		}
	}

	public Vector2 GetStick() {
		return (new Vector2 (stick[0], stick[1]));
	}
}