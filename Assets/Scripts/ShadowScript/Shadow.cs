using System.Collections;
using UnityEngine;

public interface IShadow
{
    public void SetRenderer(GameObject go);
}

public class Shadow : MonoBehaviour,IPoolingObject
{
    public GameObject parent;
    private Coroutine _renderFlow;
    public void OnBirth()
    {
        if (_renderFlow != null)
        {
            StopCoroutine(_renderFlow);
            _renderFlow = null;
        }
    }

    public void StartRender(GameObject p)
    {
        parent = p;
        _renderFlow=StartCoroutine(RenderFlow());
    }

    private IEnumerator RenderFlow()
    {
        while (true)
        {
            if (parent == null || !parent.activeSelf)
            {
                ObjectPoolManager.Instance.Return(gameObject);
                yield break; 
            }

            gameObject.transform.position = (Vector2)parent.transform.position + DateManager.Instance.currentShadow;
            gameObject.transform.rotation = parent.transform.rotation;
            gameObject.transform.localScale = parent.transform.localScale;
            yield return null;
        }
    }
    public void OnDeathInit()
    {
        StopCoroutine(_renderFlow);
        _renderFlow = null;
    }
}
