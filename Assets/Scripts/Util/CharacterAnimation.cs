using UnityEngine;
using System.Collections.Generic;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkLeftSprites;
    [SerializeField] List<Sprite> walkRightSprites;
    [SerializeField] FacingDirection defaultDirection = FacingDirection.Down;

    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }

    SpriteAnimation walkDownAnim;
    SpriteAnimation walkUpAnim;
    SpriteAnimation walkRightAnim;
    SpriteAnimation walkLeftAnim;
    SpriteAnimation currentAnim;
    SpriteRenderer spriteRenderer;

    bool wasPreviouslyMoving;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        walkDownAnim = new SpriteAnimation(walkDownSprites, spriteRenderer);
        walkUpAnim = new SpriteAnimation(walkUpSprites, spriteRenderer);
        walkLeftAnim = new SpriteAnimation(walkLeftSprites, spriteRenderer);
        walkRightAnim = new SpriteAnimation(walkRightSprites, spriteRenderer);
        SetFacingDirection(defaultDirection);

        currentAnim = walkDownAnim;

        walkDownAnim.Start();
        walkUpAnim.Start();
        walkLeftAnim.Start();
        walkRightAnim.Start();
    }

    private void Update()
    {

        var prevAnim = currentAnim;

        if (MoveX == 1f)
            currentAnim = walkRightAnim;
        else if (MoveX == -1f)
            currentAnim = walkLeftAnim;
        else if (MoveY == 1f)
            currentAnim = walkUpAnim;
        else if (MoveY == -1f)
            currentAnim = walkDownAnim;

        if (currentAnim != prevAnim || IsMoving != wasPreviouslyMoving)
            currentAnim.Start();

        if (IsMoving)
            currentAnim.HandleUpdate();
        else
            spriteRenderer.sprite = currentAnim.Frames[0];

        wasPreviouslyMoving = IsMoving;
    }

    public void SetFacingDirection(FacingDirection dir)
    {
        if (dir == FacingDirection.Right)
            MoveX = 1;
        else if (dir == FacingDirection.Left)
            MoveX = -1;
        else if (dir == FacingDirection.Up)
            MoveY = 1;
        else if (dir == FacingDirection.Down)
            MoveY = -1;
    }

    public FacingDirection DefaultDirection
    {
        get => defaultDirection;
    }

    public List<Sprite> Frames
    {
        get { return currentAnim != null ? currentAnim.Frames : walkDownSprites; }
    }
}

public enum FacingDirection { Up, Down, Left, Right }