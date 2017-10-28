using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : BaseGameObject
{
	public int type;

	public int lifePoints;

	public int strenght;

	public float speed;

	private NavMeshAgent navMeshAgent;

	private Player player;

	private AudioSource source;


	void Awake ()
	{
		navMeshAgent = GetComponent<NavMeshAgent> ();
		source = GetComponent<AudioSource> ();
	}

	public void Activate (Player player)
	{
		this.player = player;
		navMeshAgent.speed = this.speed;
	}

	protected override void OnGameEnded ()
	{
		this.enabled = false;
		navMeshAgent.enabled = false;
	}

	protected override void OnGamePaused ()
	{
		this.enabled = false;
		navMeshAgent.enabled = false;
	}

	protected override void OnGameResumed ()
	{
		navMeshAgent.enabled = true;
		this.enabled = true;
	}

	void Update ()
	{
		if (this.player != null)
			navMeshAgent.destination = this.player.transform.position;
	}

	void OnCollisionEnter (Collision collisionInfo)
	{
		if (collisionInfo.gameObject.CompareTag ("PlayerBullet")) {
			lifePoints--;
			if (!source.isPlaying) {
				source.Play ();
			}
			if (lifePoints <= 0) {
				SpawnPointManager.EnemyKilled ();
				Destroy (this.gameObject);
			}
		}
	}
}
