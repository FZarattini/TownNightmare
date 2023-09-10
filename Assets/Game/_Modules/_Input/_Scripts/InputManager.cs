using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    PlayerInput playerInputs = null;

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

        playerInputs = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInputs?.Enable();
    }

    private void OnDisable()
    {
        playerInputs?.Disable();    
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInputs.Player.Click.performed += OnClick;
    }

    void OnClick(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.OnDialogue)
        {
            DialogueManager.Instance.NextLine();
        }
        else
        {
            var rayhit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));

            if (!rayhit.collider) return;

            CardBehaviour card; 

            if((card = rayhit.collider.GetComponent<CardBehaviour>()) != null)
            {
                card.ExecuteBehaviour();
            }
        }
    }

}
