using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
	public Transform target;
	public Vector3 targetOffset;
	public float distance = 13.0f;
	public float maxDistance = 20.0f;
	public float minDistance = 1.0f;
	public float xSpeed = 200.0f;
	public float ySpeed = 200.0f;
	public int yMinLimit = -80;
	public int yMaxLimit = 80;
	public int zoomRate = 40;
	public float panSpeed = 0.5f;

	private float zoomDampening = 5.0f;
	private float xDeg = 0.0f;
	private float yDeg = 0.0f;
	private float currentDistance;
	private float desiredDistance;
	private Quaternion currentRotation;
	private Quaternion desiredRotation;
	private Quaternion rotation;
	private Vector3 position;
	private Quaternion initRotation;
	private Vector3 initPosition;
	private Vector3 initTargetPosition;

	void Start() { Init(); }
	
	void OnEnable() { Init(); }
	
	public void Init()
	{
		initPosition = new Vector3(transform.position.x, transform.position.y,transform.position.z);
		initRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
		//If there is no target, create a temporary target at 'distance' from the cameras current viewpoint
		if (!target)
		{
			GameObject go = new GameObject("Target");
			go.transform.position =new Vector3(0.0f,5.0f,0.0f);
			initTargetPosition = go.transform.position;
			target = go.transform;
		}

		distance = Vector3.Distance(transform.position, target.position);
		currentDistance = distance;
		desiredDistance = distance;
		
		position = transform.position;//the original status
		rotation = transform.rotation;
		currentRotation = transform.rotation;
		desiredRotation = transform.rotation;
		
		xDeg = Vector3.Angle(Vector3.right, transform.right );
		yDeg = Vector3.Angle(Vector3.up, transform.up );
	}


	void LateUpdate()
	{
		if (Input.GetMouseButton(1))//right button
		{
			xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

			//clamp the vertical axis for the rotation
			yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
			// set camera rotation
			desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
			currentRotation = transform.rotation;
			
			rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
			transform.rotation = rotation;
		}

		else if (Input.GetMouseButton(2))//middle button pressed
		{   

			target.rotation = transform.rotation;
			target.Translate(Vector3.right * -Input.GetAxis("Mouse X") * panSpeed);
			target.Translate(transform.up * -Input.GetAxis("Mouse Y") * panSpeed, Space.World);
		}
		
		// Reset the moving 
		else if (Input.GetKey(KeyCode.Space))
		{
			transform.position = initPosition;
			transform.rotation = initRotation;
			target.transform.position = initTargetPosition;
			Init();
		}

		// affect the desired Zoom distance if we roll the scrollwheel
		desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
		//clamp the zoom min/max
		desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
		// For smoothing of the zoom, lerp distance
		currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);
		
		// calculate position based on the new currentDistance
		position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
		transform.position = position;
	}
	
	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp(angle, min, max);
	}
}