using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Quest : MonoBehaviour 
{
	public int id;

	public string name;
	public string description;

	public List<Quest> questPrereqs = new List<Quest>();

	public bool checkifComplete()
	{
		bool isComplete = true;

		foreach (Quest questPrereq in questPrereqs) 
		{
			if(!questPrereq.isComplete)
			{
				isComplete = false;
			}
		}

		return isComplete;
	}

	public bool isComplete;
}
