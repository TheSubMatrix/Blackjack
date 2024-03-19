using UnityEngine;

[System.Serializable]
public class Card
{
    /// <summary>
    /// The possible suits a card can be
    /// </summary>
    public enum Suit
    {
        Clubs = 0,
        Spades = 1,
        Diamonds = 2,
        Hearts = 3,
    }
    /// <summary>
    /// The possible faces a card can have
    /// </summary>
    public enum Face
    {
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14,
    }
    /// <summary>
    /// Holds the face of the card
    /// </summary>
    public Face CurrentFace {  get; private set; }
    /// <summary>
    /// Holds the suit of the card
    /// </summary>
    public Suit CurrentSuit {  get; private set; }
    /// <summary>
    /// Holds the sprite to represent the front of the card
    /// </summary>
    public Sprite CurrentFrontSprite { get; private set; }
    /// <summary>
    /// Holds the sprite to represent the back of the card
    /// </summary>
    public Sprite CurrentBackSprite { get; private set; }
    /// <summary>
    /// Whether the front of the card should face toward the player. This means the player will be able to tell what the card is
    /// </summary>
    public bool IsFrontFacingPlayer { get; set; }

    public Card(Face face, Suit suit, Sprite frontsprite, Sprite backSprite, bool isFrontFacingPlayer)
    {
        CurrentFace = face;
        CurrentSuit = suit;
        CurrentFrontSprite = frontsprite;
        CurrentBackSprite = backSprite;
        IsFrontFacingPlayer = isFrontFacingPlayer;
    }
}
