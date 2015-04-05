using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CreateBattle : MonoBehaviour 
{
	public AllData allData;
	public Pathfinder pathfinder;

	public List<GameObject> characterBattleOrder;
	public int currentCharacterTurnIndex;
	public int numOfDiceSides;

	public void calculateBattleOrder
	(
		List<GameObject> characterGameObjects
	)
	{
		foreach(GameObject characterGameObject in characterGameObjects)
		{
			int diceRoll = Random.Range (0,numOfDiceSides) + (int) (characterGameObject.GetComponent<CharacterGameObject>().character.moveSpeed()*100);
			characterGameObject.GetComponent<CharacterGameObject>().initiativeScore = diceRoll;
		}
	
		characterGameObjects.Sort ((character1,character2) => character1.GetComponent<CharacterGameObject>().initiativeScore.CompareTo(character2.GetComponent<CharacterGameObject>().initiativeScore));
		characterGameObjects.Reverse ();
		allData.characterGameObjects = characterGameObjects;
	}

}