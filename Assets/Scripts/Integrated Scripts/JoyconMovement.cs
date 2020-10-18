using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyconMovement : MonoBehaviour {

	private JoyconHand joyconHandLeft;
	private JoyconHand joyconHandRight;

	private Joycon joyconLeft;
	private Joycon joyconRight;

	private Camera c;

	private enum LeftButtonMap: int {
		JUMP = Joycon.Button.DPAD_DOWN
	}

	private float lookSpeed = 7.0f;

	/*
	 *
	 *
	 *DPAD_DOWN = 0,
        DPAD_RIGHT = 1,
        DPAD_LEFT = 2,
        DPAD_UP = 3,
        SL = 4,
        SR = 5,
        MINUS = 6,
        HOME = 7,
        PLUS = 8,
        CAPTURE = 9,
        STICK = 10,
        SHOULDER_1 = 11,
        SHOULDER_2 = 12
	 * 
	 * 
	 */

	// Values made available via Unity
	public float[] stick;
	public Vector3 gyro;
	public Vector3 accel;
	public int jc_ind = 0;
	public Quaternion orientation;

	private bool grabbing = false;
	private Transform grabbedObject = null;
	private Material selectionMaterial;
	private Material temporaryMaterial;
	private Vector3 grabbedObjectScale;

	//private string horizontalInputName;
	//private string verticalInputName;
	private float movementSpeed;
	private CharacterController charController;

	private AnimationCurve jumpFallOff;
	private float jumpMultiplier;
	//private KeyCode jumpKey;
	private bool isJumping = false;
	private float jumpingStepLimit;
	private bool initialised = false;

	private Transform cursor;

	public void initValues(JoyconHand joyconHandLeft, JoyconHand joyconHandRight, float movementSpeed, AnimationCurve jumpFallOff, float jumpMultiplier, float jumpingStepLimit, Material selectionMaterial, Material temporaryMaterial, Transform cursor)
	{
		this.joyconHandLeft  = joyconHandLeft;
		this.joyconHandRight = joyconHandRight;
		this.movementSpeed = movementSpeed;
		this.jumpFallOff = jumpFallOff;
		this.jumpMultiplier = jumpMultiplier;
		this.jumpingStepLimit = jumpingStepLimit;

		this.selectionMaterial = selectionMaterial;
		this.temporaryMaterial = temporaryMaterial;
		//this.jumpKey = jumpKey;

		this.joyconLeft  = joyconHandLeft.j;
		this.joyconRight = joyconHandRight.j;

		this.cursor = cursor;

		initialised = true;

		c = GetComponentInChildren(typeof(Camera)) as Camera;
	}

	private void Awake()
	{
		charController = GetComponent<CharacterController>();
	}

	// Update is called once per frame
	void Update () {
		PlayerMovement();
		HandleGrabs ();
	}

	private void HandleGrabs () {
		RaycastHit rae = new RaycastHit ();
		Physics.Raycast (joyconHandLeft.handTransform.position, -joyconHandLeft.handTransform.forward, out rae, 4.0f,int.MaxValue,QueryTriggerInteraction.Ignore);
		if (rae.collider && rae.collider.GetComponent<Renderer> ()) {
			cursor.SetPositionAndRotation (rae.point, Quaternion.identity);
			//rae.collider.GetComponent<Renderer> ().material = selectionMaterial;
		} else {
			RaycastHit extendedRay = new RaycastHit ();
			Physics.Raycast (joyconHandLeft.handTransform.position, -joyconHandLeft.handTransform.forward, out extendedRay, 16.0f);
			if (extendedRay.collider) {
				cursor.SetPositionAndRotation (extendedRay.point, Quaternion.identity);
			} else {
				//Material m = (Material) Resources.Load("Fire",
			}
		}

		if (joyconLeft.GetButtonDown (Joycon.Button.SHOULDER_1)) {
			if (!grabbing) {
				if (rae.collider) {
					if (rae.transform.CompareTag ("Focus")) {
						grabbing = true;

						rae.rigidbody.isKinematic = true;
						rae.rigidbody.useGravity = false;
						grabbedObject = rae.transform;
						grabbedObjectScale = grabbedObject.transform.localScale;
						grabbedObject.SetParent (joyconHandLeft.transform.parent, true);

						FocusMaterial f = grabbedObject.GetComponent<FocusMaterial> ();
						if (f != null) {
							joyconHandLeft.focusElement = f.getElement();
						} else {
							joyconHandLeft.focusElement = Element.None;
						}
					}
				}
			} else if (grabbedObject) {
				grabbing = false;
				grabbedObject.GetComponent<Rigidbody> ().isKinematic = false;
				Vector3 scale = grabbedObject.transform.localScale;
				grabbedObject.GetComponent<Rigidbody> ().useGravity = true;
				grabbedObject.transform.SetParent (null);
				grabbedObject.localScale = grabbedObjectScale;
				joyconHandLeft.focusElement = Element.Air;
			}
		}

		Debug.DrawRay (joyconHandLeft.handTransform.position, -joyconHandLeft.handTransform.forward, Color.green);
		Debug.DrawRay (joyconHandRight.handTransform.position, -joyconHandRight.handTransform.forward, Color.green);
	}

	private void PlayerMovement()
	{
		float tiltAroundY = joyconHandRight.GetStick ().x;
		float tiltAroundX = joyconHandRight.GetStick ().y;
		tiltAroundY = roundTiltInput (tiltAroundY, lookSpeed);
		tiltAroundX = roundTiltInput (tiltAroundX, lookSpeed);
		transform.Rotate (0.0f, tiltAroundY, 0.0f,Space.World);
		c.transform.Rotate (-tiltAroundX, 0.0f, 0.0f);

		float horizInput = joyconHandLeft.GetStick().x * movementSpeed;
		float vertInput  = joyconHandLeft.GetStick().y * movementSpeed;

		Vector3 forwardMovement = transform.forward * vertInput;
		Vector3 rightMovement = transform.right * horizInput;

		charController.SimpleMove(forwardMovement + rightMovement);
		charController.Move((forwardMovement + rightMovement + (transform.up*-9.81F)) * Time.deltaTime);

		JumpInput();

	}

	private float roundTiltInput (float tiltInput, float lookSpeed){
		float tier2 = 1.0f * Mathf.Sign(tiltInput);
		float tier1 = 0.5f * Mathf.Sign(tiltInput);
		float tier0 = 0.0f;

		float diff2 = Mathf.Abs (tier2 - tiltInput);
		float diff1 = Mathf.Abs (tier1 - tiltInput);
		float diff0 = Mathf.Abs (tier0 - tiltInput);

		float min = Mathf.Min (diff0, diff1, diff2); 

		if (min == diff2)
			tiltInput = tier2;
		else if (min == diff1)
			tiltInput = tier1;
		else
			tiltInput = tier0;

		return tiltInput * lookSpeed;
	}

	private void JumpInput()
	{
		if (initialised) {
			if (joyconLeft.GetButtonDown (Joycon.Button.DPAD_DOWN) && !isJumping) {
				isJumping = true;
				StartCoroutine (JumpEvent ());
			}
		}
	}

	private IEnumerator JumpEvent()
	{
		float originalSlopeLimit = charController.slopeLimit;
		float originalStep = charController.stepOffset;
		charController.stepOffset = jumpingStepLimit;

		charController.slopeLimit = 90.0f;
		float timeInAir = 0.0f;

		do
		{
			float jumpForce = jumpFallOff.Evaluate(timeInAir);
			charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
			timeInAir += Time.deltaTime;
			yield return null;
		} while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

		charController.slopeLimit = originalSlopeLimit;
		charController.stepOffset = originalStep;
		isJumping = false;
	}

}
