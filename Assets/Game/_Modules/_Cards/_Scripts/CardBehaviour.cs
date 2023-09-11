using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using System.Linq;

public class CardBehaviour : MonoBehaviour
{
    [Title("Data")]
    [SerializeField] CardSO _cardData;

    [Title("LevelController")]
    [SerializeField, ReadOnly] LevelController _levelController;

    [Title("Object References")]
    [SerializeField] TextMeshPro _cardName;
    [SerializeField] TextMeshPro _cardEffects;
    [SerializeField] TextMeshPro _cardMaxCasualties;
    [SerializeField] TextMeshPro _cardOdds;

    [SerializeField] bool cardInPlay;

    public string CardDescription
    {
        get => _cardData.cardDescription;
    }

    public CardType CardType
    {
        get => _cardData.cardType;
    }


    public static Action<CardBehaviour> OnCardPlayed = null;
    public static Action<CardBehaviour> OnCardDiscarded = null;

    private void Start()
    {
        Setup();
    }

    // Setup the visual data on the cards based on the _cardData scriptable object
    [Button]
    void Setup()
    {
        _cardName.text = _cardData.cardName;
        _cardEffects.text = "";

        for (int i = 0; i < _cardData.cardEffects.Count; i++)
        {
            if (i > 0)
                _cardEffects.text += ", ";

            _cardEffects.text += _cardData.cardEffects[i].ToString();
        }


        _cardMaxCasualties.text = _cardData.MaxCitizenCasualty.ToString();
        _cardOdds.text = $"{(int)(_cardData.positiveOutcomeProbability * 100)}%";
    }

    // Executes the behaviour of the card depending on the round phase
    public void ExecuteBehaviour()
    {
        if (cardInPlay) return;

        if (_levelController == null) return;

        var roundPhase = _levelController.RoundPhase;

        switch (roundPhase)
        {
            case RoundPhases.None:
                return;
            case RoundPhases.PlayCard:
                PlayCard();
                break;
            case RoundPhases.Discard:
                DiscardCard();
                break;
            default:
                break;
        }

    }

    // Send the card discard event
    void DiscardCard()
    {
        OnCardDiscarded?.Invoke(this);
    }

    // Plays the card
    void PlayCard()
    {
        cardInPlay = true;

        var defencesModifier = _levelController.CheckDefences(_cardData.cardType);
        var outCome = _cardData.RollOutCome(defencesModifier);

        DialogueManager.Instance.StartDialogueRoutine(CardDescription);

        switch (outCome)
        {
            case OutcomeType.Positive:

                DialogueManager.Instance.AddDialogueToQueue(_cardData.positiveOutcomeDescription);
                _levelController.EnableEffect(_cardData.cardEffects);

                if (_cardData.MaxCitizenCasualty != 0)
                    _levelController.KillCitizens(_cardData.MaxCitizenCasualty);

                break;

            case OutcomeType.Negative:

                DialogueManager.Instance.AddDialogueToQueue(_cardData.negativeOutcomeDescription);

                break;

            default:
                break;
        }

        OnCardPlayed?.Invoke(this);
    }

    public void SetParent()
    {
        transform.SetParent(_levelController.DeckParent);
    }

    public void SetLevelController(LevelController levelController)
    {
        _levelController = levelController;
    }
}
