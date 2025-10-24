using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using System.Collections;
using UnityEngine.InputSystem.LowLevel;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    public bool IsOngoing { get; private set; } = false;

    private Queue<DialogueLine> sentences;

    [Header("UI")]
    [SerializeField] private RectTransform dialogePanel;
    private CanvasGroup dialogeCanvasGroup;
    [SerializeField] private Image portriatImage;
    [SerializeField] private TMPro.TextMeshProUGUI nameText;
    [SerializeField] private TMPro.TextMeshProUGUI dialogueText;
    private float screenHeight;

    [Header("Animation")]
    [SerializeField] private float moveDistance = 60f;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease easeType = Ease.OutExpo;
    [SerializeField] private float textSpeed = 10f;

    private bool isTyping = false;
    private string currentSentence;
    private Coroutine typingCoroutine;
    private UnityEvent OnDialogueEnds;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        dialogeCanvasGroup = dialogePanel.GetComponent<CanvasGroup>();
        screenHeight = dialogePanel.rect.height;
        OnDialogueEnds = new UnityEvent();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sentences = new Queue<DialogueLine>();

        dialogePanel.gameObject.SetActive(false);
        dialogePanel.DOMoveY((screenHeight / 2f) + moveDistance, 0f);
        dialogeCanvasGroup.alpha = 0f;
        dialogeCanvasGroup.interactable = false;
        dialogeCanvasGroup.blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceDialogue();
        }
    }

    public void ShowDialogePanel()
    {
        dialogePanel.gameObject.SetActive(true);
        dialogePanel.DOMoveY(screenHeight / 2f, duration);
        dialogeCanvasGroup.DOFade(1f, 0.5f).OnComplete(() =>
        {
            dialogeCanvasGroup.interactable = true;
            dialogeCanvasGroup.blocksRaycasts = true;
        }).SetEase(easeType);
    }

    public void HideDialoguePanel()
    {
        dialogePanel.DOMoveY((screenHeight / 2f) + moveDistance, duration);
        dialogeCanvasGroup.interactable = false;
        dialogeCanvasGroup.blocksRaycasts = false;
        dialogeCanvasGroup.DOFade(0f, 0.5f).SetEase(easeType);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();
        IsOngoing = true;
        Cursor.lockState = CursorLockMode.None;

        foreach (DialogueLine line in dialogue.lines) 
        {
            sentences.Enqueue(line);
        }

        ShowDialogePanel();
        AdvanceDialogue();
    }

    public void AdvanceDialogue()
    {
        if (isTyping)
        {
            CompleteSentence(currentSentence);
            return;
        }

        if (sentences.Count <= 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = sentences.Dequeue();
        
        if (line.portrait != null && portriatImage != null)
            portriatImage.sprite = line.portrait;

        if (line.name != "" && nameText != null)
            nameText.SetText(line.name);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        currentSentence = line.text;
        typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;

        dialogueText.SetText("");
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.SetText(dialogueText.text + letter);
            yield return new WaitForSeconds(1f / textSpeed);
        }

        isTyping = false;
    }

    private void CompleteSentence(string sentence)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.SetText(sentence);
        isTyping = false;
    }

    public void EndDialogue()
    {
        HideDialoguePanel();
        OnDialogueEnds?.Invoke();
        IsOngoing = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SubscribeDialogueEndsEvent(UnityEvent ext)
    {
        OnDialogueEnds?.AddListener(ext.Invoke);
    }

    public void UnsubscribeDialogueEnds()
    {
        OnDialogueEnds?.RemoveAllListeners();
    }
}
