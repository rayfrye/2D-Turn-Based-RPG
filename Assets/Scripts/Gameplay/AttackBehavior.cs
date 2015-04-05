using UnityEngine;
using System.Collections;

public class AttackBehavior : MonoBehaviour 
{
	public float attackCoolDownTime;
	public float nextAttackTime;
	public Character character;
	public Calendar cal;

	public void attackBehavior(GameObject target)
	{
		switch(character.characterClass.charClassName)
		{
		case "Infantry":
		{
			if(!attackIsOnCooldown())
			{
				doInfantryAttack(target);
			}
			break;
		}
		case "Archer":
		{
			if(!attackIsOnCooldown())
			{
				doArcheryAttack (target);
			}
			break;
		}
		default:
		{
			break;
		}
		}
	}

	public bool attackIsOnCooldown()
	{
		if(cal.gameTime >= nextAttackTime)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	public void doInfantryAttack(GameObject target)
	{
		int damage = calculateDamage(target);

		doDamage(target, damage);
				
		nextAttackTime = cal.gameTime + attackCoolDownTime;
	}
	
	public void doArcheryAttack(GameObject target)
	{
		int damage = calculateDamage(target);

		doDamage(target, damage);
		
		nextAttackTime = cal.gameTime + attackCoolDownTime;
	}

	public void doDamage(GameObject target, int damage)
	{
		int targetHealth = target.GetComponent<CharacterGameObject>().currentHealth;

		if(targetHealth > damage)
		{
			target.GetComponent<CharacterGameObject>().currentHealth -= damage;
		}
		else
		{
			targetHealth = 0;
			print ("Target dead!");
		}
	}

	public int calculateDamage(GameObject target)
	{
		int actualAttack = (int) Random.Range (0,character.attack());
		int damage = Mathf.Max (actualAttack - target.GetComponent<CharacterGameObject>().character.defense(),0);

		Debug.Log (name + " attacks " + target.name + " for " + damage + " damage.");

		return damage;
	}
}
