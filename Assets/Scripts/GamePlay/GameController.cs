using System;
using UnityEngine;

public enum GameState { FreeRoam, Battle, Dialog, Cutscence }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state;

    void Start()
    {
        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;

        playerController.OnEnterTrainersView += (Collider2D trainerCollider) =>
        {
            var trainer = trainerCollider.GetComponentInParent<TrainerController>();
            if (trainer != null)
            {
                state = GameState.Cutscence;
                StartCoroutine(trainer.TriggerTrainerBattle(playerController));
            }
        };

        battleSystem.gameObject.SetActive(false);

        DialogManager.Instance.OnShowDialog += () => { state = GameState.Dialog; };
        DialogManager.Instance.OnCloseDialog += () =>
        {
            if (state == GameState.Dialog)
                state = GameState.FreeRoam;
        };
    }

    void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        battleSystem.InitBattle();
    }

    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    void Update()
    {
        if (state == GameState.FreeRoam)
            playerController.HandleUpdate();
        else if (state == GameState.Battle)
            battleSystem.HandleUpdate();
        else if (state == GameState.Dialog)
            DialogManager.Instance.HandleUpdate();
    }
}
