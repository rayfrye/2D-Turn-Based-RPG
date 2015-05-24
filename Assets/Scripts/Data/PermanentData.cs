using UnityEngine;
using System.Collections;

public class PermanentData : MonoBehaviour 
{
	public string currentLevel;
	public int currentDoorNum ;
	public bool alreadyLoadedData;
	
	public int playerCharacterID;

	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad (this);
		alreadyLoadedData = false;
	}
}
