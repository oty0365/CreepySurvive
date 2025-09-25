using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkewer : MonoBehaviour
{
    public GameObject currentSkewer;
    public float range;
    public static bool isSkewing;

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed && !isSkewing)
        {
            isSkewing = true;
            var mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            var dir = mousePos - (Vector2)transform.position;
            var rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            var destination = (Vector2)transform.position + dir.normalized * range;
            var o = ObjectPoolManager.Instance.Get(currentSkewer, transform.position, new Vector3(0, 0, rotation - 90));
            
            o.GetComponent<SkewerPhysics>().SetSkewer(destination, dir.normalized,range);
        }
    }
}