using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyconAndMouse : AbstractClass_Movement {
	[SerializeField] private float sensitivityX = 50F;
	[SerializeField] private float sensitivityY = 50F;
	[SerializeField] private float minimumX = -360F;
	[SerializeField] private float maximumX = 360F;
	[SerializeField] private float minimumY = -60F;
	[SerializeField] private float maximumY = 60F;

	[SerializeField] private AnimationCurve jumpFallOff;
	[SerializeField] private float jumpMultiplier;
	[SerializeField] private float jumpStepOffset;
	[SerializeField] private JoyconHand leftJoycon;
	[SerializeField] private JoyconHand rightJoycon;
	//[SerializeField] private KeyCode jumpKey;
	//[SerializeField] private string horizontalInputName;
	//[SerializeField] private string verticalInputName;
	[SerializeField] private float movementSpeed;

	[SerializeField] private bool enableMouseLook;
	[SerializeField] private Material selectionMaterial;
	[SerializeField] private Material temporaryMaterial;
	[SerializeField] private Transform cursor;

	public JoyconMovement joyconMovementController;
	private SmoothMouseLook mouseLookController;
	void Start ()
	{
		joyconMovementController = gameObject.AddComponent(typeof(JoyconMovement)) as JoyconMovement;
		joyconMovementController.initValues(leftJoycon, rightJoycon, movementSpeed, jumpFallOff, jumpMultiplier, jumpStepOffset, selectionMaterial, temporaryMaterial, cursor);

		if (enableMouseLook) {
			mouseLookController = gameObject.AddComponent (typeof(SmoothMouseLook)) as SmoothMouseLook;
			mouseLookController.Init (sensitivityX, sensitivityY, minimumX, maximumX, minimumY, maximumY);
		}
	}



}
