using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SaveData : MonoBehaviour 
{
	public void saveData(List<Quest> quests)
	{
		saveQuestData(quests);
	}

	public void saveQuestData(List<Quest> quests)
	{
		string[] questData = new string[quests.Count+1];

		questData[0] = "id,name,desc,iscompleted";

		foreach(Quest quest in quests)
		{
			questData[quest.id+1] = quest.id.ToString() + "," + quest.name + "," + quest.description + "," + quest.isComplete.ToString ().ToUpper();
		}

		File.WriteAllLines("./Assets/Resources/CSV/GameData/quests.csv",questData);
	}

}
