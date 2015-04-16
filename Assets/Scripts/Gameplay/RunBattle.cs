using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunBattle : MonoBehaviour 
{
	public AllData allData;
	public int currentCharacterTurnIndex;
	public Pathfinder pathfinder;
	public battleStateType currentBattleState;
	public Calendar cal;
	public int currentNavNode;

	public enum battleStateType
	{
		GetTarget,
		GetFirstAction,
		AssignPath,
		AttackTarget,
		MoveTowardsTarget,
		FinishedMoving,
		TurnFinished
	}

	void Update()
	{
		if (allData.finishedLoading) 
		{
			runBattle (allData.characterGameObjects,allData.cells);
		}
	}
	
	public bool runBattle
	(
		List<GameObject> sortedCharcters
		,GameObject[,] grid
	)
	{
		CharacterGameObject currentCharacter = getCurrentCharacterGameObject(sortedCharcters);

		determineNextAction (currentCharacter);

		return false;
	}

	void determineNextAction(CharacterGameObject currentCharacter)
	{
		switch (currentBattleState) 
		{
		case battleStateType.GetTarget:
		{
			currentCharacter.currentTarget = getNearestTarget (currentCharacter);
			currentBattleState = battleStateType.GetFirstAction;
			break;
		}
		case battleStateType.GetFirstAction:
		{
			if(pathfinder.calcDistance (currentCharacter,currentCharacter.currentTarget) <= currentCharacter.character.attackRange())
			{
				currentBattleState = battleStateType.AttackTarget;
			}
			else
			{
				currentBattleState = battleStateType.AssignPath;
			}
			break;
		}
		case battleStateType.AttackTarget:
		{
			if(attackTarget(currentCharacter))
			{
				currentBattleState = battleStateType.TurnFinished;
			}
			break;
		}
		case battleStateType.AssignPath:
		{
			assignPath(currentCharacter);
			currentNavNode = 0;

			if(currentCharacter.distanceToTarget > 1)
			{
				currentBattleState = battleStateType.MoveTowardsTarget;
			}
			else
			{
				currentBattleState = battleStateType.TurnFinished;
			}
			break;
		}
		case battleStateType.MoveTowardsTarget:
		{
			if(NPCMoveAlongPath(currentCharacter))
			{
				currentBattleState = battleStateType.FinishedMoving;
			}
			break;
		}
		case battleStateType.FinishedMoving:
		{
			if(pathfinder.calcDistance (currentCharacter,currentCharacter.currentTarget) <= currentCharacter.character.attackRange())
			{
				currentBattleState = battleStateType.AttackTarget;
			}
			else
			{
				currentBattleState = battleStateType.TurnFinished;
			}
			break;
		}
		case battleStateType.TurnFinished:
		{
			print ("Turn is Finished");
			if(currentCharacterTurnIndex < allData.characterGameObjects.Count - 1)
			{
				currentCharacterTurnIndex++;
			}
			else
			{
				currentCharacterTurnIndex = 0;
			}

			currentBattleState = battleStateType.GetTarget;
			break;
		}
		default:
		{
			currentBattleState = battleStateType.TurnFinished;
			break;
		}
		}
	}

	CharacterGameObject getCurrentCharacterGameObject(List<GameObject> characterGameObjects)
	{
		return characterGameObjects[currentCharacterTurnIndex].GetComponent<CharacterGameObject>();
	}

	CharacterGameObject getNearestTarget(CharacterGameObject currentCharacter)
	{
		GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag (currentCharacter.enemyFactionTag);
		int compareDistance = 99999;
		CharacterGameObject target = currentCharacter;

		foreach (GameObject potentialTarget in potentialTargets)
		{
			int tempDistance = pathfinder.calcDistance(currentCharacter, potentialTarget.GetComponent<CharacterGameObject>());

			if(tempDistance  < compareDistance)
			{
				compareDistance = tempDistance;
				currentCharacter.distanceToTarget = tempDistance;
				target = potentialTarget.GetComponent<CharacterGameObject>();
			}
		}

		return target;
	}

	void assignPath(CharacterGameObject currentCharacter)
	{
		GameObject startCell = GameObject.Find ("Cell_" + currentCharacter.row + "_" + currentCharacter.col);
		GameObject endCell = GameObject.Find ("Cell_" + currentCharacter.currentTarget.row + "_" + currentCharacter.currentTarget.col);

		currentCharacter.path = pathfinder.path (startCell, endCell, allData.cells);
	}

	bool NPCMoveAlongPath(CharacterGameObject currentCharacter)
	{
		Transform currentCharacterTransform = currentCharacter.transform;
		float maxDistanceDelta = Time.deltaTime * cal.timeSpeed;

		if ((currentCharacterTransform.position - currentCharacter.path [currentNavNode].transform.position).sqrMagnitude > .001f) 
		{
			currentCharacterTransform.position = Vector3.MoveTowards (currentCharacterTransform.position, currentCharacter.path [currentNavNode].transform.position, maxDistanceDelta);
			return false;
		}
		else 
		{
			currentCharacterTransform.position = currentCharacter.path[currentNavNode].transform.position;

			if(currentNavNode == (currentCharacter.path.Count - 2) || currentNavNode == currentCharacter.character.moveSpeed ())
			{
				GameObject.Find ("Cell_" + currentCharacter.row + "_" + currentCharacter.col).GetComponent<Cell>().isWalkable = true;
				GameObject.Find ("Cell_" + currentCharacter.path[currentNavNode].GetComponent<Cell>().row + "_" + currentCharacter.path[currentNavNode].GetComponent<Cell>().col).GetComponent<Cell>().isWalkable = false;

				currentCharacter.row = currentCharacter.path[currentNavNode].GetComponent<Cell>().row;
				currentCharacter.col = currentCharacter.path[currentNavNode].GetComponent<Cell>().col;

				return true;
			}
			else
			{
				currentNavNode++;
				return false;
			}
		}
	}

	bool attackTarget(CharacterGameObject currentCharacter)
	{
		int targetHealth = currentCharacter.currentTarget.currentHealth;
		int damage = calcDamage(currentCharacter);
		
		if(targetHealth > damage)
		{
			currentCharacter.currentTarget.currentHealth -= damage;
		}
		else
		{
			currentCharacter.currentTarget.currentHealth = 0;
			print ("Target dead!");
		}

		return true;
	}

	public int calcDamage(CharacterGameObject currentCharacter)
	{
		int actualAttack = (int) Random.Range (0,currentCharacter.character.attack());
		int damage = Mathf.Max (actualAttack - currentCharacter.currentTarget.character.defense(),0);
		
		Debug.Log (currentCharacter.name + " attacks " + currentCharacter.currentTarget.name + " for " + damage + " damage.");
		
		return damage;
	}
}
