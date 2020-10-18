using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardAndMouse : AbstractClass_Movement {
    [SerializeField] private float sensitivityX = 50F;
    [SerializeField] private float sensitivityY = 50F;
    [SerializeField] private float minimumX = -360F;
    [SerializeField] private float maximumX = 360F;
    [SerializeField] private float minimumY = -60F;
    [SerializeField] private float maximumY = 60F;

    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private float jumpStepOffset;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;
    [SerializeField] private float movementSpeed;

    private KeyboardMovement keyboardMovementController;
    private SmoothMouseLook mouseLookController;
    void Start ()
    {
        keyboardMovementController = gameObject.AddComponent(typeof(KeyboardMovement)) as KeyboardMovement;
        keyboardMovementController.initValues(horizontalInputName, verticalInputName, movementSpeed, jumpFallOff, jumpMultiplier,jumpStepOffset, jumpKey);
        mouseLookController = gameObject.AddComponent(typeof(SmoothMouseLook)) as SmoothMouseLook;
        mouseLookController.Init(sensitivityX, sensitivityY, minimumX, maximumX, minimumY, maximumY);
    }



}
