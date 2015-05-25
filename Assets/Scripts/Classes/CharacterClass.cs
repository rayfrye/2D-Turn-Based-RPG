using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterClass : MonoBehaviour 
{
	public string charClassName;
	public int charClassID;
	public int moveSpeed;
	public int attack;
	public int defense;
	public int totalHealth;
	public int attackRange;

	public List<int> abilityIDs = new List<int>();
}
