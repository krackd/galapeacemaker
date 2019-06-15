using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	public GameObject target;
	public float TranslationSmoothTime = 0.1f;
	public float RotationSpeed = 0.1f;
	public Vector3 Offset = Vector3.zero;

	// Use this for initialization
	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		if (target == null)
		{
			Debug.LogError("The target of follow camera could not be null!");
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		UpdatePosition();
		UpdateRotation();
	}

	private void UpdatePosition()
	{
		Vector3 currentVelocity = Vector3.zero;
		float z = transform.position.z;
		Vector3 pos = Vector3.SmoothDamp(transform.position, target.transform.position, ref currentVelocity, TranslationSmoothTime);
		pos.z = z;
		pos += target.transform.right * Offset.x;
		pos += target.transform.up * Offset.y;
		pos += target.transform.forward * Offset.z;
		transform.position = pos;
	}

	private void UpdateRotation()
	{
		float angle = Mathf.Lerp(0, target.transform.rotation.eulerAngles.z - transform.rotation.eulerAngles.z, RotationSpeed * Time.deltaTime * 1000);
		transform.RotateAround(target.transform.position, target.transform.forward, angle);
	}
}
