using System;
using System.Collections;
using UnityEngine;

public class SkewerPhysics : MonoBehaviour,IPoolingObject
{
    public float shootSpeed;
    public Action startRender;
    public Action endRender;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private ShadowRenderer shadowRenderer;
    private Vector2 _destination;
    private Vector2 _dir;
    private Coroutine _shootFlow;
    private float _waitTime;
    private GameObject _player;
    
    
    public void OnBirth()
    {
        _player = PlayerAppearance.Instance.gameObject;
        shadowRenderer.Render();
    }

    public void SetSkewer(Vector2 destination, Vector2 dir, float distance)
    {
        _destination = destination;
        _dir = dir;
        rb2D.linearVelocity = _dir * shootSpeed;
        _waitTime = distance/shootSpeed;
        startRender?.Invoke();
        if (_shootFlow != null)
        {
            StopCoroutine(_shootFlow);
        }
        _shootFlow=StartCoroutine(ShootFlow());
    }

    private void FixedUpdate()
    {
        var dir=_destination-(Vector2)_player.transform.position;
        var rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, rot-90);
    }

    private IEnumerator ShootFlow()
    {
        yield return new WaitForSeconds(_waitTime);
        rb2D.linearVelocity = Vector2.zero;
        while (true)
        {
            rb2D.MovePosition(Vector2.MoveTowards(gameObject.transform.position, _player.transform.position, Time.deltaTime*shootSpeed*1.5f));
            
            if (Vector2.Distance(gameObject.transform.position, _player.transform.position) < 0.1f)
            {
                break;
            }

            yield return new WaitForFixedUpdate(); 
        }

        PlayerSkewer.isSkewing = false;
        endRender?.Invoke();
        ObjectPoolManager.Instance.Return(gameObject);
    }

    public void OnDeathInit()
    {
        StopCoroutine(_shootFlow);
        endRender?.Invoke();
        _shootFlow = null;
    }
    
}
