using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	#region Component Data

	[Header("Movement")]
	public float MoveForce = 20f;
	public float StrafeForce = 10f;
	public float MaxStrafeVelocity = 200;
	public float MaxMoveVelocity = 200;
	public float SpeedUpFactor = 2f;

	[Header("Rotation")]
	public float MouseSensitivity = 1f;
	public float JoystickSensitivity = 1f;

	[Header("Dodge")]
	public float DodgeForce = 2f;
	public float DodgeCooldownInSeconds = 2f;
	public int NbDodgesInARaw = 2;
	public float DodgeDurationInSeconds = 1f;

	[Header("UTurn")]
	public float UTurnCooldown = 5f;
	public int NbUTurnsInARaw = 1;

	#endregion

	#region Properties

	private bool IsDead { get { return health != null && health.IsDead; } }
	private bool CanDodge { get { return nbAvailableDodges > 0; } }
	private bool IsDodging { get { return nbDodges > 0; } }
	private bool CanUTurn { get { return nbAvailableUTurns > 0; } }

	#endregion

	#region Private fields

	private int nbAvailableUTurns;
	private int nbAvailableDodges;
	private int nbDodges = 0;

	private Rigidbody rb;
	private Collider[] colliders;
	private Health health;

	#endregion

	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		if (rb == null)
		{
			Debug.LogError("No rigid body found in player!");
		}

		colliders = GetComponents<Collider>();

		health = GetComponent<Health>();

		nbAvailableUTurns = NbUTurnsInARaw;
		nbAvailableDodges = NbDodgesInARaw;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (IsDead)
		{
			return;
		}

		UpdateVelocity();
		UpdateRotation();
	}

	void Update()
	{
		if (IsDead)
		{
			return;
		}

		UpdateSpeedUp();

		if (CanUTurn)
		{
			UpdateUTurn();
		}

		if (CanDodge)
		{
			UpdateDodge();
		}

	}

	private void UpdateSpeedUp()
	{
		if (Input.GetButtonDown("SpeedUp"))
		{
			MoveForce *= SpeedUpFactor;
			StrafeForce *= SpeedUpFactor;
			MaxMoveVelocity *= SpeedUpFactor;
			MaxStrafeVelocity *= SpeedUpFactor;

			foreach (Collider collider in colliders)
			{
				collider.isTrigger = true;
			}
		}
		else if (Input.GetButtonUp("SpeedUp"))
		{
			MoveForce /= SpeedUpFactor;
			StrafeForce /= SpeedUpFactor;
			MaxMoveVelocity /= SpeedUpFactor;
			MaxStrafeVelocity /= SpeedUpFactor;

			foreach (Collider collider in colliders)
			{
				collider.isTrigger = false; // TODO remove trigger if no collision
			}
		}
	}

	private void UpdateUTurn()
	{
		if (Input.GetButtonDown("UTurn"))
		{
			DoUTurn();
			nbAvailableUTurns--;
			StartCoroutine(uturnCooldown(UTurnCooldown));
		}
	}

	private void UpdateDodge()
	{
		bool dodgeUsed = false;

		if (Input.GetButtonDown("DodgeLeft"))
		{
			DoDodge(-transform.right);
			dodgeUsed = true;
		}
		else if (Input.GetButtonDown("DodgeRight"))
		{
			DoDodge(transform.right);
			dodgeUsed = true;
		}

		if (dodgeUsed)
		{
			nbAvailableDodges--;
			nbDodges++;
			health.IsInvincible = true;

			StartCoroutine(dodgeCooldown(DodgeCooldownInSeconds));
			StartCoroutine(stopDodge(DodgeDurationInSeconds));
		}
	}

	private void DoDodge(Vector3 dir)
	{
		rb.AddForce(dir.normalized * DodgeForce, ForceMode.Impulse);
	}

	private void DoUTurn()
	{
		transform.RotateAround(transform.position, transform.forward, 180);
	}

	private void UpdateVelocity()
	{
		float v = Input.GetAxis("Vertical");
		float h = Input.GetAxis("Horizontal");

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

	private void UpdateRotation()
	{
		float angle = -Input.GetAxis("Mouse X") * MouseSensitivity;
		angle += -Input.GetAxis("JoystickTurn") * JoystickSensitivity;
		transform.RotateAround(transform.position, transform.forward, angle);
	}

	private IEnumerator uturnCooldown(float delay)
	{
		yield return new WaitForSeconds(delay);
		nbAvailableUTurns++;
	}

	private IEnumerator dodgeCooldown(float delay)
	{
		yield return new WaitForSeconds(delay);
		nbAvailableDodges++;
	}

	private IEnumerator stopDodge(float delay)
	{
		yield return new WaitForSeconds(delay);
		nbDodges--;

		if (health != null && nbDodges <= 0)
		{
			health.IsInvincible = false;
		}
	}
}
