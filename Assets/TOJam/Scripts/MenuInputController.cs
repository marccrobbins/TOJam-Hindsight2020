using Rewired;
using UnityEngine;

public class MenuInputController : MonoBehaviour
{
    private const InputActionEventType BUTTON_TYPE = InputActionEventType.ButtonJustReleased;
    private const InputActionEventType AXIS_TYPE = InputActionEventType.AxisActive;
    
    private Player _player;

    private void Start()
    {
        _player = ReInput.players.GetPlayer(0);

        if (_player == null) return;
        
        _player.AddInputEventDelegate(OnMenuMoveHorizontal, UpdateLoopType.Update, AXIS_TYPE, "MoveHorizontal");
        _player.AddInputEventDelegate(OnMenuMoveVertical, UpdateLoopType.Update, AXIS_TYPE, "MoveVertical");
        _player.AddInputEventDelegate(OnExit, UpdateLoopType.Update, BUTTON_TYPE, "Exit");
        _player.AddInputEventDelegate(OnSelect, UpdateLoopType.Update, BUTTON_TYPE, "Select");
        _player.AddInputEventDelegate(OnBack, UpdateLoopType.Update, BUTTON_TYPE, "Back");
    }
    
    private void OnMenuMoveHorizontal(InputActionEventData data)
    {
        Debug.Log("move horizontal");
    }
    
    private void OnMenuMoveVertical(InputActionEventData data)
    {
        Debug.Log("move vertical");
    }

    private void OnExit(InputActionEventData data)
    {
        Debug.Log("exit menu");
        if (_player == null) return;

        _player.controllers.maps.SetMapsEnabled(false, "Menu");
        _player.controllers.maps.SetMapsEnabled(true, "InGame");
    }
    
    private void OnSelect(InputActionEventData data)
    {
        Debug.Log("select");
    }
    
    private void OnBack(InputActionEventData data)
    {
        Debug.Log("back");
    }
}
