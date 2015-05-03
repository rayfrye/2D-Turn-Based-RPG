using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatePlayerData : MonoBehaviour 
{
	public AllData allData;

	public void createPlayerData
	(
		GameObject folder
		,int gold
		,List<int> partyCharacterIDs
		,Dictionary<int,int> itemIDs
	)
	{
		PlayerData newPlayerData = folder.AddComponent<PlayerData>();

		newPlayerData.gold = gold;
		newPlayerData.partyCharacterIDs = partyCharacterIDs;
		newPlayerData.itemIDs = itemIDs;

		allData.playerData = newPlayerData;

	}
}
