using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class Actor
{
    public delegate void OnActorCardsChanged(Hand hand, Transform parentForCards);
    /// <summary>
    /// Holds references to methods that will trigger whenever the given actor's cards change
    /// </summary>
    public OnActorCardsChanged _onActorCardsChanged;
    /// <summary>
    /// The name of the actor
    /// </summary>
    public string Name {  get; private set; }
    /// <summary>
    /// A reference to the GameManager that created the actor 
    /// </summary>
    protected GameManager GM{ get; private set;}
    /// <summary>
    /// The actor's hand
    /// </summary>
    public Hand Hand {get; private set;} = new Hand();
    /// <summary>
    /// The actions that occur when an actor's turn begins
    /// </summary>
    public abstract void StartTurn();
    /// <summary>
    /// A transform that holds the gameobjects of the cards created by the GameManager
    /// </summary>
    public Transform _cardLocationTransform { get; protected set; }
    /// <summary>
    /// A reference to the card prefab to be instantiated inside the player's hand 
    /// </summary>
    public GameObject _cardGOPrefab { get; protected set; }
    /// <summary>
    /// Will draw a card from the given deck
    /// </summary>
    /// <param name="deck">The deck that the actor will draw from</param>
    public virtual void DrawCard(ref List<Card> deck)
    {
        if (deck.Count <= 0 || deck == null) 
        {
            return;
        }
        Hand.AddCardToHand(deck[0]);
        deck.RemoveAt(0);
    }

    public Actor(GameManager gameManager, Transform cardLocationTransform, GameObject cardGOPrefab, string name)
    {
        GM = gameManager;
        _cardLocationTransform = cardLocationTransform;
        _cardGOPrefab = cardGOPrefab;
        Name = name;
    }
    /// <summary>
    /// Actions that occur when the actor's turn is completed. Will try to progress the gm to the next turn
    /// </summary>
    protected void TurnFinished()
    {
        if(GM != null)
        {
            GM.TurnFinished();
        }
    }
}
