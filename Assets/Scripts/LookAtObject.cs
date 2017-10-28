using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAtObject : MonoBehaviour
{

	public GameObject target;

	public bool allignForward = true;

	public bool lookBack = false;

	void LateUpdate ()
	{
		Vector3 lookToVector = (target.transform.position - this.transform.position).normalized;

		if (!allignForward) {
			lookToVector.y = 0f;
		}

		if (lookBack) {
			this.transform.rotation = Quaternion.LookRotation (-lookToVector);
		} else {
			this.transform.rotation = Quaternion.LookRotation (lookToVector);
		}
	}
}
