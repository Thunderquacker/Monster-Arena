using UnityEngine;
using System.Collections.Generic;

public class SpriteAnimation
{
    public List<Sprite> Frames { get; private set; }

    SpriteRenderer spriteRenderer;
    float frameRate;
    int currentFrame;
    float timer;

    public SpriteAnimation(List<Sprite> frames, SpriteRenderer spriteRenderer, float frameRate = 0.16f)
    {
        this.Frames = frames;
        this.spriteRenderer = spriteRenderer;
        this.frameRate = frameRate;
    }

    public void Start()
    {
        if (Frames == null || Frames.Count == 0) return;

        currentFrame = 0;
        timer = 0f;
        spriteRenderer.sprite = Frames[0];
        spriteRenderer.enabled = true;
    }

    public void HandleUpdate()
    {
        if (Frames == null || Frames.Count == 0) return;

        timer += Time.deltaTime;
        if (timer > frameRate)
        {
            currentFrame = (currentFrame + 1) % Frames.Count;
            spriteRenderer.sprite = Frames[currentFrame];
            timer -= frameRate;
        }
    }
}
