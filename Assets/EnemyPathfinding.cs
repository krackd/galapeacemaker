using UnityEngine;

public class EnemyPathfinding : MonoBehaviour {

	public bool HasReachedTarget { get { return (target - transform.position).sqrMagnitude < TargetThresold * TargetThresold; } }
	public bool HasTarget { get { return hasTarget; } }
	public bool HasNoTarget { get { return !hasTarget; } }

	[Header("Movement")]
	public float MoveForce = 20f;
	public float StrafeForce = 10f;
	public float MaxStrafeVelocity = 200;
	public float MaxMoveVelocity = 200;

	[Header("Rotation")]
	public float RotationalDamp = 0.1f;

	[Header("Target")]
	public float TargetThresold = 1f;

	public Vector3 Target { set { target = value; hasTarget = true; } }
	private Vector3 target = Vector3.zero;
	private bool hasTarget = false;
	
	private bool IsDead { get { return health != null && health.IsDead; } }

	private Rigidbody rb;
	private Health health;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		if (rb == null)
		{
			Debug.LogError("No rigid body found in player!");
		}

		health = GetComponent<Health>();
	}

	void FixedUpdate()
	{
		if (IsDead || !hasTarget)
		{
			return;
		}

		UpdateVelocity();

		if (HasReachedTarget)
		{
			hasTarget = false;
		}
	}

	// Update is called once per frame
	void Update () {
		if (IsDead || !hasTarget)
		{
			return;
		}

		Turn();
	}

	void Turn()
	{
		Vector3 diff = target - transform.position;
		Quaternion rotation = Quaternion.FromToRotation(transform.forward, diff);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationalDamp * Time.deltaTime);
	}

	private void UpdateVelocity()
	{
		Vector3 dir = target - transform.position;
		dir.Normalize();
		float v = dir.y;
		float h = dir.x;

		Vector3 velocity = rb.velocity;
		float moveSpeed = v > 0 ? MoveForce : MoveForce;
		Vector3 verticalForce = transform.up * v * moveSpeed;
		Vector3 horizontalForce = transform.right * h * StrafeForce;
		Vector3 force = verticalForce + horizontalForce;
		rb.AddForce(force * Time.deltaTime * 1000f);
		velocity.x = Mathf.Clamp(velocity.x, -MaxStrafeVelocity, MaxStrafeVelocity);
		velocity.y = Mathf.Clamp(velocity.y, -MaxMoveVelocity, MaxMoveVelocity);
		rb.velocity = velocity;
	}
}
