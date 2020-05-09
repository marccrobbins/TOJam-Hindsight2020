using System;
using Rewired;
using UnityEngine;

public class GameInputController : MonoBehaviour
{
    private const InputActionEventType BUTTON_TYPE = InputActionEventType.ButtonJustReleased;
    private const InputActionEventType AXIS_TYPE = InputActionEventType.AxisActive;
    
    private Player _player;

    private void Start()
    {
        _player = ReInput.players.GetPlayer(0);

        if (_player == null) return;
     
    }

    private void OnLeftJoystickHorizontal(InputActionEventData data)
    {
        Debug.Log("Left arm move horizontal");
    }
    
    private void OnLeftJoystickVertical(InputActionEventData data)
    {
        Debug.Log("Left arm move vertical");
    }
    
    private void OnRightJoystickHorizontal(InputActionEventData data)
    {
        Debug.Log("Right arm move horizontal");
    }
    
    private void OnRightJoystickVertical(InputActionEventData data)
    {
        Debug.Log("Rgiht arm move vertical");
    }
    
    private void OnLeftTrigger(InputActionEventData data)
    {
        Debug.Log("Left trigger");
    }
    
    private void OnRightTrigger(InputActionEventData data)
    {
        Debug.Log("Right trigger");
    }
    
    private void OnMenu(InputActionEventData data)
    {
        Debug.Log("Open menu");
        if (_player == null) return;

        _player.controllers.maps.SetMapsEnabled(true, "Menu");
        _player.controllers.maps.SetMapsEnabled(false, "InGame");
    }
}
