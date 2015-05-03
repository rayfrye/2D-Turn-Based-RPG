using UnityEngine;
using System.Collections;

public class CreateItems : MonoBehaviour 
{
	public AllData allData;

	public void createItems
	(
		GameObject folder
		,int id
		,string name
		,string desc
	)
	{
		Item newItem = folder.AddComponent<Item>();

		newItem.id = id;
		newItem.itemName = name;
		newItem.desc = desc;

		allData.items.Add (newItem);
	}

}
