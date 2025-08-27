using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputController
{
    public InputSystem_Actions InputActions { get; private set; }

    public InputController()
    {
        InputActions = new InputSystem_Actions();
        InputActions.Enable();
    }
}
