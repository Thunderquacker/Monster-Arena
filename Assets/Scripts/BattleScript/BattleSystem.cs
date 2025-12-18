using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHUD playerHud;
    [SerializeField] BattleHUD enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

    public event Action<bool> OnBattleOver;

    BattleState state;
    int currentAction;
    int currentMove;

    
    void Start() { }

    
    public void InitBattle()
    {
        state = BattleState.Start;
        currentAction = 0;
        currentMove = 0;

        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        playerUnit.Setup();
        enemyUnit.Setup();
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);

        Debug.Log("Player Base learnable moves: " +
                  playerUnit.Pokemon.Base.LearnableMoves.Count);
        Debug.Log("Player runtime moves: " +
                  playerUnit.Pokemon.Moves.Count);

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);

        yield return dialogBox.TypeDialog(
            $"A wild {enemyUnit.Pokemon.Base.Name} appeared!");
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        currentAction = 0;

        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogText(true);
    }

    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        currentMove = 0;

        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);

        var moves = playerUnit.Pokemon.Moves;
        Debug.Log("PlayerMove: moves count = " + moves.Count);
        Debug.Log("MoveSelector activeSelf = " +
                  dialogBox.MoveSelectorObject.activeSelf);

        for (int i = 0; i < moves.Count; i++)
            Debug.Log($"Move {i}: {moves[i].Base.Name}");

        dialogBox.SetMoveNames(moves);
        if (moves.Count > 0)
            dialogBox.UpdateMoveSelection(currentMove, moves[currentMove]);
    }

    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;

        var move = playerUnit.Pokemon.Moves[currentMove];

        yield return dialogBox.TypeDialog(
            $"{playerUnit.Pokemon.Base.Name} used {move.Base.Name}");
        yield return new WaitForSeconds(1f);

        bool isFainted = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
        yield return enemyHud.UpdateHP();

        if (isFainted)
        {
            yield return dialogBox.TypeDialog(
                $"{enemyUnit.Pokemon.Base.Name} Fainted");

            yield return new WaitForSeconds(2f);
            OnBattleOver?.Invoke(true);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        var move = enemyUnit.Pokemon.GetRandomMove();
        if (move == null)
        {
            Debug.LogWarning("Enemy has no moves, skipping turn.");
            PlayerAction();
            yield break;
        }

        yield return dialogBox.TypeDialog(
            $"{enemyUnit.Pokemon.Base.Name} used {move.Base.Name}");
        yield return new WaitForSeconds(1f);

        bool isFainted = playerUnit.Pokemon.TakeDamage(move, enemyUnit.Pokemon);
        yield return playerHud.UpdateHP();

        if (isFainted)
        {
            yield return dialogBox.TypeDialog(
                $"{playerUnit.Pokemon.Base.Name} Fainted");

            yield return new WaitForSeconds(2f);
            OnBattleOver?.Invoke(false);
        }
        else
        {
            PlayerAction();
        }
    }

    public void HandleUpdate()
    {
        if (state == BattleState.PlayerAction)
            HandleActionSelection();
        else if (state == BattleState.PlayerMove)
            HandleMoveSelection();
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < 1)
                currentAction++;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 0)
                currentAction--;
        }

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
                PlayerMove();   
            else if (currentAction == 1)
            {
                
            }
        }
    }

    void HandleMoveSelection()
    {
        var moves = playerUnit.Pokemon.Moves;
        if (moves == null || moves.Count == 0)
            return;

        int cols = 2;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove % cols == 0 && currentMove + 1 < moves.Count)
                currentMove += 1;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove % cols == 1)
                currentMove -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove + cols < moves.Count)
                currentMove += cols;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove - cols >= 0)
                currentMove -= cols;
        }

        currentMove = Mathf.Clamp(currentMove, 0, moves.Count - 1);

        if (dialogBox != null)
            dialogBox.UpdateMoveSelection(currentMove, moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
            StartCoroutine(PerformPlayerMove());

        if (Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            state = BattleState.PlayerAction;
        }
    }
}
