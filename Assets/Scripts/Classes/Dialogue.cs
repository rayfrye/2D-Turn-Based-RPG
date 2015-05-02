using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue : MonoBehaviour 
{
	public int id;

	public string text;

	public bool hasOptions;
	public List<string> options = new List<string>();
	public List<int> optionDests = new List<int>();

	public bool hasActions;
	public List<string> actions = new List<string>();

}