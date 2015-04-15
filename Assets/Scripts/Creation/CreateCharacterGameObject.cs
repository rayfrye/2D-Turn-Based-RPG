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
		,bool isPlayer
	)
	{
		GameObject newCharacterGameObject = new GameObject();
		newCharacterGameObject.transform.parent = folder.transform;
		newCharacterGameObject.name = character.characterName;
		newCharacterGameObject.tag = factionTag;

		setupTransform (newCharacterGameObject.transform, pos, new Vector3 (1, 1, 1));
		setupSpriteRenderer (newCharacterGameObject, "Sprites/Infantry");
		setupMoveCharacter (newCharacterGameObject, cal, character);
		setupCharacter (newCharacterGameObject, character, allData, enemyFactionTag, pos, isPlayer);

		GameObject.Find ("Cell_" + newCharacterGameObject.GetComponent<CharacterGameObject> ().row + "_" + newCharacterGameObject.GetComponent<CharacterGameObject> ().col).GetComponent<Cell> ().isWalkable = false;

		allData.characterGameObjects.Add (newCharacterGameObject);
	}

	public void setupTransform(Transform characterTransform, Vector3 pos, Vector3 scale)
	{
		characterTransform.position = pos;
		characterTransform.localScale = scale;
	}

	public void setupSpriteRenderer(GameObject characterGameObject, string spriteName)
	{
		Debug.Log ("At some point want to store sprite with character class/character");
		Debug.Log ("Need to create sprite for archers");

		SpriteRenderer spriteRenderer = characterGameObject.AddComponent<SpriteRenderer> ();

		spriteRenderer.sprite = Resources.Load<Sprite>(spriteName);
		spriteRenderer.sortingOrder = 5;
	}

	public void setupMoveCharacter(GameObject characterGameObject, Calendar cal, Character character)
	{
		MoveCharacter moveCharacter = characterGameObject.AddComponent<MoveCharacter> ();

		moveCharacter.character = character;
		moveCharacter.cal = cal;
	}

	public void setupCharacter(GameObject characterGameObject, Character character, AllData allData, string enemyFactionTag, Vector3 pos, bool isPlayer)
	{
		CharacterGameObject characterGameObjectScript = characterGameObject.AddComponent<CharacterGameObject> ();

		characterGameObjectScript.character = character;
		characterGameObjectScript.allData = allData;
		characterGameObjectScript.enemyFactionTag = enemyFactionTag;
		characterGameObjectScript.currentHealth = character.characterClass.totalHealth;
		characterGameObjectScript.row = Mathf.Abs ((int) pos.y);
		characterGameObjectScript.col = Mathf.Abs ((int) pos.x);
		characterGameObjectScript.isPlayer = isPlayer;
	}

}
