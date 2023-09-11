using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Title("References")]
    [SerializeField] TextMeshPro _dialogueText;
    [SerializeField, ReadOnly] string currentDialogue;
    [SerializeField, ReadOnly] List<string> dialogueQueue;

    [Title("Control")]
    [SerializeField, Sirenix.OdinInspector.ReadOnly] bool writingLine;
    [SerializeField] float writeSpeed;

    Action currentCallback = null;

    public static Action OnDialogueStart = null;
    public static Action OnDialogueFinish = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        LevelController.OnRoundPhaseChanged += SetPhaseDialogue;
    }

    private void OnDisable()
    {
        LevelController.OnRoundPhaseChanged -= SetPhaseDialogue;
    }


    public void AddDialogueToQueue(string dialogue)
    {
        dialogueQueue.Add(dialogue);
    }

    // Starts a new dialogue, starting from the last line if dialogue has been completed before
    public void StartDialogueRoutine(string dialogue, Action callback = null)
    {
        GameManager.Instance.OnDialogue = true;
        _dialogueText.text = string.Empty;
        currentDialogue = dialogue;
        currentCallback = callback;
        StartCoroutine(DialogueRoutine(callback));
    }

    // Writes the dialogues lines character by character
    IEnumerator DialogueRoutine(Action callback)
    {
        foreach (char c in currentDialogue.ToCharArray())
        {
            writingLine = true;
            _dialogueText.text += c;
            yield return new WaitForSeconds(writeSpeed);
        }
        writingLine = false;

        yield return null;
    }

    // Starts next line of dialogue
    public void NextLine()
    {
        if (writingLine) return;

        if(dialogueQueue.Count == 0)
        {
            FinishDialogue();
        }
        else
        {
            StartDialogueRoutine(dialogueQueue[0]);
            dialogueQueue.RemoveAt(0);
        }
    }

    // Finishes the dialogue
    void FinishDialogue()
    {
        StopAllCoroutines();
        //_dialogueText.text = string.Empty;
        GameManager.Instance.OnDialogue = false;
        currentDialogue = null;

        OnDialogueFinish?.Invoke();
        currentCallback?.Invoke();
    }

    void SetPhaseDialogue(RoundPhases roundPhase)
    {
        switch (roundPhase)
        {
            case RoundPhases.Discard:
                _dialogueText.text = "Choose a card to be discarded!";
                break;
            case RoundPhases.PlayCard:
                _dialogueText.text = "Choose a card to be played. The remaining card will be sent to the bottom of your deck!";
                break;
        }
    }
}
