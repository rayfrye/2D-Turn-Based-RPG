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
		,List<string> dialogue
	)
	{
		GameObject newCharacterGameObject = new GameObject();
		newCharacterGameObject.transform.parent = folder.transform;
		newCharacterGameObject.name = character.characterName;
		newCharacterGameObject.tag = tag;
		
		setupTransform (newCharacterGameObject.transform, pos, new Vector3 (1, 1, 1));
		setupMoveCharacter (newCharacterGameObject, cal, character);
		setupCharacter (newCharacterGameObject, character, allData, faction, enemyFaction, pos, isPlayer, row, col, dialogue);

		setupEquipment(newCharacterGameObject);
		CharacterGameObject characterGo = newCharacterGameObject.GetComponent<CharacterGameObject>();

		setupSprite (newCharacterGameObject, pos, characterGo.currentBody, "body", new Color32(248,205,154,255), 5);
		setupSprite (newCharacterGameObject, pos, characterGo.currentHair, "hair", new Color32(255,255,255,255), 6);
		setupSprite (newCharacterGameObject, pos, characterGo.currentHat, "hat", new Color32(255,255,255,255), 7);
		setupSprite (newCharacterGameObject, pos, characterGo.currentShirt, "shirt", new Color32(255,255,255,255), 6);
		setupSprite (newCharacterGameObject, pos, characterGo.currentPants, "pants", new Color32(255,255,255,255), 6);
		
		GameObject.Find ("Cell_" + newCharacterGameObject.GetComponent<CharacterGameObject> ().row + "_" + newCharacterGameObject.GetComponent<CharacterGameObject> ().col).GetComponent<Cell> ().isWalkable = false;

		allData.characterGameObjects.Add (newCharacterGameObject);

		return newCharacterGameObject;
	}

	public void setupEquipment(GameObject go)
	{
		CharacterGameObject characterGameObject = go.GetComponent<CharacterGameObject> ();

		characterGameObject.race = "human";
		characterGameObject.currentBody = "Squire_body";
		characterGameObject.currentHat = "hat";
		characterGameObject.currentHair = "Squire_hair";
		characterGameObject.currentShirt = "Squire_shirt";
		characterGameObject.currentPants = "Squire_pants";
	}

	public void setupSprite(GameObject characterGameObject, Vector3 pos, string spriteName, string spriteType, Color32 spriteColor, int spriteLayerOrder)
	{
		GameObject go = new GameObject ();
		go.name = spriteName;
		
		go.transform.parent = characterGameObject.transform;
		
		setupTransform (go.transform, pos, new Vector3(1,1,1));
		setupSpriteRenderer (go, "Sprites/"+spriteName, spriteLayerOrder, spriteColor);
		setupAnimation (characterGameObject, go, spriteType);
	}

	public void setupAnimation(GameObject go, GameObject animationGameObject, string animationPart)
	{
		CharacterGameObject characterGameObject = go.GetComponent<CharacterGameObject> ();
		Animator animator = animationGameObject.AddComponent<Animator> ();
		animator.speed = .6f;

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
		case "hair":
		{
			characterGameObject.hairAnimator = animator;
			break;
		}
		case "shirt":
		{
			characterGameObject.shirtAnimator = animator;
			break;
		}
		case "pants":
		{
			characterGameObject.pantsAnimator = animator;
			break;
		}
		default:
		{
			break;
		}
		}
	}

	public void setupTransform(Transform characterTransform, Vector3 pos, Vector3 scale)
	{
		characterTransform.position = pos;
		characterTransform.localScale = scale;
	}

	public void setupSpriteRenderer(GameObject characterGameObject, string spriteName, int sortingOrder,Color32 spriteColor)
	{
		Debug.Log ("At some point want to store sprite with character class/character");

		SpriteRenderer spriteRenderer = characterGameObject.AddComponent<SpriteRenderer> ();

		spriteRenderer.sprite = Resources.Load<Sprite>(spriteName);
		spriteRenderer.sortingOrder = sortingOrder;

		spriteRenderer.color = spriteColor;
	}

	public void setupMoveCharacter(GameObject characterGameObject, Calendar cal, Character character)
	{
		MoveCharacter moveCharacter = characterGameObject.AddComponent<MoveCharacter> ();

		moveCharacter.character = character;
		moveCharacter.cal = cal;
	}

	public void setupCharacter(GameObject characterGameObject, Character character, AllData allData, string faction, string enemyFaction, Vector3 pos, bool isPlayer, int row, int col, List<string> dialogue)
	{
		CharacterGameObject characterGameObjectScript = characterGameObject.AddComponent<CharacterGameObject> ();

		characterGameObjectScript.character = character;
		characterGameObjectScript.allData = allData;
		characterGameObjectScript.faction = faction;
		characterGameObjectScript.enemyFaction = enemyFaction;
		characterGameObjectScript.currentHealth = character.characterClass.totalHealth;
		characterGameObjectScript.row = row;
		characterGameObjectScript.col = col;
		characterGameObjectScript.isPlayer = isPlayer;
		characterGameObjectScript.path = new List<GameObject> ();
		characterGameObjectScript.currentDir = CharacterGameObject.dir.South;
		characterGameObjectScript.dialogue = dialogue;
	}

}
