using System;
using UnityEngine;

public class ObjectShadowRenderer : MonoBehaviour
{
    [SerializeField] ShadowRenderer shadowRenderer;

    private void Start()
    {
        shadowRenderer.Render();
    }
}
