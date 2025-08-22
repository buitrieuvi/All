using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController
{
    public InputSystem_Actions InputActions { get; private set; }

    public InputController()
    {
        InputActions = new InputSystem_Actions();
        InputActions.Enable();
        Debug.Log("InputController initialized and InputActions enabled.");
    }
}
