using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CreateCharacterGameObject : MonoBehaviour 
{
	public AllData allData;

	public GameObject createCharacterGameObject
	(
		Calendar cal
		,GameObject folder
		,Vector3 pos
		,Character character
		,string tag
		,string faction
		,string enemyFaction
		,bool isPlayer
		,int row
		,int col
	)
	{
		GameObject newCharacterGameObject = new GameObject();
		newCharacterGameObject.transform.parent = folder.transform;
		newCharacterGameObject.name = character.characterName;
		newCharacterGameObject.tag = tag;

		setupTransform (newCharacterGameObject.transform, pos, new Vector3 (1, 1, 1));
		setupSpriteRenderer (newCharacterGameObject, "Sprites/human_body", 5);
		setupMoveCharacter (newCharacterGameObject, cal, character);
		setupCharacter (newCharacterGameObject, character, allData, enemyFaction, pos, isPlayer, row, col);
		setupAnimation (newCharacterGameObject, newCharacterGameObject, "body");
		setupEquipment(newCharacterGameObject);

		setupHat (newCharacterGameObject, pos);

		GameObject.Find ("Cell_" + newCharacterGameObject.GetComponent<CharacterGameObject> ().row + "_" + newCharacterGameObject.GetComponent<CharacterGameObject> ().col).GetComponent<Cell> ().isWalkable = false;

		allData.characterGameObjects.Add (newCharacterGameObject);

		return newCharacterGameObject;
	}

	public void setupEquipment(GameObject go)
	{
		CharacterGameObject characterGameObject = go.GetComponent<CharacterGameObject> ();

		characterGameObject.race = "human";
		characterGameObject.currentBody = "body";
		characterGameObject.currentHat = "hat";
	}

	public void setupHat(GameObject characterGameObject, Vector3 pos)
	{
		GameObject newHat = new GameObject ();
		newHat.name = "hat";

		newHat.transform.parent = characterGameObject.transform;

		setupTransform (newHat.transform, pos, new Vector3(1,1,1));
		setupSpriteRenderer (newHat, "Sprites/human_hat", 6);
		setupAnimation (characterGameObject, newHat, "hat");
	}

	public void setupAnimation(GameObject go, GameObject animationGameObject, string animationPart)
	{
		CharacterGameObject characterGameObject = go.GetComponent<CharacterGameObject> ();
		Animator animator = animationGameObject.AddComponent<Animator> ();
		animator.speed = .75f;

		switch (animationPart) 
		{
		case "body":
		{
			characterGameObject.bodyAnimator = animator;
			break;
		}
		case "hat":
		{
			characterGameObject.hatAnimator = animator;
			break;
		}
		default:
		{
			break;
		}
		}

		//animator.runtimeAnimatorController =  Resources.Load<RuntimeAnimatorController>("Animations/human_"+animationPart+"_walk_s");

	}

	public void setupTransform(Transform characterTransform, Vector3 pos, Vector3 scale)
	{
		characterTransform.position = pos;
		characterTransform.localScale = scale;
	}

	public void setupSpriteRenderer(GameObject characterGameObject, string spriteName, int sortingOrder)
	{
		Debug.Log ("At some point want to store sprite with character class/character");

		SpriteRenderer spriteRenderer = characterGameObject.AddComponent<SpriteRenderer> ();

		spriteRenderer.sprite = Resources.Load<Sprite>(spriteName);
		spriteRenderer.sortingOrder = sortingOrder;
	}

	public void setupMoveCharacter(GameObject characterGameObject, Calendar cal, Character character)
	{
		MoveCharacter moveCharacter = characterGameObject.AddComponent<MoveCharacter> ();

		moveCharacter.character = character;
		moveCharacter.cal = cal;
	}

	public void setupCharacter(GameObject characterGameObject, Character character, AllData allData, string enemyFaction, Vector3 pos, bool isPlayer, int row, int col)
	{
		CharacterGameObject characterGameObjectScript = characterGameObject.AddComponent<CharacterGameObject> ();

		characterGameObjectScript.character = character;
		characterGameObjectScript.allData = allData;
		characterGameObjectScript.enemyFaction = enemyFaction;
		characterGameObjectScript.currentHealth = character.characterClass.totalHealth;
		characterGameObjectScript.row = row;
		characterGameObjectScript.col = col;
		characterGameObjectScript.isPlayer = isPlayer;
		characterGameObjectScript.path = new List<GameObject> ();
		characterGameObjectScript.currentDir = CharacterGameObject.dir.South;
	}

}
