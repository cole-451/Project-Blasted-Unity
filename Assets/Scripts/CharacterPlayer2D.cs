using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterPlayer2D : Character2D, IDamagable
{
	[SerializeField, Range(0, 20)] float jumpForce = 12;

	private Vector2 inputMove = Vector2.zero;
	private PlayerInputSystem_Actions inputActions;

    [SerializeField] float maxHealth = 100.0f;
    [SerializeField] Slider healthSlider;

    [SerializeField] GameObject leftDamager;
    [SerializeField] GameObject rightDamager;

	private bool stunned = false;


    protected override void Awake()
	{
		base.Awake();
		//get character part that contains the flip code???
		inputActions = new PlayerInputSystem_Actions();
		health = maxHealth;
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
		animator.SetBool("InAir", !characterController.onGround);
		if (characterController.onGround)
		{
			animator.ResetTrigger("Jump");
		}
        healthSlider.value = health / maxHealth;

		if (stunned)
		{
			//set stun bool
			inputActions.Disable();
			//startcoroutine(stunCR()); set stun back to false

		}

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
		OnAttackHitBox();
		StartCoroutine(PunchCooldownCR());

    }

    void OnJump(InputAction.CallbackContext ctx)
	{
		if (characterController.onGround) {
            movement.y = jumpForce;
            animator.SetTrigger("Jump");
            animator.SetBool("InAir", true);
        } 
		
	}

	IEnumerator PunchCooldownCR()
	{
		yield return new WaitForSeconds(3f);
        animator.ResetTrigger("Punch");

        //make a bool to hold the combo
    }
    IEnumerator KickCooldownCR()
	{
		yield return new WaitForSeconds(3f);
		animator.ResetTrigger("Kick");
	}

    public void OnDamage(float damage)
    {
        health -= damage;
		Mathf.Clamp(health, 0f, maxHealth);
		if (health <= 0f)
		{
			//Die
		}
		stunned = true;
    }

	public void OnAttackHitBox()
	{
		if(facing == eFace.Left)
		{
			leftDamager.SetActive(true);
		}
		if(facing == eFace.Right)
		{
			rightDamager.SetActive(true);
		}
	}
}
