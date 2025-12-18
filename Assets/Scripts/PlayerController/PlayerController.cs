using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public event Action OnEncountered;
    public event Action<Collider2D> OnEnterTrainersView;

    Vector2 moveInput;
    Vector2 lastMoveDir = Vector2.down;
    Character character;

    void Awake()
    {
        character = GetComponent<Character>();
    }

    public void HandleUpdate()
    {
        if (character.isMoving) return;

        moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveInput.y -= 1;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput.x -= 1;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput.x += 1;

        moveInput = moveInput.normalized;

        if (moveInput != Vector2.zero)
        {
            lastMoveDir = moveInput;
            StartCoroutine(character.MoveTo(moveInput, OnMoveOver));
        }
        else
        {
            character.Animator.IsMoving = false;
        }

        character.HandleUpdate();

        if (Keyboard.current.zKey != null && Keyboard.current.zKey.wasPressedThisFrame)
        {
            Interact();
        }
    }

    void Interact()
    {
        var facingDir = new Vector3(lastMoveDir.x, lastMoveDir.y, 0f);
        var interactPos = transform.position + facingDir;

        Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);

        var collider = Physics2D.OverlapCircle(
            interactPos,
            0.3f,
            GameLayers.i.InteractableLayer
        );
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact(transform);
        }
    }

    private void OnMoveOver()
    {
        CheckForEncounters();
        CheckIfInTrainersView();
    }

    void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(
                transform.position,
                0.2f,
                GameLayers.i.GrassLayer) != null)
        {
            if (UnityEngine.Random.Range(1, 101) >= 10)
            {
                character.Animator.IsMoving = false;
                OnEncountered?.Invoke();
            }
        }
    }

    private void CheckIfInTrainersView()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.FovLayer);

        if ( collider != null)
        {
            character.Animator.IsMoving = false;
            OnEnterTrainersView?.Invoke(collider);
        }
    }
}
 