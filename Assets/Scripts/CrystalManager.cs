using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalManager : MonoBehaviour
{

	public Crystal[] crystals;

	private int numCrystals;


	void Awake ()
	{
		numCrystals = crystals.Length;
	}

	public void SetAllInvulnerable (bool invulnerable)
	{
		for (int c = 0; c < numCrystals; c++) {
			if (crystals [c] != null)
				crystals [c].invulnerable = invulnerable;
		}	
	}

	public void ResetLifePoints ()
	{
		for (int c = 0; c < numCrystals; c++) {
			if (crystals [c] != null)
				crystals [c].ResetLifePoints ();
		}	
	}
}
