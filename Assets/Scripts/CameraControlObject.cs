using UnityEngine;
using System.Collections;

public class CameraControlObject : MonoBehaviour
{
	public bool camActive = true;
	public Transform target;
	public Transform hand;// the leap motion object coordinate
	public Vector3 targetOffset;
	public float distance = 13.0f;
	public float maxDistance = 20.0f;
	public float minDistance = 1.0f;
	public float xSpeed = 100.0f;
	public float ySpeed = 100.0f;
	public int yMinLimit = -20;
	public int yMaxLimit = 60;
	public int zoomRate = 40;
	public float panSpeed = 0.5f;

	private Vector3 handScreenPos;
	private Camera camera;
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
	private Vector3 newTarget = Vector3.zero;

	void Start() { Init(); }
	
	void OnEnable() { Init(); }
	
	public void Init()
	{ 
		initPosition = new Vector3(transform.position.x, transform.position.y,transform.position.z);
		//initPosition = new Vector3(7.0f,2.0f,-7.0f);
		initRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
		//initRotation = new Quaternion(0.0f,315.0f,0.0f,transform.rotation.w);
		//If there is no target, create a temporary target at 'distance' from the cameras current viewpoint

		camera = GetComponent<Camera>();
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
		
		xDeg = Vector3.Angle(Vector3.right, transform.right);
		yDeg = Vector3.Angle(Vector3.up, transform.up);
	}

	public void ScrollToTarget(Vector3 target) {
		newTarget = target;
	}

	void LateUpdate()
	{
		// pan to new target incrementally
		if (newTarget != Vector3.zero) {
			if (Vector3.Distance(newTarget, target.position) > 0.5f) {
				Vector3 offset = (newTarget - target.position).normalized * panSpeed / 2f;
				target.position += offset;
			} else {
				newTarget = Vector3.zero;
			}
		}

		if (camActive) {
			handScreenPos = camera.WorldToScreenPoint(hand.position);

			//camera position scroll test
			if (handScreenPos.x<=1 || handScreenPos.x>=(Screen.width-1) || 
			    handScreenPos.y<=1 || handScreenPos.y>=(Screen.height-1)){
				if (handScreenPos.x<=1)xDeg+=xSpeed*0.02f;
				if(handScreenPos.x>(Screen.width-1)) xDeg-=xSpeed*0.02f;
				if (handScreenPos.y<=1)yDeg-=ySpeed*0.02f;
				if(handScreenPos.y>=(Screen.height-1)) yDeg += ySpeed*0.02f;
				yDeg = yDeg = ClampAngle (yDeg, yMinLimit, yMaxLimit);
				desiredRotation = Quaternion.Euler (yDeg, xDeg, 0);
				currentRotation = transform.rotation;
				rotation = Quaternion.Lerp (currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
				transform.rotation = rotation;

			}


			if (Input.GetMouseButton (1)) {//right button
				xDeg += Input.GetAxis ("Mouse X") * xSpeed * 0.02f;
				yDeg -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;

				//clamp the vertical axis for the rotation
				yDeg = ClampAngle (yDeg, yMinLimit, yMaxLimit);
				// set camera rotation
				desiredRotation = Quaternion.Euler (yDeg, xDeg, 0);
				currentRotation = transform.rotation;
				
				rotation = Quaternion.Lerp (currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
				transform.rotation = rotation;
			} else if (Input.GetMouseButton (0)) {//left button pressed

				target.rotation = transform.rotation;
				target.Translate (Vector3.right * -Input.GetAxis ("Mouse X") * panSpeed);
				target.Translate (transform.up * -Input.GetAxis ("Mouse Y") * panSpeed, Space.World);
			}
			
			// Reset the moving 
			else if (Input.GetKey (KeyCode.Space)) {
				transform.position = initPosition;
				transform.rotation = initRotation;
				target.transform.position = initTargetPosition;
				Init ();
			}

			// affect the desired Zoom distance if we roll the scrollwheel
			desiredDistance -= Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs (desiredDistance);
			//clamp the zoom min/max
			desiredDistance = Mathf.Clamp (desiredDistance, minDistance, maxDistance);
			// For smoothing of the zoom, lerp distance
			currentDistance = Mathf.Lerp (currentDistance, desiredDistance, Time.deltaTime * zoomDampening);
			
			// calculate position based on the new currentDistance
			position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
			transform.position = position;
		}
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