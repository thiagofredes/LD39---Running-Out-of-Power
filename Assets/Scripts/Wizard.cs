using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Wizard : BaseGameObject
{

	public Transform[] wizardPositions;

	public Transform centerPosition;

	public Player player;

	public Text bossLife;

	public CrystalManager crystalManager;

	public int lifePoints = 100;

	public int lifePerCrystal = 25;

	public bool invulnerable = true;

	public float invulnerableMoveTime;

	public float vulnerableMoveTime;

	public float vulnerableTime = 5f;

	public float shotSpeed = 10f;

	public float timeBetweenSpawns = 5f;

	public float timeBetweenShots = 1f;

	public GameObject bulletPrefab;

	public GameObject shotOrigin;

	public SpawnPointManager spawnManager;

	public AudioClip intro;

	public AudioClip outOfPower;

	public AudioClip death;

	public AudioClip damage;

	public ParticleSystem explosion;

	private float shootTime;

	private float spawnTime;

	private int numPositions;

	private int currentPosition;

	private Coroutine moveCoroutine;

	private Collider thisCollider;

	private int lifeDepleted;

	private AudioSource audioSource;


	void Awake ()
	{
		numPositions = wizardPositions.Length;
		shootTime = 0f;
		spawnTime = 0f;
		thisCollider = GetComponent<Collider> ();
		thisCollider.enabled = false;
		audioSource = GetComponent<AudioSource> ();
	}

	IEnumerator Start ()
	{
		yield return new WaitForSeconds (1f);
		audioSource.PlayOneShot (intro);
	}

	protected override void OnGameEnded ()
	{
		StopAllCoroutines ();
		this.enabled = false;
	}

	protected override void OnGamePaused ()
	{
		StopAllCoroutines ();
		this.enabled = false;
	}

	protected override void OnGameResumed ()
	{
		this.currentPosition = -1;
		this.enabled = true;
	}

	void Update ()
	{
		if (invulnerable) {
			this.transform.LookAt (player.transform.position);
			MoveToNextPosition (GetNearestPosition ());
			Shoot ();
		}
		Spawn ();
	}

	private void Spawn ()
	{
		spawnTime += Time.deltaTime;
		if (spawnTime > timeBetweenSpawns) {
			spawnManager.SpawnEnemy (Random.Range (0, 3));
			spawnTime = 0f;
		}
	}


	private void Shoot ()
	{
		shootTime += Time.deltaTime;
		if (shootTime > timeBetweenShots) {
			GameObject bullet = (GameObject)GameObject.Instantiate (bulletPrefab, shotOrigin.transform.position, shotOrigin.transform.rotation);
			bullet.tag = "WizardBullet";
			bullet.GetComponent<Bullet> ().SetVelocity (shotOrigin.transform.forward, shotSpeed);
			shootTime = 0f;
		}
	}

	private void MoveToNextPosition (int position)
	{
		if (position != currentPosition) {
			currentPosition = position;
			if (moveCoroutine != null)
				StopCoroutine (moveCoroutine);
			moveCoroutine = StartCoroutine (Move (wizardPositions [currentPosition].position, invulnerableMoveTime));
		}
	}

	private int GetNearestPosition ()
	{
		float smallestDistance = Mathf.Infinity;
		int index = 0;
		float distance = 0f;
		for (int p = 0; p < numPositions; p++) {
			distance = Vector3.Distance (player.transform.position, wizardPositions [p].position);
			if (distance < smallestDistance) {
				smallestDistance = distance;
				index = p;
			}
		}
		return index;
	}

	private IEnumerator Move (Vector3 position, float time, UnityAction afterAction = null)
	{
		float t = 0;
		Vector3 originalPosition = this.transform.position;
		while (t < time) {
			this.transform.position = Vector3.Slerp (originalPosition, position, t / time);
			yield return new WaitForEndOfFrame ();
			t += Time.deltaTime;
		}
		this.transform.position = position;
		if (afterAction != null) {
			afterAction.Invoke ();
		}
	}

	public void ToggleVulnerability ()
	{
		if (invulnerable) {
			invulnerable = false;
			StopAllCoroutines ();
			StartCoroutine (Move (centerPosition.position, vulnerableMoveTime, () => {
				SetCollider (true);
				this.transform.LookAt (player.transform.position);
			})
			);
		} else {			
			StartCoroutine (Move (centerPosition.position + Vector3.up * 2f, invulnerableMoveTime, () => {
				SetCollider (false, () => {
					invulnerable = true;
					crystalManager.SetAllInvulnerable (false);
				});
			}));
		}
	}

	private void SetCollider (bool activation, UnityAction afterAction = null)
	{
		thisCollider.enabled = activation;
		if (afterAction != null)
			afterAction.Invoke ();
	}

	void OnCollisionEnter (Collision collisionInfo)
	{
		if (collisionInfo.gameObject.CompareTag ("PlayerBullet")) {
			lifePoints -= 1;
			lifeDepleted++;
			bossLife.text = lifePoints.ToString ();
			if (!audioSource.isPlaying) {
				audioSource.PlayOneShot (damage);
			}
			if (lifePoints <= 0) {
				bossLife.text = 0.ToString ();
				StartCoroutine (Death ());
			} else if (lifeDepleted == lifePerCrystal) {
				lifeDepleted = 0;
				if (lifePoints == 20) {
					audioSource.Stop ();
					audioSource.PlayOneShot (outOfPower);
				}
				ToggleVulnerability ();
			}
		}
	}

	private IEnumerator Death ()
	{
		this.GetComponent<Collider> ().enabled = false;
		audioSource.PlayOneShot (death);
		yield return new WaitForSeconds (death.length);
		explosion.Play ();
		this.GetComponent<Renderer> ().enabled = false;
		yield return new WaitForSeconds (1f);
		GameManager.EndGame ();
	}
}
