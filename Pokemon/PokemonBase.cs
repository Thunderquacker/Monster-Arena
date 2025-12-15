// PokemonBase.cs - FIXED
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new Pokemon")]
public class PokemonBase : ScriptableObject
{
    // Use an unambiguous name for the field
    [SerializeField] string pokemonName;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    // BASE STATS (Private fields)
    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;

    [SerializeField] List<LearnableMove> learnableMoves;

    // PUBLIC PROPERTIES (Accessors for fields)
    public string Name // PascalCase for public access
    {
        get { return pokemonName; }
    }

    public string Description
    {
        get { return description; }
    }

    // EXPOSING STATS
    public int MaxHP // New Public Property
    {
        get { return maxHP; }
    }

    public int Attack // New Public Property
    {
        get { return attack; }
    }

    public int Defense // New Public Property
    {
        get { return defense; }
    }

    public int SpAttack // New Public Property
    {
        get { return spAttack; }
    }

    public int SpDefense // New Public Property
    {
        get { return spDefense; }
    }

    public int Speed // New Public Property
    {
        get { return speed; }
    }

    public List<LearnableMove> LearnableMoves
    {
        get { return learnableMoves; }
    }

}

// (The enum remains correct)
[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;
    public MoveBase Base
    {
        get { return moveBase; }
    }
    public int Level
    {
        get { return level; }
    }
}
public enum PokemonType { 
    Steel,
    Fire,
    Water,
    Bug,
    Grass,
    Dragon,
    Rock,
    Ground,
    Flying,
    Electric,
    Ghost,
    Dark,
    Psychic,
    Ice,
    Normal,
    Fighting,
    Poison
}