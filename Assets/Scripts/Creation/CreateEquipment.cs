using UnityEngine;
using System.Collections;

public class CreateEquipment : MonoBehaviour 
{
	public Equipment createEquipment
	(
		GameObject folder
		,int id
		,string name
		,string desc
		,int typeID
		,int moveSpeed
		,int addAttack
		,int addDefense
		,int addAttackRange		
	)
	{
		GameObject go = new GameObject();
		go.transform.parent = folder.transform;

		Equipment newEquipment = go.AddComponent<Equipment> ();



		return newEquipment;
	}
}
