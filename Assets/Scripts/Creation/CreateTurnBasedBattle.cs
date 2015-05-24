using UnityEngine;
using System.Collections;

public class CreateTurnBasedBattle : MonoBehaviour 
{
	public void createTurnBasedBattle
	(
		AllData allData
		,Transform cameraTarget
	)
	{
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<SmoothCamera2D> ().target = cameraTarget;
	}
}
