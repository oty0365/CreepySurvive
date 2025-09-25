using System.Collections;
using UnityEngine;

public class DateManager : SceneSingletonMonoBehaviour<DateManager>
{
    [SerializeField] private float tickSpeed;
    [SerializeField]private float timeRange;
    private float _dateTime;
    public Vector2 currentShadow;

    private void Start()
    {
        StartCoroutine(ClockFlow());
    }
    
    private IEnumerator ClockFlow()
    {
        while (true)
        {
            // 올라가는 시간
            while (_dateTime < timeRange)
            {
                _dateTime += tickSpeed * Time.deltaTime;
                float progress = Mathf.Clamp01(_dateTime / timeRange);
                currentShadow = new Vector2(1f - 2f * progress, -1f);

                yield return null;
            }
            
            while (_dateTime > 0f)
            {
                _dateTime -= tickSpeed * Time.deltaTime;

                float progress = Mathf.Clamp01(_dateTime / timeRange);
                currentShadow = new Vector2(1f - 2f * progress, -1f);

                yield return null;
            }
        }
    }

}
