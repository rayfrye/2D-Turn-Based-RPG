using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ConvertData : MonoBehaviour 
{
	public bool convertStringToBool(string s)
	{
		if(s == "TRUE")
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public int convertStringToInt(string s)
	{
		return int.Parse (s);
	}

	public float convertStringtoFloat(string s)
	{
		return float.Parse (s);
	}

	public List<string> convertStringtoListString(string s)
	{
		List<string> list = new List<string>();

		int noOfValues = s.Count(f => f == '|');

		string[] values = s.Split ('|');
		
		if(noOfValues > 0)
		{
			for(int i = 0; i < noOfValues; i++)
			{
				list.Add (values[i]);
			}
		}

		return list;
	}

	public List<int> convertStringtoListInt(string s)
	{
		List<int> list = new List<int>();
		
		int noOfValues = s.Count(f => f == '|');

		string[] values = s.Split ('|');

		if(noOfValues > 0)
		{
			for(int i = 0; i < noOfValues; i++)
			{
				list.Add (int.Parse (values[i]));
			}
		}
		
		return list;
	}
}