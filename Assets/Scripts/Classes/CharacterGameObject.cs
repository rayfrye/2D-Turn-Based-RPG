using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterGameObject : MonoBehaviour 
{
	public Character character;
	public string enemyFactionTag; 
	public int initiativeScore;

	public CharacterGameObject currentTarget;
	public int distanceToTarget;
	public List<GameObject> path;

	public bool isPlayer;

	public int row;
	public int col;

	public AllData allData;
	
	public int currentHealth;
}