using UnityEngine;

public class SpriteShadow : Shadow,IShadow
{
    public void SetRenderer(GameObject go)
    {
        var psr = parent.GetComponent<SpriteRenderer>();
        var gosr = go.GetComponent<SpriteRenderer>();
        psr.sprite = gosr.sprite;
        psr.color = gosr.color;
    }
}
