using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public delegate void DoneButtonPressed();
    /// <summary>
    /// holds methods to be triggered when the player's done button is pressed. This will end the players turn
    /// </summary>
    public DoneButtonPressed _doneButtonPressed;
    public delegate void HitButtonPressed();
    /// <summary>
    /// Holds methods to be triggered when the player's hit button is pressed. This will make the player attempt to draw a card
    /// </summary>
    public HitButtonPressed _hitButtonPressed;
    /// <summary>
    /// The canvas group that will hold gameplay elements
    /// </summary>
    [SerializeField] CanvasGroup _gameplayCanvasGroup;
    /// <summary>
    /// The canvas group that will hold elements to be shown when the game is completed
    /// </summary>
    [SerializeField] CanvasGroup _gameFinishedCanvasGroup;
    /// <summary>
    /// The TMP text to show the result of the game in the UI
    /// </summary>
    [SerializeField] TMP_Text _resultText;
    private void Start()
    {
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null)
        {
            gm._gameOver += OnGameOver;
            gm._winnersDetermined += OnWinnersDetermined;
        }
    }
    /// <summary>
    /// Will attempt to make any actors hooked into the delegate finish their turn
    /// </summary>
    public void OnDoneButtonPressed()
    {
        _doneButtonPressed?.Invoke();
    }
    /// <summary>
    /// Will attempt to make any actors hooked into the delegate draw a card
    /// </summary>
    public void OnHitButtonPressed()
    {
        _hitButtonPressed?.Invoke();
    }
    /// <summary>
    /// Will transition the player back to the main menu
    /// </summary>
    public void OnMainMenuButtonPressed()
    {
        SceneManager.LoadScene("Main Menu");
    }
    /// <summary>
    /// Will reload the game scene, reseting the game to its start
    /// </summary>
    public void OnReplayButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// UI actions that will occur when the game is over. Disables the gameplay canvas group and enables the game finished canvas group
    /// </summary>
    void OnGameOver()
    {
        DisableCanvasGroup(_gameplayCanvasGroup);
        EnableCanvasGroup(_gameFinishedCanvasGroup);
    }
    /// <summary>
    /// Hides and disables all interaction with a given canvas group
    /// </summary>
    /// <param name="groupToDisable">The canvas group the will be disabled</param>
    void DisableCanvasGroup(CanvasGroup groupToDisable)
    {
        if(groupToDisable != null)
        {
            groupToDisable.alpha = 0;
            groupToDisable.blocksRaycasts = false;
            groupToDisable.interactable = false;
        }
    }
    /// <summary>
    /// shows and enables all interaction with a given canvas group
    /// </summary>
    /// <param name="groupToEnable">The canvas group the will be enabled</param>
    void EnableCanvasGroup(CanvasGroup groupToEnable)
    {
        if(groupToEnable != null)
        {
            groupToEnable.alpha = 1;
            groupToEnable.blocksRaycasts = true;
            groupToEnable.interactable = true;
        }
    }
    /// <summary>
    /// Actions that will occur when a winner is determined. This will create and update the text object showing the results of the game
    /// </summary>
    /// <param name="winners">The strings representing the names of the game winners</param>
    void OnWinnersDetermined(List<string> winners)
    {
        if(_resultText != null)
        {
            _resultText.text = CreateWinString(winners);
        }
    }
    /// <summary>
    /// Creates the string showed in the results text
    /// </summary>
    /// <param name="winners">The names of the game winners</param>
    /// <returns></returns>
    string CreateWinString(List<string> winners)
    {
        if(winners.Count <= 0)
        {
            return "Nobody Won";
        }
        else if(winners.Count == 1) 
        {
            return winners[0] + " Won";
        }
        else
        {
            string stringToReturn = string.Empty;
            foreach(string winner in winners)
            {
                stringToReturn += winner;
                if(winner != winners.Last())
                {
                    stringToReturn += " and ";
                }
            }
            stringToReturn += " Tied";
            return stringToReturn;
        }
    }
}
