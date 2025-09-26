using UnityEngine;

public class TileMapSetter : MonoBehaviour
{
    [SerializeField] private ShadowRenderer shadowRenderer;
    void Start()
    {
        shadowRenderer.Render();
    }


}
