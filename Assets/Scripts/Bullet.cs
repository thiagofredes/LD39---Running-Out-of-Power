using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BaseGameObject
{

	public Vector3 velocityDirection;

	public float speed = 20f;

	public float life = 1f;

	private Rigidbody thisRigidbody;

	private Vector3 oldSpeed;


	void Awake ()
	{
		thisRigidbody = GetComponent<Rigidbody> ();
		this.enabled = false;
	}

	public void SetVelocity (Vector3 velocity, float speed = 0, float life = 0f)
	{
		if (speed > 0f)
			thisRigidbody.velocity = velocity * speed;
		else
			thisRigidbody.velocity = velocity * this.speed;
		oldSpeed = thisRigidbody.velocity;
		if (life > 0f)
			Destroy (this.gameObject, life);
		else
			Destroy (this.gameObject, this.life);
	}

	protected override void OnGamePaused ()
	{
		thisRigidbody.velocity = Vector3.zero;
	}

	protected override void OnGameEnded ()
	{
		thisRigidbody.velocity = Vector3.zero;
	}

	protected override void OnGameResumed ()
	{
		thisRigidbody.velocity = oldSpeed;
	}

	void OnCollisionEnter (Collision collisionInfo)
	{		
		Destroy (this.gameObject);
	}
}
