using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue : MonoBehaviour 
{
	public int id;

	public List<string> dialogueStepText = new List<string>();
	public List<List<string>> dialogueOptions = new List<List<string>>();
	public List<List<string>> dialogueActions = new List<List<string>>();

}