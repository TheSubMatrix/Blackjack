using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hand
{
    /// <summary>
    /// The maximum size of a given hand
    /// </summary>
    public const uint maxHandSize = 5;
    /// <summary>
    /// A list of gameobjects representing the actor's cards
    /// </summary>
    public List<GameObject> _cardGameObjects { get; set; } = new List<GameObject>();
    public List<Card> CardsInHand { get; private set; } = new List<Card>();

    /// <summary>
    /// Adds a given card to the hand and recalculates the value of the hand
    /// </summary>
    /// <param name="cardToAdd">The card that will be added to the hand</param>
    public void AddCardToHand(Card cardToAdd)
    {
        if(CardsInHand.Count + 1 <= maxHandSize)
        {
            CardsInHand.Add(cardToAdd);
            CalculateHandValue();
        }
    }
    public uint HandValue { get; private set; } = 0;
    /// <summary>
    /// Calculates the value of the hand
    /// </summary>
    void CalculateHandValue()
    {
        uint calculatedValue = 0;
        uint aceCount = 0;

        foreach (Card card in CardsInHand)
        {
            if((uint)card.CurrentFace >= 2 && (uint)card.CurrentFace <= 10)
            {
                calculatedValue += (uint)card.CurrentFace;
            }
            else if (card.CurrentFace == Card.Face.Jack || card.CurrentFace == Card.Face.Queen || card.CurrentFace == Card.Face.King)
            {
                calculatedValue += 10;
            }
            else
            {
                calculatedValue += 11;
                aceCount++;
            }
        }

        if (calculatedValue > 21 && aceCount > 0)
        {
            for(int i = 0; i < aceCount && calculatedValue > 21; i++)
            {
                calculatedValue -= 10;
            }
        }
        HandValue = calculatedValue;
    }
}