using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : BaseGameObject
{

	public CanvasGroup congratulations;

	public CanvasGroup gameOver;

	public Player player;


	protected override void OnGameEnded ()
	{
		if (player.lifePoints > 0) {
			congratulations.alpha = 1f;
		} else {
			gameOver.alpha = 1f;
		}
	}
}
