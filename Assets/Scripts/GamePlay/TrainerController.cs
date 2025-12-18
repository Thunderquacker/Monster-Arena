using System;
using System.Collections;
using UnityEngine;

public class TrainerController : MonoBehaviour
{
    [SerializeField] Dialog dialog;
    [SerializeField] GameObject exclamation;
    [SerializeField] GameObject fov;

    Character character;

    void Awake()
    {
        character = GetComponent<Character>();
    }

    void Start()
    {
        SetFovRotation(character.Animator.DefaultDirection);
    }

    public IEnumerator TriggerTrainerBattle(PlayerController player)
    {
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);

        var diff = player.transform.position - transform.position;
        var moveVector = diff - diff.normalized;
        moveVector = new Vector2(Mathf.Round(moveVector.x), Mathf.Round(moveVector.y));

        if (character != null)
            yield return StartCoroutine(character.MoveTo(moveVector));

        character.LookTowards(player.transform.position);

        yield return DialogManager.Instance.ShowDialog(dialog, () =>
        {
            Debug.Log("Starting Trainer Battle.");
        });
    }

    public void SetFovRotation(FacingDirection dir)
    {
        float angle = 0f;

        if (dir == FacingDirection.Right)
            angle = 90f;
        else if (dir == FacingDirection.Up)       
            angle = 180f;
        else if (dir == FacingDirection.Left)     
            angle = 270f;

        fov.transform.eulerAngles = new Vector3(0f, 0f, angle);
    }
}
