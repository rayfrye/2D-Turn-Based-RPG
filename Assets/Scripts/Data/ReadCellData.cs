using UnityEngine;
using System.Collections;

public class ReadCellData : MonoBehaviour 
{
	public string getCellValue(string cellString, string cellType)
	{
		string s = "";
		
		if(cellString.Length > 0 && cellString.Contains (cellType))
		{
			int prefixIndexStart = cellString.IndexOf(cellType)+cellType.Length + 1;
			
			string temp = cellString.Substring(prefixIndexStart);
			
			int prefixIndexEnd = temp.IndexOf("|");
			
			s = temp.Substring(0,prefixIndexEnd);
		}
		
		return s;
	}
	
}
