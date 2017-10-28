using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillByFall : MonoBehaviour
{
	void OnTriggerEnter (Collider collider)
	{
		if (collider.gameObject.CompareTag ("Player")) {
			SceneManager.LoadScene ("Main");
		}
	}
}
