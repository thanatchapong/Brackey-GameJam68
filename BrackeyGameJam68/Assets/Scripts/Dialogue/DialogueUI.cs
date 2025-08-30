using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI sentenceText;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Image image;


    //Sound
    [SerializeField] private AudioClip dialogueAudio;


    //skip Dialogoue
    float PressedTime;
    bool isPressed = false;


    //Singleton
    public static DialogueUI instance;
    public bool isActive = false;
    [SerializeField] private float secondPerChar = 0.06f;
    private float charTimer = 0f;
    private bool isTyping = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void SetDialogueLine(string sentence, string speaker, Sprite sprite)
    {
        isActive = true;
        dialoguePanel.SetActive(true);

        image.sprite = sprite;
        nameText.text = speaker;
        sentenceText.text = sentence;
        sentenceText.maxVisibleCharacters = 0;

        isTyping = true;
        charTimer = 0f;
    }

    public void EndDialogue()
    {
        Time.timeScale = 1;
        isActive = false;
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (!isActive || !sentenceText) return;

        charTimer += Time.unscaledDeltaTime;
        while (charTimer >= secondPerChar && sentenceText.maxVisibleCharacters < sentenceText.text.Length)
        {
            AudioManager.instance.PlaySound(dialogueAudio, 0.1f);
            sentenceText.maxVisibleCharacters++;
            charTimer -= secondPerChar;
        }
        isTyping = sentenceText.maxVisibleCharacters < sentenceText.text.Length;

        if (Pressed())
        {

            if (isTyping)
            {
                sentenceText.maxVisibleCharacters = sentenceText.text.Length;
                isTyping = false;
            }
            else if (DialogueTrigger.activeTrigger != null)
            {
                DialogueTrigger.activeTrigger.NextLine();
            }
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return))
        {
            if (!isPressed)
            {
                PressedTime = charTimer;
                isPressed = true;
            }

            if (charTimer - PressedTime >= 1f)
            {
                DialogueTrigger.activeTrigger.SkipDialogue();
            }
        }
        else
        {
            isPressed = false;
        }
    }

    private bool Pressed() =>
        Input.GetKeyDown(KeyCode.Space) ||
        Input.GetKeyDown(KeyCode.E) ||
        Input.GetKeyDown(KeyCode.Return) ||
        Input.GetMouseButtonDown(0);
}
