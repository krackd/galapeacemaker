using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	public GameObject target;
	public float TranslationSmoothTime = 0.1f;
	public float RotationSpeed = 0.1f;

	[Header("Smooth Offset")]
	public bool EnableSmoothOffset = false;
	public Vector3 MaxOffset = Vector3.zero;
	private Vector3 offset = Vector3.zero;
	public float OffsetSmoothSpeedUp = 0.005f;
	public float OffsetSmoothSpeedDown = 0.0001f;

	private float startZ;

	// Use this for initialization
	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		if (target == null)
		{
			Debug.LogError("The target of follow camera could not be null!");
		}

		startZ = transform.position.z;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		UpdateOffset();
		UpdatePosition();
		UpdateRotation();
	}

	private void UpdateOffset()
	{
		if (EnableSmoothOffset)
		{
			float v = Mathf.Abs(Input.GetAxis("Vertical"));
			float h = Input.GetAxis("Horizontal");
			float z = Mathf.Clamp01(Mathf.Abs(v) + Mathf.Abs(h));
			Vector3 offsetTarget = new Vector3(MaxOffset.x * h, MaxOffset.y * v, MaxOffset.z * z);
			float smoothSpeed = z < 0.5f ? OffsetSmoothSpeedDown : OffsetSmoothSpeedUp;
			offset = Vector3.Lerp(offset, offsetTarget, smoothSpeed * Time.deltaTime * 1000);
		}
	}

	private void UpdatePosition()
	{
		Vector3 currentVelocity = Vector3.zero;
		float z = transform.position.z;
		Vector3 targetPos = target.transform.position;
		targetPos.z = startZ;
		Vector3 pos = Vector3.SmoothDamp(transform.position, targetPos, ref currentVelocity, TranslationSmoothTime);
		pos += target.transform.right * offset.x;
		pos += target.transform.up * offset.y;
		pos += target.transform.forward * offset.z;
		transform.position = pos;
	}

	private void UpdateRotation()
	{
		float angle = Mathf.Lerp(0, target.transform.rotation.eulerAngles.z - transform.rotation.eulerAngles.z, RotationSpeed * Time.deltaTime * 1000);
		transform.RotateAround(target.transform.position, target.transform.forward, angle);
	}
}
