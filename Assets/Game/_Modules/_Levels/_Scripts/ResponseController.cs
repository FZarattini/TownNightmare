using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResponseController : MonoBehaviour
{
    [SerializeField] LevelController _levelController;
    [SerializeField] List<CitizensResponse> _citizensResponse;

    // Specific scene response behaviour for the village
    public void CitizensResponseBehaviour(CardBehaviour card)
    {
        for (int i = 0; i < _citizensResponse.Count; i++)
        {
            if (card.CardType == _citizensResponse[i].ResponseType)
            {
                RunResponse(_citizensResponse[i]);
                _citizensResponse.RemoveAt(i);
                break;
            }
        }
    }

    // Executes the citizens' response if they are successfull
    void RunResponse(CitizensResponse response)
    {
        GameObject responseObject = response.ResponseObject;
        bool activateObject = response.ActivateObject;
        string responseMessage = response.ResponseMessage;

        if (responseObject != null)
            responseObject.SetActive(activateObject);

        switch (response.ResponseType)
        {
            case CardType.Land:
                _levelController.LandDefences++;
                break;
            case CardType.Air:
                _levelController.AirDefences++;
                break;
            case CardType.Magic:
                _levelController.MagicDefences++;
                break;

            default:
                break;
        }

        DialogueManager.Instance.AddDialogueToQueue(responseMessage);
    }
}