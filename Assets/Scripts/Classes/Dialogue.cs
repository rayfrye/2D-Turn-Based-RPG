using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue : MonoBehaviour 
{
	public int id;

	public List<string> dialogueStepText = new List<string>();
	public List<string[]> dialogueOptions = new List<string[]>();
	public List<string[]> dialogueActions = new List<string[]>();

}
