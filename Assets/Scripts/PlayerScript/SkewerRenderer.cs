using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkewerRenderer : MonoBehaviour
{
    [SerializeField] private GameObject head;         
    [SerializeField] private float renderDistance = 0.2f;
    [SerializeField] private GameObject[] chainObjTypes;
    private List<GameObject> _chainPart =  new();
    private GameObject _player;

    private Coroutine _renderFlow;

    public void StartRendering()
    {
        _player = PlayerAppearance.Instance.gameObject;
        if (_renderFlow != null)
            StopCoroutine(_renderFlow);
        _renderFlow = StartCoroutine(RenderFlow());
    }

    public void StopRendering()
    {
        if (_renderFlow != null)
        {
            StopCoroutine(_renderFlow);
            _renderFlow = null;
        }
        
        DisableAllChains();
    }

    public void DisableAllChains()
    {
        for (int i = _chainPart.Count - 1; i >= 0; i--)
        {
            ObjectPoolManager.Instance.Return(_chainPart[i]);
        }
        _chainPart.Clear();
    }

    private IEnumerator RenderFlow()
    {
        while (true)
        {
            Vector2 dir = ((Vector2)head.transform.position - (Vector2)_player.transform.position).normalized;
            float distance = Vector2.Distance(_player.transform.position, head.transform.position);
            float rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            var counter = 0;
            DisableAllChains();
            for (float i = 0; i < distance; i+=renderDistance)
            {
                var o =ObjectPoolManager.Instance.Get(chainObjTypes[counter % 2],(Vector2)_player.transform.position+dir*i,new Vector3(0,0,rotation-90)); 
                _chainPart.Add(o);
                counter++;
            }
            yield return null; 
        }
    }

    private void OnEnable()
    {
        var skewerPhysics = gameObject.GetComponent<SkewerPhysics>();
        skewerPhysics.startRender+=StartRendering;
        skewerPhysics.endRender+=StopRendering;
    }

    private void OnDisable()
    {
        var skewerPhysics = gameObject.GetComponent<SkewerPhysics>();
        skewerPhysics.startRender-=StartRendering;
        skewerPhysics.endRender-=StopRendering;
    }
}