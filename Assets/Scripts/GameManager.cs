using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The amount of turns that the game will run
    /// </summary>
    const int _turnsToPlay = 2;
    /// <summary>
    /// A reference to the index in _cardSprites that holds the image of the back of the card
    /// </summary>
    const int _cardBackIndex = 52;
    /// <summary>
    /// The number of turns that have passed in the game thusfar
    /// </summary>
    int _turnsPassed;
    /// <summary>
    /// This property will automatically start either the Player's or AI's turn whenever the number of turns passed changes
    /// </summary>
    int TurnsPassed 
    { 
        get 
        { 
            return _turnsPassed; 
        } 
        set
        {
            _turnsPassed = value;
            if (_turnsPassed % 2 == 0 && _player != null)
            {
                _player.StartTurn();
            }
            if (_turnsPassed % 2 == 1 && _ai != null)
            {
                _ai.StartTurn();
            }
        }
    }
    /// <summary>
    /// A reference to the player object
    /// </summary>
    Player _player;
    /// <summary>
    /// A reference to the ai object
    /// </summary>
    AI _ai;
    /// <summary>
    /// The transform that will contain the player's card GameObjects
    /// </summary>
    [SerializeField] Transform _playerGroup;
    /// <summary>
    /// The transform that will contain the AI's card GameObjects
    /// </summary>
    [SerializeField] Transform _aiGroup;
    /// <summary>
    /// A reference to the prefab that will be instantiated to show a card on the screen
    /// </summary>
    [SerializeField] GameObject _cardPrefab;
    /// <summary>
    /// A reference to all the possible sprites that can be given to cards
    /// </summary>
    [SerializeField] Sprite[] _cardSprites = new Sprite[53];
    /// <summary>
    /// A list of cards representing the deck in the game
    /// </summary>
    [HideInInspector] public List<Card> _deck;
    [HideInInspector] public delegate void OnGameOver();
    /// <summary>
    /// will be invoked when the game is over
    /// </summary>
    [HideInInspector] public OnGameOver _gameOver;
    [HideInInspector] public delegate void WinnersDetermined(List<string> winners);
    /// <summary>
    /// will be invoked whe the winner is determined
    /// </summary>
    [HideInInspector] public WinnersDetermined _winnersDetermined;
    void Start()
    {
        _gameOver += GameEnded; 

        _player = new Player(this, _playerGroup, _cardPrefab, "Player");
        _ai = new AI(this, _aiGroup, _cardPrefab, "AI");

        CreateDeck();
        Shuffle(ref _deck);

        _player._onActorCardsChanged += UpdateGameObjectsForCards;
        _ai._onActorCardsChanged += UpdateGameObjectsForCards;

        _player.DrawCard(ref _deck);
        _player.DrawCard(ref _deck);

        _ai.DrawCard(ref _deck);
        _ai.DrawCard(ref _deck);

        TurnsPassed = 0;
    }

    /// <summary>
    /// Actions that occur when the turn is finished. This will check if the game is complete and increment the turns passed
    /// </summary>
    public void TurnFinished()
    {
        if(TurnsPassed >= _turnsToPlay - 1)
        {
            _gameOver?.Invoke();
            return;
        }
        TurnsPassed++;
    }
    /// <summary>
    /// Creates the deck of cards based on the enumerations in the Card class and adds them to the _deck variable
    /// </summary>
    void CreateDeck()
    {
        foreach(Card.Suit suit in Enum.GetValues(typeof(Card.Suit)))
        {
            foreach(Card.Face face in Enum.GetValues(typeof(Card.Face)))
            {
                _deck.Add(new Card(face, suit, FindCardSprite(face, suit), _cardSprites[_cardBackIndex], false));
            }
        }
    }
    /// <summary>
    /// Shuffles a list
    /// </summary>
    /// <typeparam name="T">The type of list that will be shuffled</typeparam>
    /// <param name="listToShuffle">A reference to the list that will be shuffled</param>
    void Shuffle<T>(ref List<T> listToShuffle)
    {
        int startingIndex = 0;
        int endingIndex = listToShuffle.Count;
        if(endingIndex > 0)
        {
            for(int i = startingIndex; i < endingIndex; i++) 
            {
                T valueToShuffle = listToShuffle[i];
                int indexToMoveValueTo = UnityEngine.Random.Range(startingIndex, endingIndex);
                listToShuffle[i] = listToShuffle[indexToMoveValueTo];
                listToShuffle[indexToMoveValueTo] = valueToShuffle;
            }
        }
    }
    /// <summary>
    /// Actions to occur when the game has reached its end. This will determine the winner and show the ai's hand to the player
    /// </summary>
    void GameEnded()
    {
        UpdateGameObjectsForCards(_ai.Hand, _ai._cardLocationTransform, true);

        List<string> winnerNames = new List<string>();

        foreach(Actor winner in DetermineWinners(new List<Actor> {_player, _ai })) 
        {
            winnerNames.Add(winner.Name);
        }
        _winnersDetermined.Invoke(winnerNames);
    }
    /// <summary>
    /// Finds the correct sprite for a card given a face and suit
    /// </summary>
    /// <param name="face">The face of the sprite desired</param>
    /// <param name="suit">The suit of the sprite desired</param>
    /// <returns></returns>
    Sprite FindCardSprite(Card.Face face, Card.Suit suit)
    {
        //This is dumb. Find a better way
        return _cardSprites[((int)suit * 13) + (12-((int)face - 2))];
    }
    /// <summary>
    /// Determines the winner based on the given actors
    /// </summary>
    /// <param name="actors">The actors that the method will determine the winner between</param>
    /// <returns>A list of actors that won / tied</returns>
    List<Actor> DetermineWinners(List<Actor> actors)
    {
        List<Actor> winnersToReturn = new List<Actor>();
        if (actors.Count <= 0)
        {
            return winnersToReturn;
        }

        uint highestHandValue = 0;
        foreach (Actor actor in actors)
        {
            if (actor != null)
            {
                if (actor.Hand.HandValue > highestHandValue && actor.Hand.HandValue <= 21)
                {
                    winnersToReturn.Clear();
                    highestHandValue = actor.Hand.HandValue;
                    winnersToReturn.Add(actor);
                }
                else if (actor.Hand.HandValue == highestHandValue)
                {
                    winnersToReturn.Add(actor);
                }
            }
        }
        return winnersToReturn;
    }
    /// <summary>
    /// The method will update the GameObjects for a given hand 
    /// </summary>
    /// <param name="hand">The hand that will determine what the gameobject sprites will be</param>
    /// <param name="parentForCards">A transform that will be the parent of the gameobjects created by the method</param>
    void UpdateGameObjectsForCards(Hand hand, Transform parentForCards)
    {
        foreach (GameObject cardGameObject in hand._cardGameObjects)
        {
            Destroy(cardGameObject);
        }
        hand._cardGameObjects.Clear();

        foreach (Card card in hand.CardsInHand)
        {
            GameObject newCardGO = Instantiate(_cardPrefab, parentForCards);
            hand._cardGameObjects.Add(newCardGO);
            newCardGO.name = card.CurrentFace.ToString() + " Of " + card.CurrentSuit.ToString();
            if (newCardGO.GetComponentInChildren<Image>() != null)
            {
                if (card.IsFrontFacingPlayer)
                {
                    newCardGO.GetComponentInChildren<Image>().sprite = card.CurrentFrontSprite;
                }
                else
                {
                    newCardGO.GetComponentInChildren<Image>().sprite = card.CurrentBackSprite;
                }
            }
        }
    }
    /// <summary>
    /// The method will update the GameObjects for a given hand and change their visibility state
    /// </summary>
    /// <param name="hand">The hand that will determine what the gameobject sprites will be</param>
    /// <param name="parentForCards">A transform that will be the parent of the gameobjects created by the method</param>
    /// <param name="newDesiredCardVisibilityState">Whether the cards will be visible when displayed</param>
    void UpdateGameObjectsForCards(Hand hand, Transform parentForCards, bool newDesiredCardVisibilityState)
    {
        foreach(Card card in hand.CardsInHand)
        {
            card.IsFrontFacingPlayer = newDesiredCardVisibilityState;
        }
        UpdateGameObjectsForCards(hand, parentForCards);
    }
}
