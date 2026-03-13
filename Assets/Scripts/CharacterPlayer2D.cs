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
	[SerializeField] AudioClip punchSound;

	int punchCount = 0;

    [SerializeField] GameObject leftDamager;
    [SerializeField] GameObject rightDamager;



    protected override void Awake()
	{
		base.Awake();
		//get character part that contains the flip code???
		inputActions = new PlayerInputSystem_Actions();
		health = maxHealth;
	}

	void OnEnable()
	{
		leftDamager.SetActive(false);
		rightDamager.SetActive(false);// for some reason this stops the inputactions

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
		animator.SetFloat("Health", health);
		animator.SetBool("InAir", !characterController.onGround);
		if (characterController.onGround)
		{
			animator.ResetTrigger("Jump");
		}
        healthSlider.value = health / maxHealth;

		animator.SetInteger("HitCount", punchCount);
		

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
		yield return new WaitForSeconds(0.3f);
		leftDamager.SetActive(false);
		rightDamager.SetActive(false);
		yield return new WaitForSeconds(3f);
        animator.ResetTrigger("Punch");

        //make a bool to hold the combo
    }
    IEnumerator KickCooldownCR()
	{
		yield return new WaitForSeconds(3f);
		animator.ResetTrigger("Kick");
	}

	IEnumerator StunCR()
	{
		yield return new WaitForSeconds(3f);
		punchCount = 0;
		animator.ResetTrigger("Hit");
	}

    public void OnDamage(float damage)
    {
		
        health -= damage;
		punchCount++;
		Mathf.Clamp(health, 0f, maxHealth);

		animator.SetTrigger("Hit");
		//stun player
        inputActions.Disable();

		if(health <=0)
		{
			animator.SetTrigger("Death");
			inputActions.Disable();
			//get launched

		}
        StartCoroutine(StunCR()); 
		

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
