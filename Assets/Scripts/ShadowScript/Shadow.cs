using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShadow
{
    public void SetRenderer(GameObject go);
}

public class Shadow : MonoBehaviour,IPoolingObject
{
    public GameObject parent;
    private Coroutine _renderFlow;
    protected Vector2 _renderLength;

    public void OnBirth()
    {
        if (_renderFlow != null)
        {
            StopCoroutine(_renderFlow);
            _renderFlow = null;
        }
    }

    public void Initialize(GameObject p)
    {
        parent = p;
        _renderFlow=StartCoroutine(RenderFlow());
    }

    public virtual void RenderTick(){}

    private IEnumerator RenderFlow()
    {
        while (true)
        {
            if (parent == null || !parent.activeSelf)
            {
                ObjectPoolManager.Instance.Return(gameObject);
                yield break; 
            }

            gameObject.transform.position = (Vector2)parent.transform.position + DateManager.Instance.currentShadow*_renderLength;
            gameObject.transform.rotation = parent.transform.rotation;
            gameObject.transform.localScale = parent.transform.localScale;
            RenderTick();
            yield return new WaitForSeconds(0.016f);
        }
    }
    public void OnDeathInit()
    {
        StopCoroutine(_renderFlow);
        _renderFlow = null;
    }
}