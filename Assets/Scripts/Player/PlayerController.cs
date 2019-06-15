using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[Header("Movement")]
	public float MoveForce = 20f;
	public float StrafeForce = 10f;

	public float MaxStrafeVelocity = 100;
	public float MaxMoveVelocity = 100;

	[Header("Dodge")]
	public float DodgeForce = 2f;
	public float DodgeCooldownInSeconds = 2f;
	public int NbDodgesInARaw = 2;
	public float DodgeDurationInSeconds = 1f;

	[Header("Rotation")]
	public float MouseSensitivity = 1f;

	private int nbAvailableDodges;
	private int nbDodges = 0;
	private bool IsDodging { get { return nbDodges > 0; } }

	private Rigidbody rb;
	private Health health;

	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		if (rb == null)
		{
			Debug.LogError("No rigid body found in player!");
		}

		health = GetComponent<Health>();

		resetNbDodges();
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		if (health != null && health.IsDead)
		{
			return;
		}

		UpdatePosition();
		UpdateRotation();
	}

	void Update()
	{
		if (health != null && health.IsDead)
		{
			return;
		}

		if (Input.GetButtonDown("UTurn"))
		{
			DoUTurn();
		}

		if (nbAvailableDodges > 0)
		{
			UpdateDodge();
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

	private void UpdatePosition()
	{
		float v = Input.GetAxis("Vertical");
		float h = Input.GetAxis("Horizontal");

		Vector3 velocity = rb.velocity;
		float moveSpeed = v > 0 ? MoveForce : MoveForce * 0.25f;
		Vector3 verticalForce = transform.up * v * moveSpeed;
		Vector3 horizontalForce = transform.right * h * StrafeForce;
		Vector3 force = verticalForce + horizontalForce;
		rb.AddForce(force * Time.deltaTime * 1000f);
		velocity.x = Mathf.Clamp(velocity.x, -MaxStrafeVelocity, MaxStrafeVelocity);
		velocity.y = Mathf.Clamp(velocity.y, -MaxMoveVelocity * 0.5f, MaxMoveVelocity);
		rb.velocity = velocity;
	}

	private void UpdateRotation()
	{
		float mouseX = Input.GetAxis("Mouse X");
		//Quaternion rot = Quaternion.Euler(0, 0, mouseY * MouseSensitivity);
		transform.RotateAround(transform.position, transform.forward, -mouseX * MouseSensitivity);
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

	private void resetNbDodges()
	{
		nbAvailableDodges = NbDodgesInARaw;
	}
}
