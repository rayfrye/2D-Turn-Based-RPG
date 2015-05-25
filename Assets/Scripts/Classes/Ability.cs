using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ability : MonoBehaviour 
{
	public int id;
	public string abilityName;

	public float damageMultiplier;

	public float accuracy;

	public int damageTypeID;

	public List<int> abilityEffectIDs = new List<int>();

}
