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
		newDialogue.dialogueStepText = "I'm an NPC";
		newDialogue.dialogueStepHasActions = true;
		newDialogue.dialogueActions.Add ("questID:0|step:0|");

		allData.dialogues.Add (newDialogue);

	}

}
