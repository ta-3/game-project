using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMovement : MonoBehaviour {

    private string horizontalInputName;
    private string verticalInputName;
    private float movementSpeed;
    private CharacterController charController;

    private AnimationCurve jumpFallOff;
    private float jumpMultiplier;
    private KeyCode jumpKey;
    private bool isJumping;
    private float jumpingStepLimit;
    public void initValues( string horizontalInputName,string verticalInputName, float movementSpeed, AnimationCurve jumpFallOff, float jumpMultiplier, float jumpingStepLimit, KeyCode jumpKey)
    {
        this.horizontalInputName = horizontalInputName;
        this.verticalInputName = verticalInputName;
        this.movementSpeed = movementSpeed;
        this.jumpFallOff = jumpFallOff;
        this.jumpMultiplier = jumpMultiplier;
        this.jumpingStepLimit = jumpingStepLimit;
        this.jumpKey = jumpKey;
        
    }
    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

	// Update is called once per frame
	void Update () {
        PlayerMovement();
    }
    private void PlayerMovement()
    {
        float horizInput = Input.GetAxis(horizontalInputName) * movementSpeed;
        float vertInput = Input.GetAxis(verticalInputName) * movementSpeed;

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        charController.SimpleMove(forwardMovement + rightMovement);
        charController.Move((forwardMovement + rightMovement + (transform.up*-9.81F)) * Time.deltaTime);

        JumpInput();

    }

    private void JumpInput()
    {
        if (Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
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
