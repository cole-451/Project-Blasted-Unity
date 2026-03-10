using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterPlayer2D : Character2D
{
	[SerializeField, Range(0, 20)] float jumpForce = 12;

	private Vector2 inputMove = Vector2.zero;
	private PlayerInputSystem_Actions inputActions;

	protected override void Awake()
	{
		base.Awake();
		inputActions = new PlayerInputSystem_Actions();
	}

	void OnEnable()
	{
		inputActions.Enable();

		inputActions.Player.Move.performed += OnMove;
		inputActions.Player.Move.canceled += OnMove;

		inputActions.Player.Attack.performed += OnAttack;
		inputActions.Player.Jump.performed += OnJump;
	}

	void OnDisable()
	{
		inputActions.Player.Move.performed -= OnMove;
		inputActions.Player.Move.canceled -= OnMove;

		inputActions.Player.Attack.performed -= OnAttack;
		inputActions.Player.Jump.performed -= OnJump;

		inputActions.Disable();
	}

	private void Update()
	{
		animator.SetBool("OnGround", characterController.onGround);
	}

	protected override void FixedUpdate()
	{
		// horizontal movement
		movement.x = inputMove.x * speed;
		animator.SetFloat("Speed", Mathf.Abs(movement.x));
		if (Mathf.Abs(movement.x) > 0.1f) facing = (movement.x > 0) ? eFace.Right : eFace.Left;

		base.FixedUpdate();
	}

	void OnMove(InputAction.CallbackContext ctx)
	{
		inputMove = ctx.ReadValue<Vector2>();
	}

	void OnAttack(InputAction.CallbackContext ctx)
	{
		animator.SetTrigger("Punch");
		StartCoroutine(PunchCooldownCR());

    }

    void OnJump(InputAction.CallbackContext ctx)
	{
		if (characterController.onGround) movement.y = jumpForce;
	}

	IEnumerator PunchCooldownCR()
	{
		yield return new WaitForSeconds(2.5f);
        animator.ResetTrigger("Punch");

        //make a bool to hold the combo
    }
    IEnumerator KickCooldownCR()
	{
		yield return new WaitForSeconds(2.5f);
		animator.ResetTrigger("Kick");
	}

}
