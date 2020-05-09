using Rewired;
using UnityEngine;

public class MenuInputController : MonoBehaviour
{
    private const InputActionEventType BUTTON_TYPE = InputActionEventType.ButtonJustReleased;
    private const InputActionEventType AXIS_TYPE = InputActionEventType.AxisActive;

    [SerializeField] private CanvasGroup startingCanvasGroup;
    [SerializeField] private CanvasGroup settingsCanvasGroup;
    
    private Player _player;

    private void Start()
    {
        _player = ReInput.players.GetPlayer(0);

        if (_player == null) return;
        
        _player.AddInputEventDelegate(OnCloseMenu, UpdateLoopType.Update, BUTTON_TYPE, "Exit");
        _player.AddInputEventDelegate(OnOpenMenu, UpdateLoopType.Update, BUTTON_TYPE, "Menu");
        _player.AddInputEventDelegate(OnBack, UpdateLoopType.Update, BUTTON_TYPE, "Back");
    }
    
    private void OnCloseMenu(InputActionEventData data)
    {
        if (_player == null ||
            !GameManager.Instance.HasStarted) return;
        
        Debug.Log("exit menu");
        
        Time.timeScale = 1;

        _player.controllers.maps.SetMapsEnabled(false, "Menu");
        _player.controllers.maps.SetMapsEnabled(true, "InGame");

        if (startingCanvasGroup) startingCanvasGroup.alpha = 0;
    }
    
    private void OnOpenMenu(InputActionEventData data)
    {
        if (_player == null ||
            !GameManager.Instance.HasStarted) return;
        
        Debug.Log("open menu");
        
        Time.timeScale = 0;

        _player.controllers.maps.SetMapsEnabled(true, "Menu");
        _player.controllers.maps.SetMapsEnabled(false, "InGame");

        if (startingCanvasGroup) startingCanvasGroup.alpha = 1;
    }
    
    private void OnBack(InputActionEventData data)
    {
        Debug.Log("back");


    }

    public void Play()
    {
        if (startingCanvasGroup) startingCanvasGroup.alpha = 0;
        
        GameManager.Instance.StartGame();
        
        _player.controllers.maps.SetMapsEnabled(false, "Menu");
        _player.controllers.maps.SetMapsEnabled(true, "InGame");
        
        //Tell game manager to start the game
        //If we make more levels then level select
    }

    public void ShowSettings()
    {
//        if (!settingsCanvasGroup) return;
//
//        settingsCanvasGroup.alpha = 1;
//        settingsCanvasGroup.interactable = true;
    }

    public void ShowCredits()
    {
        
    }
}
