using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Trigger", menuName = "Dialogue/Dialogue Trigger")]
public class DialogueTrigger : ScriptableObject
{
    [SerializeField] private Dialogue[] dialogues;
    private int currentSentence;
    private int currentDialogueIndex = -1;
    public static DialogueTrigger activeTrigger;
    
    public void TriggerDialogue(int dialogueIndex) {
        if(dialogues.Length <= dialogueIndex) {
            Debug.LogError("Dialogue index out of bounds!");
            return;
        }

        currentDialogueIndex = dialogueIndex;
        currentSentence = 0;
        activeTrigger = this;
        
        ShowCurrentLine();
    }

    public void NextLine() {
        currentSentence++;
        Debug.Log(currentSentence + " " + currentDialogueIndex);
        if(currentSentence >= dialogues[currentDialogueIndex].sentences.Length) {
            DialogueUI.instance.EndDialogue();
            currentSentence = 0;
            activeTrigger = null;
            return;
        }

        ShowCurrentLine();
    }

    public void ShowCurrentLine() {
        Dialogue dialogue = dialogues[currentDialogueIndex];
        string speaker = "";
        Sprite sprite = null;
        foreach(var nameMap in dialogue.nameMaps) {
            if(nameMap.sentencesMap.Contains(currentSentence)) {
                speaker = nameMap.name;
                sprite = nameMap.image;
                break;
            }
        }
        DialogueUI.instance.SetDialogueLine(
            dialogue.sentences[currentSentence],
            speaker,
            sprite
        );
    }
}
