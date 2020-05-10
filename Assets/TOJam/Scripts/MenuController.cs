using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    private const InputActionEventType BUTTON_TYPE = InputActionEventType.ButtonJustReleased;
    private const InputActionEventType AXIS_TYPE = InputActionEventType.AxisActive;

    public EventSystem eventSystem;
    public MusicController musicController;
    public CanvasGroup startingCanvasGroup;
    public CanvasGroup settingsCanvasGroup;
    public CanvasGroup creditsCanvasGroup;
    public CanvasGroup winStateCanvasGroup;
    public CanvasGroup loseStateCanvasGroup;
    
    private Player player;
    private CanvasGroup currentCanvasGroup;
    private CanvasGroup lastCanvasGroup;

    private void Start()
    {
        currentCanvasGroup = startingCanvasGroup;
        
        player = ReInput.players.GetPlayer(0);
        player?.AddInputEventDelegate(OnOpenMenu, UpdateLoopType.Update, BUTTON_TYPE, "Menu");
        player?.AddInputEventDelegate(OnCloseMenu, UpdateLoopType.Update, BUTTON_TYPE, "Exit");
        player?.AddInputEventDelegate(OnBack, UpdateLoopType.Update, BUTTON_TYPE, "Back");
    }
    
    private void OnOpenMenu(InputActionEventData data)
    {
        ShowSettings();

        Time.timeScale = 0;
	    
        player.controllers.maps.SetMapsEnabled(true, "Menu");
        player.controllers.maps.SetMapsEnabled(false, "InGame");
	    
        if (musicController) musicController.SetMuffle(true);
    }
    
    private void OnCloseMenu(InputActionEventData data)
    {
        if (player == null ||
            !GameManager.Instance.HasStarted) return;

        Time.timeScale = 1;
        
        player.controllers.maps.SetMapsEnabled(false, "Menu");
        player.controllers.maps.SetMapsEnabled(true, "InGame");

        TransitionMenu(null);
        if(musicController) musicController.SetMuffle(false);
    }
    
    private void OnBack(InputActionEventData data)
    {
        if (GameManager.Instance.HasStarted)
        {
            OnCloseMenu(data);
        }

        if (currentCanvasGroup == startingCanvasGroup) return;
        TransitionMenu(lastCanvasGroup);
    }

    #region MainMenu

    public void Play()
    {
        TransitionMenu(null);
        
        GameManager.Instance.StartGame();
        
        player.controllers.maps.SetMapsEnabled(false, "Menu");
        player.controllers.maps.SetMapsEnabled(true, "InGame");
        
        //Tell game manager to start the game
        //If we make more levels then level select
    }

    public void ShowSettings()
    {
        if (!settingsCanvasGroup) return;
        TransitionMenu(settingsCanvasGroup);
    }

    public void ShowCredits()
    {
        if (!creditsCanvasGroup) return;
        TransitionMenu(creditsCanvasGroup);
    }
    
    #endregion MainMenu

    #region Settings

    //Might not get to these    

    #endregion Settings

    #region EndStates

    public void ShowWinState()
    {
        if (!winStateCanvasGroup) return;
        TransitionMenu(winStateCanvasGroup);
        
        player.controllers.maps.SetMapsEnabled(true, "Menu");
        player.controllers.maps.SetMapsEnabled(false, "InGame");
    }

    public void ShowLoseState()
    {
        if (!loseStateCanvasGroup) return;
        TransitionMenu(loseStateCanvasGroup);
        
        player.controllers.maps.SetMapsEnabled(true, "Menu");
        player.controllers.maps.SetMapsEnabled(false, "InGame");
    }
    
    #endregion EndStates

    public void BackToMainMenu()
    {
        GameManager.Instance.ResetGame();
        TransitionMenu(startingCanvasGroup);
    }

    private void TransitionMenu(CanvasGroup nextMenu)
    {
        if (!nextMenu)
        {
            if (currentCanvasGroup)
            {
                currentCanvasGroup.alpha = 0;
                currentCanvasGroup.interactable = false;
            }

            currentCanvasGroup = null;
            lastCanvasGroup = null;
            return;
        }
        
        lastCanvasGroup = currentCanvasGroup;
        currentCanvasGroup = nextMenu;
        
        var currentMenuTransform = currentCanvasGroup.transform;
        if (eventSystem) eventSystem.SetSelectedGameObject(currentMenuTransform.GetChild(0).gameObject);
        
        currentCanvasGroup.alpha = 1;
        currentCanvasGroup.interactable = true;

        if (!lastCanvasGroup) return;
        lastCanvasGroup.alpha = 0;
        lastCanvasGroup.interactable = false;
    }
}
