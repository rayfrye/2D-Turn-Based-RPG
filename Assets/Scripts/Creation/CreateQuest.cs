using UnityEngine;
using System.Collections;

public class CreateQuest : MonoBehaviour 
{
	public AllData allData;
	
	public void createQuest
	(
		GameObject folder
		,int id
		,string name
		,string description
		,bool isComplete
	)
	{
		Quest newQuest = folder.AddComponent<Quest> ();
		
		newQuest.id = id;
		newQuest.name = name;
		newQuest.description = description;
		newQuest.isComplete = isComplete;
		
		allData.quests.Add (newQuest);
	}
}
