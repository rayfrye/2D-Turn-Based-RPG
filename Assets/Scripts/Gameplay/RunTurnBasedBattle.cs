using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RunTurnBasedBattle : MonoBehaviour 
{
	public List<GameObject> playerParty = new List<GameObject>();
	public List<GameObject> enemyParty = new List<GameObject>();

	public enum battleState
	{
		PickingAction
		,PickingTarget
		,PerformingAction
	}

	public battleState currentState;

	public List<GameObject> allParties()
	{
		List<GameObject> allParties = playerParty;

		allParties.AddRange(enemyParty);

		return allParties;
	}

	void Update()
	{
		runBattle ();
	}

	void runBattle()
	{
		switch(currentState)
		{
		case battleState.PickingAction:
		{

			break;
		}
		default:
		{
			break;
		}
		}
	}



}
