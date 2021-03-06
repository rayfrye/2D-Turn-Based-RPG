﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterGameObject : MonoBehaviour 
{
	public Character character;
	public string faction;
	public string enemyFaction; 
	public int initiativeScore;

	public CharacterGameObject currentTarget;
	public int distanceToTarget;
	public List<GameObject> path;

	public string race;

	public string currentBody;
	public string currentHat;
	public string currentHair;
	public string currentPants;
	public string currentShirt;

	public Animator bodyAnimator;
	public Animator hatAnimator;
	public Animator hairAnimator;
	public Animator pantsAnimator;
	public Animator shirtAnimator;

	public bool isPlayer;

	public int row;
	public int col;

	public AllData allData;
	
	public int currentHealth;
	
	public dir currentDir;

	public List<string> dialogue = new List<string>();

	public enum dir
	{
		North
		,South
		,East
		,West
	}

	public string currentDirString()
	{
		switch (currentDir) 
		{
		case dir.North:
		{
			return "n";
			break;
		}
		case dir.South:
		{
			return "s";
			break;
		}
		case dir.East:
		{
			return "e";
			break;
		}
		case dir.West:
		{
			return "w";
			break;
		}
		default:
		{
			return "s";
			break;
		}
		}
	}

	public void currentAnimation(string animationType)
	{
		bodyAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/" + currentBody + "_" + animationType + "_" + currentDirString());
		hatAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/" + currentHat + "_" + animationType + "_" + currentDirString());
		hairAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/" + currentHair + "_" + animationType + "_" + currentDirString());
		shirtAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/" + currentShirt + "_" + animationType + "_" + currentDirString());
		pantsAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/" + currentPants + "_" + animationType + "_" + currentDirString());


	}
}