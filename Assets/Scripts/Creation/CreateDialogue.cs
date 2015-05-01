using UnityEngine;
using System.Collections;

public class CreateDialogue : MonoBehaviour 
{
	public AllData allData;

	public void createDialogue
	(
		GameObject folder
	)
	{
		Dialogue newDialogue = folder.AddComponent<Dialogue> ();

		newDialogue.id = 0;
		newDialogue.dialogueStepText.Add ("I'm an NPC");
		newDialogue.dialogueStepText.Add ("I'm really just here to test the dialogue");

		allData.dialogues.Add (newDialogue);

	}

}
