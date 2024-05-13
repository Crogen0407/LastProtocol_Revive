using UnityEngine;
using UnityEngine.UI;

public abstract class CharacterAnimation
{
    protected RectTransform rect;
    public bool isAnimating { get; protected set; } = false;

    public abstract void Animation();

    public virtual void Init(RectTransform rect)
    {
        this.rect = rect;
    }
}
