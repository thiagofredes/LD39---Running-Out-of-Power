using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : BaseGameObject
{
	public CharacterController playerController;

	public AimingController aim;

	public Text playerLife;

	public float playerSpeed = 5f;

	public int lifePoints = 10;

	public float timeBetweenDamage = 0.8f;

	public GameObject shotStart;

	public GameObject shotPrefab;

	public float timeBetweenShots = 0.1f;

	private float shotTime = 0f;

	private float damageTime = 0f;

	private AudioSource source;


	protected override void OnGameEnded ()
	{
		this.enabled = false;
		playerController.enabled = false;
	}

	protected override void OnGamePaused ()
	{
		this.enabled = false;
		playerController.enabled = false;
	}

	protected override void OnGameResumed ()
	{
		this.enabled = true;
		playerController.enabled = true;
	}

	void Awake ()
	{
		source = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update ()
	{
		Vector3 lookPoint = aim.ProjectedPosition;
		Vector3 newPlayerForward = lookPoint - playerController.transform.position;
		Vector3 newShotForward = lookPoint - shotStart.transform.position;
		Vector3 movement = Input.GetAxis ("Horizontal") * AimingController.CameraRightProjected + Input.GetAxis ("Vertical") * AimingController.CameraUpProjected;
		float horizontalInput = Input.GetAxis ("Horizontal");
		float verticalInput = Input.GetAxis ("Vertical");

		if (Input.GetMouseButton (0)) {
			if (shotTime > timeBetweenShots) {
				Shoot ();
			}
		}

		damageTime += Time.deltaTime;
		shotTime += Time.deltaTime;
		newPlayerForward.y = 0;
		newShotForward.y = 0;
		playerController.transform.rotation = Quaternion.LookRotation (newPlayerForward);
		shotStart.transform.rotation = Quaternion.LookRotation (newShotForward);
		playerController.Move ((movement.normalized - 2 * Vector3.up).normalized * Time.deltaTime * playerSpeed);
	}

	private void Shoot ()
	{
		GameObject bullet = (GameObject)GameObject.Instantiate (shotPrefab, shotStart.transform.position, shotStart.transform.rotation);
		bullet.tag = "PlayerBullet";
		bullet.GetComponent<Bullet> ().SetVelocity (shotStart.transform.forward);
		shotTime = 0f;
	}

	void OnCollisionEnter (Collision collisionInfo)
	{
		GameObject collisionObject = collisionInfo.gameObject;
		if (collisionObject.CompareTag ("WizardBullet")) {
			lifePoints--;
			playerLife.text = lifePoints.ToString ();
			if (!source.isPlaying) {
				source.Play ();
			}
		} 
		if (lifePoints <= 0) {
			playerLife.text = 0.ToString ();
			Destroy (this.gameObject);
			GameManager.EndGame ();
		}
	}

	void OnControllerColliderHit (ControllerColliderHit collisionInfo)
	{
		GameObject collisionObject = collisionInfo.gameObject;
		if (collisionObject.CompareTag ("Enemy") && damageTime > timeBetweenDamage) {
			damageTime = 0f;
			lifePoints -= collisionObject.GetComponent<Enemy> ().strenght;
			if (!source.isPlaying) {
				source.Play ();
			}
		}
		playerLife.text = lifePoints.ToString ();
		if (lifePoints <= 0) {
			playerLife.text = 0.ToString ();
			GameManager.EndGame ();
		}
	}
}
