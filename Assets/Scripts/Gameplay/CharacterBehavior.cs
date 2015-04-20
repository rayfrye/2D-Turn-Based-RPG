using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CharacterBehavior : MonoBehaviour 
{
	public GameObject target;
	public List<GameObject> potentialTargets = new List<GameObject>();

	public CharacterGameObject characterGameObject;
	public MoveCharacter moveCharacter;
	public AttackBehavior attackBehavior;
	
	// Update is called once per frame
	void Update () 
	{
		//determineNextAction();
	}

	void determineNextAction()
	{
		if(target == null)
		{
			getTarget();
		}
		else
		{
			if(!isInOptimalAttackRange())
			{
				moveToOptimalAttackRange();
			}
			else
			{
				attackTarget();
			}
		}
	}

	public void getTarget()
	{
		potentialTargets = GameObject.FindGameObjectsWithTag("NPC").ToList();
		float tempDistance = 99999;
		GameObject tempTarget = gameObject;
		
		foreach(GameObject potentialTarget in potentialTargets)
		{
			Debug.Log ("Should there be a more advanced way than just finding the nearest target? Can we find the highest priority target?");
			float tempTargetDistance = (transform.position - potentialTarget.transform.position).sqrMagnitude;

			if(potentialTarget.GetComponent<CharacterGameObject>().faction == characterGameObject.enemyFaction)
			{
				if(tempTargetDistance < tempDistance)
				{
					tempDistance = tempTargetDistance;
					tempTarget = potentialTarget;
				}
			}
		}
		
		if(tempTarget != gameObject)
		{
			target = tempTarget;
		}
	}

	public bool isInOptimalAttackRange()
	{
		Debug.Log ("Add min and max optimal range to character class, determine how close character is to target and move based on that.");

		bool isInOptimalAttackRange = false;

		if((transform.position - target.transform.position).sqrMagnitude < 5)
		{
			isInOptimalAttackRange = true;
		}
			
		return isInOptimalAttackRange;
	}

	public void moveToOptimalAttackRange()
	{
		moveCharacter.moveTowardsTarget(target.transform.position);
	}

	public void attackTarget()
	{
		attackBehavior.attackBehavior(target);
	}
}
