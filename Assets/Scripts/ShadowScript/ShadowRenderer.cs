using UnityEngine;

public class ShadowRenderer : MonoBehaviour
{
    public Color shadowColor;
    public GameObject shadowObject;
    public GameObject shadowPrefab;

    public void Render()
    {
        var s=ObjectPoolManager.Instance.Get(shadowPrefab, gameObject.transform.position, Vector3.zero);
        s.GetComponent<IShadow>().SetRenderer(shadowObject);
        s.GetComponent<Shadow>().StartRender(gameObject);
    }
}
