using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using System;

public class LevelController : MonoBehaviour
{
    [Title("References")]
    [SerializeField] LevelDataSO _levelData;
    [SerializeField] EffectsDataSO _effectsData;
    [SerializeField, ReadOnly] CardManager _cardManager;
    [SerializeField] Transform _deckParent;
    [SerializeField] List<Transform> _cardAnchors;
    [SerializeField] Transform playCardAnchor;
    [SerializeField] TextMeshPro _deckCount;
    [SerializeField] WinScreen _winScreen;
    [SerializeField] LoseScreen _loseScreen;

    [Title("Player Hand")]
    [SerializeField, ReadOnly] List<CardBehaviour> _playerDeck;
    [SerializeField, ReadOnly] List<CardBehaviour> _playerHand;

    [Title("Effect States")]
    [SerializeField, ReadOnly] bool isHunger;
    [SerializeField, ReadOnly] int roundsInHunger = 0;
    [SerializeField, ReadOnly] bool isThirst;
    [SerializeField, ReadOnly] int roundsInThirst = 0;
    [SerializeField, ReadOnly] bool isSick;
    [SerializeField, ReadOnly] int roundsInSickness = 0;

    [Title("Defences Data")]
    [SerializeField, ReadOnly] int landDefences = 0;
    [SerializeField, ReadOnly] int airDefences = 0;
    [SerializeField, ReadOnly] int magicDefence = 0;
    [SerializeField] float modifierByDefence;

    [Title("Recovery Data")]
    [SerializeField] float recoveryOdds;

    [Title("Control")]
    [SerializeField, ReadOnly] RoundPhases _currentRoundPhase;
    [SerializeField, ReadOnly] int _remainingCitizens;
    [SerializeField, ReadOnly] int roundsPlayed = 0;

    public static Action<RoundPhases> OnRoundPhaseChanged = null;
    public static Action<int> OnCitizensQuantityChanged = null;
    public static Action OnLevelStart = null;


    public Transform DeckParent => _deckParent;

    public RoundPhases RoundPhase => _currentRoundPhase;

    private void Awake()
    {
        _cardManager = FindObjectOfType<CardManager>();
    }

    private void OnEnable()
    {
        CardBehaviour.OnCardPlayed += OnCardPlayed;
        CardBehaviour.OnCardDiscarded += OnCardDiscarded;
        DialogueManager.OnDialogueFinish += StartRound;
    }

    private void OnDisable()
    {
        CardBehaviour.OnCardPlayed -= OnCardPlayed;
        CardBehaviour.OnCardDiscarded -= OnCardDiscarded;
        DialogueManager.OnDialogueFinish -= StartRound;
    }

    private void Start()
    {
        _playerDeck = _cardManager.GetPlayerDeck(_levelData.deckSize, this);
        if (_playerDeck == null) return;

        _remainingCitizens = _levelData.totalCitizens;

        OnCitizensQuantityChanged?.Invoke(_remainingCitizens);

        UpdateDeckCount();

        OnLevelStart?.Invoke();

        StartRound();
    }

    void StartRound()
    {
        if (_remainingCitizens <= 0)
        {
            _winScreen.SetupScreen(_playerDeck.Count);
            _winScreen.gameObject.SetActive(true);
            GameManager.Instance.SaveScore(_levelData.levelName);
            return;
        }

        if(_playerDeck.Count < _levelData.playerHandSize)
        {
            _loseScreen.gameObject.SetActive(true);
            return;
        }

        roundsPlayed++;
        GetPlayerHand();
        ChangeRoundPhase(RoundPhases.Discard);
    }

    void UpdateDeckCount()
    {
        _deckCount.text = _playerDeck.Count.ToString();
    }

    void OnCardDiscarded(CardBehaviour card)
    {

        _playerHand.Remove(card);
        Destroy(card.gameObject);

        ChangeRoundPhase(RoundPhases.PlayCard);
    }

    void OnCardPlayed(CardBehaviour card)
    {
        for (int i = 0; i < _playerHand.Count; i++)
        {
            if (_playerHand[i] != card)
            {
                ReturnToDeck(_playerHand[i]);
            }
        }

        card.transform.position = playCardAnchor.position;

        TryCitizensResponse(card);

        CheckOngoingEffects();
    }

    void TryCitizensResponse(CardBehaviour card)
    {
        var result = UnityEngine.Random.Range(0, 1f);

        if(result <= _levelData.citizensResponseOdds)
        {
            CitizensResponseBehaviour(card);
        }
    }

    void ReturnToDeck(CardBehaviour card)
    {
        _playerDeck.Add(card);
        _playerHand.Remove(card);
        card.gameObject.SetActive(false);
        UpdateDeckCount();
    }

    void GetPlayerHand()
    {
        if (_playerHand.Count > 0)
        {
            for (int i = 0; i < _playerHand.Count; i++)
            {
                var card = _playerHand[i];
                _playerHand.RemoveAt(i);
                Destroy(card.gameObject);
            }
        }

        for (int i = 0; i < _levelData.playerHandSize; i++)
        {
            var selectedCard = _playerDeck[i];

            selectedCard.gameObject.SetActive(true);
            selectedCard.transform.position = _cardAnchors[i].transform.position;

            _playerHand.Add(selectedCard);
        }

        foreach(CardBehaviour c in _playerHand)
        {
            _playerDeck.Remove(c);
        }

        UpdateDeckCount();
    }

    public float CheckDefences(CardType cardType)
    {
        // There are no defences for Natural Disasters and Resource types
        switch (cardType)
        {
            case CardType.Air:
                return airDefences * modifierByDefence;

            case CardType.Land:
                return landDefences * modifierByDefence;

            case CardType.Magic:
                return magicDefence * modifierByDefence;

            default:
                return 0;
        }
    }

    public void EnableEffect(List<CardEffects> effects)
    {
        foreach (CardEffects effect in effects)
        {
            switch (effect)
            {
                case CardEffects.Hunger:
                    if (isHunger)
                    {
                        // Display already Hunger message
                        DialogueManager.Instance.AddDialogueToQueue(_effectsData.GetAlreadyHungerMessage());
                    }
                    else
                    {
                        // Display Hunger message
                        DialogueManager.Instance.AddDialogueToQueue(_effectsData.GetHungerMessage());
                        isHunger = true;
                    }

                    break;

                case CardEffects.Thirst:
                    if (isThirst)
                    {
                        // Display already thirst message
                        DialogueManager.Instance.AddDialogueToQueue(_effectsData.GetAlreadyThirstMessage());
                    }
                    else
                    {
                        // Display thirst message
                        DialogueManager.Instance.AddDialogueToQueue(_effectsData.GetThirstMessage());
                        isThirst = true;
                    }

                    break;

                case CardEffects.Sickness:
                    if (isSick)
                    {
                        // Display already sick message
                        DialogueManager.Instance.AddDialogueToQueue(_effectsData.GetAlreadySickMessage());
                    }
                    else
                    {
                        // Display sick message
                        DialogueManager.Instance.AddDialogueToQueue(_effectsData.GetSickMessage());
                        isSick = true;
                    }

                    break;
            }
        }
    }

    void CheckOngoingEffects()
    {
        if (isHunger)
        {
            roundsInHunger++;

            if (roundsInHunger >= _effectsData.hungerMaxRounds)
            {
                isHunger = false;
                DialogueManager.Instance.AddDialogueToQueue(_effectsData.hungerEndedMessage);
            }
            else
            {
                var recovered = RollEffectRecovery();

                if (recovered)
                {
                    isHunger = false;
                    DialogueManager.Instance.AddDialogueToQueue(_effectsData.hungerRecoveryMessage);
                }
            }

            // If there was no recovery
            if (isHunger)
            {
                KillCitizens(_effectsData.hungerCasualtiesPerRound);
            }
        }

        if (isThirst)
        {
            roundsInThirst++;

            if (roundsInThirst >= _effectsData.thirstMaxRounds)
            {
                isThirst = false;
                DialogueManager.Instance.AddDialogueToQueue(_effectsData.thirstEndedMessage);
            }
            else
            {
                var recovered = RollEffectRecovery();

                if (recovered)
                {
                    isThirst = false;
                    DialogueManager.Instance.AddDialogueToQueue(_effectsData.thirstRecoveryMessage);
                }
            }

            // If there was no recovery
            if (isThirst)
            {
                KillCitizens(_effectsData.hungerCasualtiesPerRound);
            }
        }

        if (isSick)
        {
            roundsInSickness++;

            if (roundsInSickness >= _effectsData.sicknessMaxRounds)
            {
                isSick = false;
                DialogueManager.Instance.AddDialogueToQueue(_effectsData.sicknessEndedMessage);
            }
            else
            {
                var recovered = RollEffectRecovery();

                if (recovered)
                {
                    isSick = false;
                    DialogueManager.Instance.AddDialogueToQueue(_effectsData.sicknessRecoveryMessage);
                }
            }

            // If there was no recovery
            if (isSick)
            {
                KillCitizens(_effectsData.sicknessCasualtiesPerRound);
            }
        }
    }

    bool RollEffectRecovery()
    {
        var result = UnityEngine.Random.Range(0, 1f);

        if (result <= _effectsData.effectRecoveryChance)
            return true;
        return false;
    }

    public void KillCitizens(int maxPossibility)
    {
        var citizensQuantity = UnityEngine.Random.Range(1, maxPossibility + 1);

        if (_remainingCitizens - citizensQuantity < 0)
        {
            citizensQuantity = _remainingCitizens;
            _remainingCitizens = 0;
        }
        else
            _remainingCitizens -= citizensQuantity;

        GameManager.Instance.AddPlayerScore(citizensQuantity);

        DialogueManager.Instance.AddDialogueToQueue($"{citizensQuantity} citizens died!");

        OnCitizensQuantityChanged?.Invoke(_remainingCitizens);

    }

    void ChangeRoundPhase(RoundPhases roundPhase)
    {
        _currentRoundPhase = roundPhase;
        OnRoundPhaseChanged?.Invoke(roundPhase);
    }

    protected void RunResponse(CitizensResponse response)
    {
        GameObject responseObject = response.ResponseObject;
        bool activateObject = response.ActivateObject;
        string responseMessage = response.ResponseMessage;

        if (responseObject != null)
            responseObject.SetActive(activateObject);

        switch (response.ResponseType)
        {
            case CardType.Land:
                landDefences++;
                break;
            case CardType.Air:
                airDefences++;
                break;
            case CardType.Magic:
                magicDefence++;
                break;

            default:
                break;
        }

        DialogueManager.Instance.AddDialogueToQueue(responseMessage);
    }

    protected virtual void LevelChanges() { }

    protected virtual void CitizensResponseBehaviour(CardBehaviour card) { }

}

public enum RoundPhases
{
    None,
    Discard,
    PlayCard,
}
