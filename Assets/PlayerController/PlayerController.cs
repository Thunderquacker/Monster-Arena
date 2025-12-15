using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask SolidObjectsLayer;
    public LayerMask grassLayer;

    private Vector2 moveInput;
    private bool isMoving = false;
    private Animator animator;
    private Vector2 lastMoveDir = Vector2.down;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isMoving) return; // Don't accept new input while moving

        moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveInput.y -= 1;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput.x -= 1;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput.x += 1;

        moveInput = moveInput.normalized;

        if (moveInput != Vector2.zero)
        {
            lastMoveDir = moveInput;

            animator.SetFloat("moveX", moveInput.x);
            animator.SetFloat("moveY", moveInput.y);

            animator.SetFloat("lastMoveX", lastMoveDir.x);
            animator.SetFloat("lastMoveY", lastMoveDir.y);

            Vector3 targetPos = transform.position + new Vector3(moveInput.x, moveInput.y, 0);

            if (IsWalkable(targetPos))
            {
                StartCoroutine(MoveTo(targetPos));
            }
        }

        // Set animator parameters for speed and movement state
        animator.SetFloat("Speed", moveInput.sqrMagnitude);
        animator.SetBool("isMoving", isMoving);
    }

    IEnumerator MoveTo(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;

        CheckForEncounters();
    }

    bool IsWalkable(Vector3 targetPos)
    {
        return Physics2D.OverlapCircle(targetPos, 0.2f, SolidObjectsLayer) == null;
    }

    private void CheckForEncounters()
    {
        if(Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            if(Random.Range(1, 101) >= 10)
            {
                Debug.Log("Encounter a wild Pokemon!");
            }
        }
    }
}
