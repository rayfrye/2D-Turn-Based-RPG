using UnityEngine;
using System.Collections;

public class CreateQuest : MonoBehaviour 
{
	public AllData allData;
	
	public void createQuest
	(
		GameObject folder
	)
	{
		Quest newQuest = folder.AddComponent<Quest> ();
		
		newQuest.id = 0;
		newQuest.name = "Starting Out";
		newQuest.description = "Dummy quest for dummies";
		newQuest.isComplete = false;
		
		allData.quests.Add (newQuest);
		
	}


}
