using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateAbility : MonoBehaviour 
{
	public void createAbility
	(
		AllData allData
		,GameObject folder
		,int id
		,string abilityName
		,float damageMultiplier
		,float accuracy
		,int damageTypeID
		,List<int> abilityEffectIDs
	)
	{
		Ability newAbility = folder.AddComponent<Ability>();

		newAbility.id = id;
		newAbility.abilityName = abilityName;
		newAbility.damageMultiplier = damageMultiplier;
		newAbility.accuracy = accuracy;
		newAbility.damageTypeID = damageTypeID;
		newAbility.abilityEffectIDs = abilityEffectIDs;
		
		allData.abilities.Add (newAbility);

	}
}
