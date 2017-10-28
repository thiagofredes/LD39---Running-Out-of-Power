using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

	public float rotateSpeed = 5f;

	public float oscilateSpeed = 0.1f;

	void Update ()
	{
		this.transform.Rotate (0f, 0f, Time.deltaTime * rotateSpeed);
		this.transform.Translate (0f, 0f, Time.deltaTime * oscilateSpeed * Mathf.Sin (Time.time));
	}
}
