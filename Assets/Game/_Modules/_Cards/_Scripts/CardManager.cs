using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Sirenix.OdinInspector;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    [SerializeField] List<GameObject> _cardPrefabs;

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

    // Instantiates the player deck for the current level
    public List<CardBehaviour> GetPlayerDeck(int deckSize, LevelController levelController)
    {
        if (_cardPrefabs.Count <= 0) return null;

        List<CardBehaviour> deck = new List<CardBehaviour>();

        for(int i = 0; i < deckSize; i++)
        {
            var instantiatedCard = Instantiate(_cardPrefabs[Random.Range(0, _cardPrefabs.Count)]).GetComponent<CardBehaviour>();
            instantiatedCard.SetLevelController(levelController);
            instantiatedCard.SetParent();
            deck.Add(instantiatedCard);
            instantiatedCard.gameObject.SetActive(false);
        }

        ListExtensions.Shuffle(deck);

        return deck;
    }
}
