using UnityEngine;

public class FrameManager : SceneSingletonMonoBehaviour<FrameManager>
{
    [SerializeField] private int frameRate;
    void Start()
    {
        Application.targetFrameRate = frameRate;
    }
}
