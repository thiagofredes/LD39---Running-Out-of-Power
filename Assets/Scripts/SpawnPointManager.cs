using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : BaseGameObject
{
	public Transform[] spawnPoints;

	public Enemy[] enemiesPrefabs;

	public Player player;

	public int maxEnemies = 6;

	private int numSpawnPoints;

	private int numEnemyTypes;

	private int notSpawnPoint = -1;

	private Enemy[] minions;

	private int currentEnemies = 0;

	private static SpawnPointManager instance;


	void Awake ()
	{
		numSpawnPoints = spawnPoints.Length;
		numEnemyTypes = enemiesPrefabs.Length;
		minions = new Enemy[numEnemyTypes];
		for (int e = 0; e < numEnemyTypes; e++) {
			minions [enemiesPrefabs [e].type] = enemiesPrefabs [e];
		}
		instance = this;
	}

	protected override void OnGameEnded ()
	{
		this.enabled = false;
	}

	protected override void OnGamePaused ()
	{
		this.enabled = false;
	}

	protected override void OnGameResumed ()
	{
		this.enabled = true;
	}

	void Update ()
	{
		float smallestDistance = Mathf.Infinity;
		float distance;
		for (int s = 0; s < numSpawnPoints; s++) {
			distance = Vector3.Distance (spawnPoints [s].position, player.transform.position);
			if (distance < smallestDistance) {
				smallestDistance = distance;
				notSpawnPoint = s;
			}
		}
	}

	public void SpawnEnemy (int type)
	{
		if (currentEnemies < maxEnemies) {
			int index;

			do {
				index = Random.Range (0, numSpawnPoints);
			} while(index == notSpawnPoint);

			Enemy enemy = (Enemy)GameObject.Instantiate (minions [type], spawnPoints [index].position, spawnPoints [index].rotation);
			enemy.Activate (player);
			currentEnemies++;
		}
	}

	public static void EnemyKilled ()
	{
		instance.currentEnemies--;
	}
}
