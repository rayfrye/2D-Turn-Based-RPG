using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour 
{
	public int gold;
	public List<int> partyCharacterIDs = new List<int>();

	public Dictionary<int,int> itemIDs = new Dictionary<int, int>();
}
