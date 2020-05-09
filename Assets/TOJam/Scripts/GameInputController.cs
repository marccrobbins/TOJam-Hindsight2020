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
     
        _player.AddInputEventDelegate(OnLeftJoystickHorizontal, UpdateLoopType.Update, AXIS_TYPE, "LeftJoystickHorizontal");
        _player.AddInputEventDelegate(OnLeftJoystickVertical, UpdateLoopType.Update, AXIS_TYPE, "LeftJoystickVertical");
        _player.AddInputEventDelegate(OnRightJoystickHorizontal, UpdateLoopType.Update, AXIS_TYPE, "RightJoystickHorizontal");
        _player.AddInputEventDelegate(OnRightJoystickVertical, UpdateLoopType.Update, AXIS_TYPE, "RightJoystickVertical");
        _player.AddInputEventDelegate(OnLeftTrigger, UpdateLoopType.Update, AXIS_TYPE, "LeftTriggerAction");
        _player.AddInputEventDelegate(OnRightTrigger, UpdateLoopType.Update, AXIS_TYPE, "RightTriggerAction");
    }

    private void OnLeftJoystickHorizontal(InputActionEventData data)
    {
        Debug.LogFormat("Left arm move horizontal [{0}]", data.GetAxis());
    }
    
    private void OnLeftJoystickVertical(InputActionEventData data)
    {
        Debug.LogFormat("Left arm move vertical [{0}]", data.GetAxis());
    }
    
    private void OnRightJoystickHorizontal(InputActionEventData data)
    {
        Debug.LogFormat("Right arm move horizontal [{0}]", data.GetAxis());
    }
    
    private void OnRightJoystickVertical(InputActionEventData data)
    {
        Debug.LogFormat("Right arm move vertical [{0}]", data.GetAxis());
    }
    
    private void OnLeftTrigger(InputActionEventData data)
    {
        Debug.LogFormat("Left trigger [{0}]", data.GetAxis());
    }
    
    private void OnRightTrigger(InputActionEventData data)
    {
        Debug.LogFormat("Right trigger [{0}]", data.GetAxis());
    }
}
