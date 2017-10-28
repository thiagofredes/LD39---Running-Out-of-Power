using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimingController : MonoBehaviour
{
	public Image aimImage;

	public Vector3 projectedPosition;

	public float maxCamRaylength = 100f;

	private Camera mainCamera;


	public Vector3 ProjectedPosition {
		get{ return this.projectedPosition; }
	}

	public static Vector3 CameraRight;

	public static Vector3 CameraUp;

	public static Vector3 CameraRightProjected;

	public static Vector3 CameraUpProjected;


	void OnEnable ()
	{
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = false;
	}

	void Awake ()
	{
		mainCamera = Camera.main;
		CameraRight = mainCamera.transform.transform.right;
		CameraRightProjected = Vector3.ProjectOnPlane (mainCamera.transform.transform.right, Vector3.up).normalized;
		CameraUp = mainCamera.transform.transform.up;
		CameraUpProjected = Vector3.ProjectOnPlane (mainCamera.transform.transform.up, Vector3.up).normalized;
	}

	void Update ()
	{
		Vector3 mousePosition = Input.mousePosition;
		Ray ray = mainCamera.ScreenPointToRay (mousePosition);
		RaycastHit raycastHit;
		aimImage.rectTransform.position = mousePosition;
		if (Physics.Raycast (ray.origin, ray.direction, out raycastHit, maxCamRaylength)) {
			projectedPosition = raycastHit.point;
		} else {
			projectedPosition = ray.origin + mainCamera.transform.forward * maxCamRaylength;
		}
	}
}
