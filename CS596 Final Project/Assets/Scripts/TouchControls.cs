using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class TouchControls : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction tapHold;
    private InputAction tapSingle;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        tapHold = playerInput.actions["TapHold"];
        tapSingle = playerInput.actions["TapSingle"];
    }

    private void OnEnable()
    {
        tapHold.Enable();
        tapSingle.Enable();

        tapHold.performed += OnTapHold;
    }

    private void OnDisable()
    {
        tapHold.performed -= OnTapHold;

        tapHold.Disable();
        tapSingle.Disable();
    }

    private void Update()
    {
        if (tapSingle != null && tapSingle.WasPressedThisFrame())
        {
            Debug.Log("TapSingle Pressed");
        }
    }

    private void OnTapHold(InputAction.CallbackContext context)
    {
        Debug.Log("TapHold Performed: " + context.ReadValue<float>());
    }
}
