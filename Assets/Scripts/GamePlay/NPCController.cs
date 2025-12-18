using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPattern = 2f;

    NPCState state = NPCState.Idle;
    float idleTimer = 0f;
    int currentPattern = 0;

    Character character;

    void Awake()
    {
        character = GetComponent<Character>();
        if (character == null)
            character = GetComponentInChildren<Character>();
        if (character == null)
            Debug.LogError("Character component not found on NPCController GameObject", this);
    }

    public void Interact(Transform initiator)
    {
        if (state != NPCState.Idle)
            return;

        state = NPCState.Dialog;

        character.LookTowards(initiator.position);

        StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () =>
        {
            idleTimer = 0f;
            state = NPCState.Idle;
        }));

        if (character != null)
            StartCoroutine(character.MoveTo(new Vector2(-2, 0)));
    }

    void Update()
    {
        if (state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > timeBetweenPattern)
            {
                idleTimer = 0f;
                if (movementPattern != null && movementPattern.Count > 0)
                    StartCoroutine(Walk());
            }
        }

        if (character != null)
            character.HandleUpdate();
    }

    IEnumerator Walk()
    {
        state = NPCState.Walking;

        var oldPos = transform.position;

        yield return StartCoroutine(character.MoveTo(movementPattern[currentPattern]));

        if (transform.position != oldPos)
            currentPattern = (currentPattern + 1) % movementPattern.Count;

        state = NPCState.Idle;
    }

    public enum NPCState { Idle, Walking, Dialog }
}
