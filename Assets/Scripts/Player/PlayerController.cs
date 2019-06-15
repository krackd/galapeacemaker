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
	public float DodgeCooldown = 2f;

	[Header("Rotation")]
	public float MouseSensitivity = 1f;

	private bool canDodge = true;

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		if (rb == null)
		{
			Debug.LogError("No rigid body found in player!");
		}
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		UpdatePosition();
		UpdateRotation();
	}

	void Update()
	{
		if (Input.GetButtonDown("UTurn"))
		{
			DoUTurn();
		}

		if (canDodge)
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
			// TODO make we can use dodge twice
			canDodge = false;
			StartCoroutine(dodgeCooldown(DodgeCooldown));
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
		canDodge = true;
	}
}
