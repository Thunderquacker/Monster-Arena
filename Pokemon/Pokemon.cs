// Pokemon.cs - CLEANED UP
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Pokemon
{
    // Use a clearer name for the base data
    PokemonBase Base;
    int level;

    public int HP { get; set; }
    public List<Move> Moves {  get; set; }

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        Base = pBase;
        level = pLevel;
        HP = MaxHP;

        Moves = new List<Move>();

        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= level)
            {
                Moves.Add(new Move(move.Base));
            }

            if (Moves.Count >= 4)
                break;
        }
        
        // REMOVED: The illegal statement 'yes.name;'
    }

    // Renamed 'yes' to 'Base' for clarity, and used PascalCase for stats
    public int MaxHP
    {
        get { return Mathf.FloorToInt((Base.MaxHP * level) / 100f) + 10; }
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Attack * level) / 100f) + 5; }
    }

    public int Defense
    {
        get { return Mathf.FloorToInt((Base.Defense * level) / 100f) + 5; }
    }

    public int SpAttack
    {
        get { return Mathf.FloorToInt((Base.SpAttack * level) / 100f) + 5; }
    }

    public int SpDefense
    {
        get { return Mathf.FloorToInt((Base.SpDefense * level) / 100f) + 5; }
    }

    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Speed * level) / 100f) + 5; }
    }
}