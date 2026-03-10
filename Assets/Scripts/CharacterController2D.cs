using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CharacterController2D : MonoBehaviour
{
	[SerializeField] LayerMask groundLayerMask;
	[SerializeField] Transform groundCheck;
	[SerializeField] float groundCheckRadius;

	public bool onGround { get; private set; }
	public Vector2 groundNormal { get; private set; } = Vector2.zero;

	private Vector2 prevPosition;
	private Vector2 position;
	private Vector2 positionOffset;

	private Vector2 velocity;
	private Vector2 hitPoint = Vector2.zero;


	private Rigidbody2D rb;
	private CapsuleCollider2D capsuleCollider;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		capsuleCollider = GetComponent<CapsuleCollider2D>();
	}

	private void Update()
	{
		CheckCollision();
	}

	void FixedUpdate()
	{
		prevPosition = rb.position;
		position = prevPosition + positionOffset;
		velocity = (position - positionOffset) / Time.fixedDeltaTime;

		rb.MovePosition(position);
		positionOffset = Vector2.zero;
	}

	public void Move(Vector2 move)
	{
		positionOffset += move;
	}

	public void Teleport(Vector2 newPosition)
	{
		Vector2 delta = newPosition - position;
		prevPosition += delta;
		position = newPosition;
		rb.MovePosition(position);
	}

	public void CheckCollision()
	{
		onGround = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundLayerMask);
		foreach (var collider in colliders)
		{
			if (collider.gameObject == gameObject) continue;

			onGround = true;
		}

		if (onGround)
		{
			RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector2.down, 2, groundLayerMask);
			if (raycastHit.collider != null)
			{
				hitPoint = raycastHit.point;
				groundNormal = raycastHit.normal;
			}
		}
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = (onGround) ? Color.yellow : Color.red;
		Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
		if (onGround)
		{
			Gizmos.color = Color.white;
			Gizmos.DrawRay(hitPoint, groundNormal);
			Gizmos.DrawRay(hitPoint, new Vector2(groundNormal.y, -groundNormal.x));
		}
	}
}
