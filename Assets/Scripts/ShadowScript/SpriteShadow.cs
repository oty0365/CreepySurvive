using UnityEngine;

public class SpriteShadow : Shadow,IShadow
{
    public void SetRenderer(GameObject go)
    {
        var sr = gameObject.GetComponent<SpriteRenderer>();
        var gord = go.GetComponent<ShadowRenderer>();
        var gosr = go.GetComponent<SpriteRenderer>();
        sr.sprite=gosr.sprite;
        sr.sortingOrder = gord.order;
        _renderLength = gord.rendForce;
        sr.color = gord.shadowColor;
    }
    
}
