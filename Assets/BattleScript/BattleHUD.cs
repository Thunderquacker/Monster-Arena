using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    Pokemon poke;

    public void SetData(Pokemon pokemon)
    {
        poke = pokemon;

        Debug.Log($"HUD SetData: {pokemon.Base.Name} Lvl {pokemon.Level}");
        nameText.text = pokemon.Base.Name;
        levelText.text = "Lvl " + pokemon.Level;

        // Use MaxHP consistently
        hpBar.SetHP((float)pokemon.HP / pokemon.MaxHP);
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)poke.HP / poke.MaxHP);
    }
}
