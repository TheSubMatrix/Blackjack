using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Player : Actor
{
    bool isCurrentlyActive = false;
    /// <summary>
    /// Actions that will occur when the Player's turn begins. This will enable the player to interact with their hand
    /// </summary>
    public override void StartTurn()
    {
        isCurrentlyActive = true;
    }
    public Player(GameManager gameManager, Transform cardLocationTransform, GameObject cardPrefab, string name) : base(gameManager, cardLocationTransform, cardPrefab, name)
    {
        UIManager uiManager = Object.FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager._doneButtonPressed += OnDoneButtonPressed;
            uiManager._hitButtonPressed += OnHitButtonPressed;
        }
    }
    /// <summary>
    /// The actions that will occur when the hit button is pressed in the UI. This will attempt to draw a card if it is the player's turn
    /// </summary>
    public void OnHitButtonPressed()
    {
        if(isCurrentlyActive && GM != null)
        {
            DrawCard(ref GM._deck);
        }
    }
    /// <summary>
    /// Will draw a card from the given deck and attempt to update the card GameObjects
    /// </summary>
    /// <param name="deck">The deck that the actor will draw from</param>
    public override void DrawCard(ref List<Card> deck)
    {
        if (deck.Count > 0 || deck != null)
        {
            deck[0].IsFrontFacingPlayer = true;
        }
        base.DrawCard(ref deck);
        _onActorCardsChanged?.Invoke(Hand, _cardLocationTransform);
    }
    /// <summary>
    /// The actions that will occur when the Done button is pressed in the UI. Progresses the GameManager to the next turn and makes player input no longer register
    /// </summary>
    public void OnDoneButtonPressed()
    {
        if(isCurrentlyActive && GM != null)
        {
            isCurrentlyActive = false;
            TurnFinished();
        }
    }
}