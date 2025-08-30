using System.Linq;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Dialogue Trigger", menuName = "Dialogue/Dialogue Trigger")]
public class DialogueTrigger : ScriptableObject
{
    [SerializeField] private Dialogue[] dialogues;
    [SerializeField] private Dialogue[] endlessDialogues;
    private int currentSentence;
    public static DialogueTrigger activeTrigger;
    private Dialogue currentDialogue;
    [SerializeField] private List<Dialogue> playOnlyOnce;
    private List<Dialogue> played = new List<Dialogue>();
    
    private void OnEnable()
    {
        // Ensure per-run state is cleared when the asset is loaded (e.g., entering Play Mode)
        played.Clear();
    }
    
    public void TriggerDialogue(int dialogueIndex) {
        Debug.LogError("TRIGGER DIALOGUE");
        if (dialogues.Length <= dialogueIndex)
        {
            currentDialogue = endlessDialogues[Random.Range(0, endlessDialogues.Length)];
        }
        else
        {
            currentDialogue = dialogues[dialogueIndex];
        }
        if(playOnlyOnce.Contains(currentDialogue) && played.Contains(currentDialogue)) {
            Debug.LogError("ALREADY PLAYED");
            Debug.LogError(string.Join(", ", played.Select(d => d != null ? d.name : "null")));
            return;
        }

        played.Add(currentDialogue);
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
