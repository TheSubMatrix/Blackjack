using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AI : Actor
{
    /// <summary>
    /// The number the AI will stop drawing cards at
    /// </summary>
    const uint _stopNumber = 17;
    /// <summary>
    /// Tracks whether the current card being drawn is the first card drawn by the AI
    /// </summary>
    bool _isFirstCardDrawn = true;
    /// <summary>
    /// Actions that will occur at the start of the AI's turn. This includes AI logic
    /// </summary>
    public override void StartTurn()
    {
        if(GM != null)
        {
            while (Hand.HandValue < _stopNumber && Hand.CardsInHand.Count <= Hand.maxHandSize)
            {
                DrawCard(ref GM._deck);
            }
        }
        TurnFinished();
    }
    /// <summary>
    /// Will draw a card from the given deck and attempt to update the card GameObjects and will make the card visible to the player if it is the first card drawn
    /// </summary>
    /// <param name="deck">The deck that the actor will draw from</param>
    public override void DrawCard(ref List<Card> deck)
    {
        base.DrawCard(ref deck);
        if (_isFirstCardDrawn)
        {
            Hand.CardsInHand[0].IsFrontFacingPlayer = true;
            _isFirstCardDrawn=false;
        }
        _onActorCardsChanged?.Invoke(Hand, _cardLocationTransform);
    }
    public AI(GameManager gameManager, Transform cardLocationTransform, GameObject cardPrefab, string name) : base(gameManager, cardLocationTransform, cardPrefab, name)
    {

    }
}
