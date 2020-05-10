using Rewired;
using UnityEngine;

public class MenuInputController : MonoBehaviour
{
    private const InputActionEventType BUTTON_TYPE = InputActionEventType.ButtonJustReleased;
    private const InputActionEventType AXIS_TYPE = InputActionEventType.AxisActive;

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
        player = ReInput.players.GetPlayer(0);

        if (player == null) return;
        
        player.AddInputEventDelegate(OnCloseMenu, UpdateLoopType.Update, BUTTON_TYPE, "Exit");
        player.AddInputEventDelegate(OnOpenMenu, UpdateLoopType.Update, BUTTON_TYPE, "Menu");
        player.AddInputEventDelegate(OnBack, UpdateLoopType.Update, BUTTON_TYPE, "Back");
    }
    
    private void OnCloseMenu(InputActionEventData data)
    {
        if (player == null ||
            !GameManager.Instance.HasStarted) return;
        
        player.controllers.maps.SetMapsEnabled(false, "Menu");
        player.controllers.maps.SetMapsEnabled(true, "InGame");

        TransitionMenu(null);
    }
    
    private void OnOpenMenu(InputActionEventData data)
    {
        if (player == null ||
            !GameManager.Instance.HasStarted) return;
        
        player.controllers.maps.SetMapsEnabled(true, "Menu");
        player.controllers.maps.SetMapsEnabled(false, "InGame");

        TransitionMenu(settingsCanvasGroup);
    }
    
    private void OnBack(InputActionEventData data)
    {
        if (GameManager.Instance.HasStarted)
        {
            TransitionMenu(null);
        }

        if (lastCanvasGroup == startingCanvasGroup) return;
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
    }

    public void ShowLoseState()
    {
        if (!loseStateCanvasGroup) return;
        TransitionMenu(loseStateCanvasGroup);
    }
    
    #endregion EndStates

    public void BackToMainMenu()
    {
        
    }

    public void RestartGame()
    {
        
    }

    private void TransitionMenu(CanvasGroup nextMenu)
    {
        if (!nextMenu)
        {
            currentCanvasGroup.alpha = 0;
            currentCanvasGroup.interactable = false;
            return;
        }
        
        lastCanvasGroup = currentCanvasGroup;
        currentCanvasGroup = nextMenu;
        
        currentCanvasGroup.alpha = 1;
        currentCanvasGroup.interactable = true;
        lastCanvasGroup.alpha = 0;
        lastCanvasGroup.interactable = false;
    }
}
