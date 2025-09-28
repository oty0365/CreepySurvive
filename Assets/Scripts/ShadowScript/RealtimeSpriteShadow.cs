using UnityEngine;

public class RealtimeSpriteShadow : Shadow,IShadow
{
    [SerializeField] private SpriteRenderer sr;
    private SpriteRenderer _parentSr;
    public void SetRenderer(GameObject go)
    {
        var gord = go.GetComponent<ShadowRenderer>();
        _parentSr = go.GetComponent<SpriteRenderer>();
        sr.sprite=_parentSr.sprite;
        sr.sortingOrder = gord.order;
        _renderLength = gord.rendForce;
        sr.color = gord.shadowColor;
        sr.flipY = true;
    }

    public override void RenderTick()
    {
        if (_parentSr != null)
        {
            sr.sprite = _parentSr.sprite;
            sr.flipX = _parentSr.flipX;
        }
    }
}
