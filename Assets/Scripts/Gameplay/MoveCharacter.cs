using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MoveCharacter : MonoBehaviour 
{
	public Character character;
	public Calendar cal;

	public List<GameObject> waypoints = new List<GameObject>();
	public int currentWaypoint;

	void Start ()
	{
		cal = GameObject.Find("GameData").GetComponent<Calendar>();
	}
	
	public void moveTowardsTarget(Vector3 targetLocation)
	{
		transform.position = Vector3.MoveTowards(transform.position,targetLocation,character.characterClass.moveSpeed * Time.deltaTime * cal.timeSpeed);
	}

	public void moveAwayFromTarget(Vector3 targetLocation)
	{
		transform.position = Vector3.MoveTowards(transform.position,targetLocation ,- character.characterClass.moveSpeed * Time.deltaTime * cal.timeSpeed);
	}
	
	
}
