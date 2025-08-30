using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Trigger", menuName = "Dialogue/Dialogue Trigger")]
public class DialogueTrigger : ScriptableObject
{
    [SerializeField] private Dialogue[] dialogues;
    [SerializeField] private Dialogue[] endlessDialogues;
    private int currentSentence;
    public static DialogueTrigger activeTrigger;
    private Dialogue currentDialogue;
    
    public void TriggerDialogue(int dialogueIndex) {
        if(dialogues.Length <= dialogueIndex) {
            currentDialogue = endlessDialogues[Random.Range(0, endlessDialogues.Length)];
        }
        else {
            currentDialogue = dialogues[dialogueIndex];
        }

        currentSentence = 0;
        activeTrigger = this;

        Time.timeScale = 0;
        
        ShowCurrentLine();
    }

    public void NextLine() {
        currentSentence++;
        if(currentSentence >= currentDialogue.sentences.Length) {
            DialogueUI.instance.EndDialogue();
            currentSentence = 0;
            activeTrigger = null;
            return;
        }
        
        Time.timeScale = 0;

        ShowCurrentLine();
    }

    public void ShowCurrentLine() {
        string speaker = "";
        Sprite sprite = null;
        
        Time.timeScale = 0;
        
        foreach (var nameMap in currentDialogue.nameMaps)
        {
            if (nameMap.sentencesMap.Contains(currentSentence))
            {
                speaker = nameMap.name;
                sprite = nameMap.image;
                break;
            }
        }
        DialogueUI.instance.SetDialogueLine(
            currentDialogue.sentences[currentSentence],
            speaker,
            sprite
        );
    }
}
