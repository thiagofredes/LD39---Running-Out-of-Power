using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameObject : MonoBehaviour
{
	void OnEnable ()
	{
		GameManager.GamePaused += OnGamePaused;
		GameManager.GameEnded += OnGameEnded;
		GameManager.GameResumed += OnGameResumed;
	}

	void OnDisable ()
	{
		GameManager.GamePaused -= OnGamePaused;
		GameManager.GameEnded -= OnGameEnded;
		GameManager.GameResumed -= OnGameResumed;
	}

	protected virtual void OnGamePaused ()
	{
	}

	protected virtual void OnGameEnded ()
	{
		
	}

	protected virtual void OnGameResumed ()
	{
		
	}
}
