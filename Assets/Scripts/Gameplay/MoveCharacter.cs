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

//		currentWaypoint = 0;
//		waypoints = GameObject.FindGameObjectsWithTag("Waypoint").ToList ();
	}
	
	void Update ()
	{
		//getTarget();
		//moveNPC(targetLocation);
	}

//	void moveNPC(Vector2 destination)
//	{
//		transform.position = Vector3.MoveTowards(transform.position,destination,character.characterClass.moveSpeed * Time.deltaTime * cal.timeSpeed);
//	}
//
//	void getTarget()
//	{
//		if((transform.position - targetLocation).sqrMagnitude < 2)
//		{
//			if(currentWaypoint < waypoints.Count-1)
//			{
//				currentWaypoint++;
//			}
//			else
//			{
//				currentWaypoint = 0;
//			}
//		}
//
//		targetLocation = waypoints[currentWaypoint].transform.position;
//	}

	public void moveTowardsTarget(Vector3 targetLocation)
	{
		transform.position = Vector3.MoveTowards(transform.position,targetLocation,character.characterClass.moveSpeed * Time.deltaTime * cal.timeSpeed);
	}

	public void moveAwayFromTarget(Vector3 targetLocation)
	{
		transform.position = Vector3.MoveTowards(transform.position,targetLocation ,- character.characterClass.moveSpeed * Time.deltaTime * cal.timeSpeed);
	}
	
	
}
