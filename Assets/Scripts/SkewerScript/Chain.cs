using UnityEngine;

public class Chain : MonoBehaviour,IPoolingObject
{
    [SerializeField] private ShadowRenderer shadowRenderer;
    public void OnBirth()
    {
        shadowRenderer.Render();
    }

    public void OnDeathInit()
    {
    }
}
