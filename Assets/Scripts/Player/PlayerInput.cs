using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action OnMouseClick, OnMouseRightClick, OnFly, OnInventory;
    public event Action<int> OnKeyButtonClick;
    public event Action<float> OnScrollInput;

    public bool RunningPressed { get; private set; }
    public Vector3 MovementInput { get; private set; }
    public Vector2 MousePosition { get; private set; }
    public bool IsJumping { get; private set; }

    private KeyCode[] keyCodes =
    {
        KeyCode.Alpha0,
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
    };

    private void Update()
    {
        GetMouseClick();
        GetMouseRightClick();
        GetMousePosition();
        GetMovementInput();
        GetJumpInput();
        GetRunInput();
        GetFlyInput();
        GetNumberPress();
        GetScrollInput();
        GetInventoryInput();
    }

    private void GetInventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInventory?.Invoke();
        }
    }

    private void GetFlyInput()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            OnFly?.Invoke();
        }

    }

    private void GetRunInput()
    {
        RunningPressed = Input.GetKey(KeyCode.LeftShift);
    }

    private void GetJumpInput()
    {
        IsJumping = Input.GetButton("Jump");
    }

    private void GetMovementInput()
    {
        MovementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    private void GetMousePosition()
    {
        MousePosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    private void GetMouseClick()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnMouseClick?.Invoke();
        }
    }

    private void GetMouseRightClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            OnMouseRightClick?.Invoke();
        }
    }

    private void GetNumberPress()
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                OnKeyButtonClick?.Invoke(i-1);
            }
        }
    }

    private void GetScrollInput()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
            OnScrollInput?.Invoke(-1);
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)
            OnScrollInput?.Invoke(1);
    }


}