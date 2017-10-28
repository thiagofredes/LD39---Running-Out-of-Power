using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Crystal : BaseGameObject
{
	public ParticleSystem particles;

	public ParticleSystem bossParticles;

	public int lifePoints = 10;

	private int originalLifePoints;

	public bool invulnerable = false;

	public UnityEvent OnDestroy;

	private Coroutine particleCoroutine;

	private AudioSource source;

	void Awake ()
	{
		originalLifePoints = lifePoints;
		source = GetComponent<AudioSource> ();
	}

	void OnCollisionEnter (Collision collisionInfo)
	{
		if (!invulnerable) {
			if (collisionInfo.gameObject.CompareTag ("PlayerBullet")) {
				if (particleCoroutine == null)
					particleCoroutine = StartCoroutine (EmitParticles ());
				lifePoints--;
				if (!source.isPlaying) {
					source.Play ();
				}
				if (lifePoints == 0) {
					if (OnDestroy != null)
						OnDestroy.Invoke ();
					Destroy (this.gameObject);
				}
			}
		}
	}

	void LateUpdate ()
	{
		if (!invulnerable) {
			if (!bossParticles.isPlaying) {
				bossParticles.Play ();
			}
		} else {
			bossParticles.Stop ();
		}
	}


	private IEnumerator EmitParticles ()
	{
		particles.Play ();
		yield return new WaitForSeconds (0.2f);
		particles.Stop ();
		particleCoroutine = null;
		yield return null;
	}

	public void ResetLifePoints ()
	{
		lifePoints = originalLifePoints;
		bossParticles.Stop ();
	}

	protected override void OnGameEnded ()
	{
		particles.Stop ();
		this.enabled = false;
	}
}
