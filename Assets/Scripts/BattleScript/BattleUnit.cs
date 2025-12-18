using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] PokemonBase basePokemon;
    [SerializeField] int level = 5;
    [SerializeField] bool isPlayerUnit;

    public Pokemon Pokemon { get; private set; }

    public void Setup()
    {
        Pokemon = new Pokemon(basePokemon, level);

        var image = GetComponent<Image>();
        image.sprite = isPlayerUnit
            ? Pokemon.Base.BackSprite
            : Pokemon.Base.FrontSprite;
    }
}
