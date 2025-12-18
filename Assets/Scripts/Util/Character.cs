using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed = 5f;
    CharacterAnimation animator;
    public bool isMoving { get; private set; }

    void Awake()
    {
        animator = GetComponent<CharacterAnimation>();
    }

    public void LookTowards(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            animator.MoveX = diff.x > 0 ? 1f : -1f;
            animator.MoveY = 0f;
        }
        else
        {
            animator.MoveX = 0f;
            animator.MoveY = diff.y > 0 ? 1f : -1f;
        }

        animator.IsMoving = false;
    }

    public IEnumerator MoveTo(Vector2 moveVector, Action OnMoveOver = null)
    {
        if (isMoving)
            yield break;

        animator.MoveX = Mathf.Clamp(moveVector.x, -1f, 1f);
        animator.MoveY = Mathf.Clamp(moveVector.y, -1f, 1f);

        Vector3 targetPos = transform.position + new Vector3(moveVector.x, moveVector.y, 0);

        if (!IsPathClear(targetPos))
            yield break;

        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;

        OnMoveOver?.Invoke();
    }

    public void HandleUpdate()
    {
        animator.IsMoving = isMoving;
    }

    bool IsPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;
        var dir = diff.normalized;

        if (Physics2D.BoxCast(
                transform.position + dir,
                new Vector2(0.2f, 0.2f),
                0f,
                dir,
                diff.magnitude - 1f,
                GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer))
        {
            return false;
        }

        return true;
    }

    bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(
                targetPos,
                0.2f,
                GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer) != null)
        {
            return false;
        }
        return true;
    }

    public CharacterAnimation Animator => animator;
}
