using UnityEngine;
using System.Collections;

public class CreateCharacterGameObject : MonoBehaviour 
{
	public AllData allData;

	public void createCharacterGameObject
	(
		Calendar cal
		,GameObject folder
		,Vector3 pos
		,Character character
		,string factionTag
		,string enemyFactionTag
	)
	{
		GameObject newCharacterGameObject = new GameObject();
		newCharacterGameObject.transform.parent = folder.transform;
		newCharacterGameObject.name = character.characterName;
		newCharacterGameObject.tag = factionTag;
		
		/*******************************************************************************
		 * Transform settings
		*******************************************************************************/
		newCharacterGameObject.transform.position = pos;
		newCharacterGameObject.transform.localScale = new Vector3(1,1,1);
		
		/*******************************************************************************
		 * Sprite Renderer settings
		*******************************************************************************/
		newCharacterGameObject.AddComponent<SpriteRenderer>();
		Debug.Log ("At some point want to store sprite with character class/character");
		Debug.Log ("Need to create sprite for archers");
		newCharacterGameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Infantry");
		newCharacterGameObject.GetComponent<SpriteRenderer> ().sortingOrder = 5;
		
		/*******************************************************************************
		 * MoveCharacter
		*******************************************************************************/
		newCharacterGameObject.AddComponent<MoveCharacter>();
		newCharacterGameObject.GetComponent<MoveCharacter>().character = character;
		newCharacterGameObject.GetComponent<MoveCharacter>().cal = cal;

		/*******************************************************************************
		 * Character
		*******************************************************************************/
		newCharacterGameObject.AddComponent<CharacterGameObject>();
		newCharacterGameObject.GetComponent<CharacterGameObject>().character = character;
		newCharacterGameObject.GetComponent<CharacterGameObject>().allData = allData;
		newCharacterGameObject.GetComponent<CharacterGameObject>().enemyFactionTag = enemyFactionTag;
		newCharacterGameObject.GetComponent<CharacterGameObject>().currentHealth = character.characterClass.totalHealth;
		newCharacterGameObject.GetComponent<CharacterGameObject>().row = Mathf.Abs ((int) pos.y);
		newCharacterGameObject.GetComponent<CharacterGameObject>().col = Mathf.Abs ((int) pos.x);

		GameObject.Find ("Cell_" + newCharacterGameObject.GetComponent<CharacterGameObject> ().row + "_" + newCharacterGameObject.GetComponent<CharacterGameObject> ().col).GetComponent<Cell> ().isWalkable = false;



		allData.characterGameObjects.Add (newCharacterGameObject);
	}
}
