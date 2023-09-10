using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CitizensResponse
{
    [SerializeField] CardType _responseType;
    [SerializeField] GameObject _responseObject;
    [SerializeField] bool _activateObject;
    [SerializeField] string _responseMessage;

    public CardType ResponseType => _responseType;
    public GameObject ResponseObject => _responseObject;
    public bool ActivateObject => _activateObject;
    public string ResponseMessage => _responseMessage;
}
