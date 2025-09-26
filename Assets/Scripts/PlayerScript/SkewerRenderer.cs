using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkewerRenderer : MonoBehaviour
{
    [SerializeField] private GameObject head;       
    [SerializeField] private float renderDistance = 0.2f;
    [SerializeField] private GameObject[] chainObjTypes;
    
    private List<GameObject> _chainPart = new();
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
            float distance = Vector2.Distance(_player.transform.position, head.transform.position);
            int requiredSegments = Mathf.CeilToInt(distance / renderDistance);
            
            while (_chainPart.Count < requiredSegments)
            {
                int typeIndex = _chainPart.Count % 2;
                GameObject newChain = ObjectPoolManager.Instance.Get(chainObjTypes[typeIndex],_player.gameObject.transform.position, Vector3.zero);
                _chainPart.Add(newChain);
            }
            
            while (_chainPart.Count > requiredSegments)
            {
                int lastIndex = _chainPart.Count - 1;
                ObjectPoolManager.Instance.Return(_chainPart[lastIndex]);
                _chainPart.RemoveAt(lastIndex);
            }
            
            Vector2 dir = ((Vector2)head.transform.position - (Vector2)_player.transform.position).normalized;
            float rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            
            for (int i = 0; i < _chainPart.Count; i++)
            {
                Vector2 position = (Vector2)_player.transform.position + dir * (i * renderDistance);
                _chainPart[i].transform.position = position;
                _chainPart[i].transform.rotation = Quaternion.Euler(0, 0, rotation);
            }
            
            yield return new WaitForSeconds(0.016f);
        }
    }

    private void OnEnable()
    {
        
        var skewerPhysics = gameObject.GetComponent<SkewerPhysics>();
        skewerPhysics.startRender += StartRendering;
        skewerPhysics.endRender += StopRendering;
    }

    private void OnDisable()
    {
        var skewerPhysics = gameObject.GetComponent<SkewerPhysics>();
        skewerPhysics.startRender -= StartRendering;
        skewerPhysics.endRender -= StopRendering;
    }
}