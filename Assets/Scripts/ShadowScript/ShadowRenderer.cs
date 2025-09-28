using UnityEngine;

public class ShadowRenderer : MonoBehaviour
{
    public Color shadowColor;
    public Sprite shadowSprite;
    public GameObject shadowPrefab;
    public int order;
    public Vector2 rendForce;

    public void Render()
    {
        var s=ObjectPoolManager.Instance.Get(shadowPrefab, gameObject.transform.position, Vector3.zero);
        s.GetComponent<Shadow>().Initialize(gameObject);
        s.GetComponent<IShadow>().SetRenderer(gameObject);

    }
}
