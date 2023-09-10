using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VillageController : LevelController
{
    [Title("Village Specifics")]

    [SerializeField] List<CitizensResponse> _citizensResponse;

    protected override void LevelChanges()
    {
        base.LevelChanges();
    }

    protected override void CitizensResponseBehaviour(CardBehaviour card)
    {
        base.CitizensResponseBehaviour(card);

        for(int i = 0; i < _citizensResponse.Count; i++)
        {
            if(card.CardType == _citizensResponse[i].ResponseType)
            {
                RunResponse(_citizensResponse[i]);
                _citizensResponse.RemoveAt(i);
                break;
            }
        }
    }
}
