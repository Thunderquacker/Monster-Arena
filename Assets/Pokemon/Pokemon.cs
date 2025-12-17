using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    public PokemonBase Base { get; private set; }
    public int Level { get; private set; }

    public int HP { get; set; }
    public List<Move> Moves { get; private set; }

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        Base = pBase;
        Level = pLevel;

        Debug.Log($"[Pokemon ctor] Base={Base.name}, Level={Level}, LearnableMoves={Base.LearnableMoves.Count}");

        Moves = new List<Move>();

        foreach (var lm in Base.LearnableMoves)
        {
            if (lm == null)
            {
                Debug.LogWarning("[Pokemon ctor] Null learnable move entry");
                continue;
            }

            Debug.Log($"[Pokemon ctor] lm: {(lm.Base == null ? "NULL" : lm.Base.name)} lvl {lm.Level}");

            if (lm.Base == null)
                continue;

            if (lm.Level > Level)
                continue;

            Moves.Add(new Move(lm.Base));
            if (Moves.Count >= 4)
                break;
        }

        Debug.Log($"[Pokemon ctor] Final runtime moves: {Moves.Count}");

        HP = MaxHP;
    }

    public int MaxHP =>
        Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 10;
    public int Attack =>
        Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5;
    public int Defense =>
        Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5;
    public int SpAttack =>
        Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5;
    public int SpDefense =>
        Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5;
    public int Speed =>
        Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5;

    public bool TakeDamage(Move move, Pokemon attacker)
    {
        float modifiers = Random.Range(0.85f, 1f);
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d);

        HP -= damage;

        if(HP <= 0)
        {
            HP = 0;
            return true;
        }
        return false;
    }

    public Move GetRandomMove()
    {
        // If no moves, return null and let caller handle it
        if (Moves == null || Moves.Count == 0)
            return null;

        int index = Random.Range(0, Moves.Count);   // upper bound is exclusive
        return Moves[index];
    }

}
