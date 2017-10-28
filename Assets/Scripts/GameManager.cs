using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

	public static event Action GamePaused;

	public static event Action GameEnded;

	public static event Action GameResumed;

	public static void Pause ()
	{
		if (GamePaused != null)
			GamePaused ();
	}

	public static void Resume ()
	{
		if (GameResumed != null)
			GameResumed ();
	}

	public static void EndGame ()
	{
		if (GameEnded != null)
			GameEnded ();
	}

	public void ResetScene ()
	{
		SceneManager.LoadScene ("Main");
	}
}
