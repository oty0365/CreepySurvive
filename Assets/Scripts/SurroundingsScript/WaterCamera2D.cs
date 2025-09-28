using UnityEngine;

[ExecuteAlways]
public class WaterCamera2D : MonoBehaviour
{
    public Camera waterCamera;        // Water 레이어 전용 카메라
    public RenderTexture waterRT;     // 물 전용 RenderTexture
    public Material waterMaterial;    // 물 머티리얼
    public float distortionStrength = 0.05f; // 출렁임 강도

    void LateUpdate()
    {
        if (!waterCamera || !waterMaterial) return;

        // RenderTexture 자동 생성/할당
        if (!waterRT || waterRT.width != Screen.width || waterRT.height != Screen.height)
        {
            if (waterRT) waterRT.Release();
            waterRT = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
            waterCamera.targetTexture = waterRT;
        }

        // Shader에 텍스처와 distortion 값 전달
        waterMaterial.SetTexture("_ReflectionTex", waterRT);
        waterMaterial.SetFloat("_Distortion", distortionStrength*0.01f);

        // Camera는 Orthographic으로 Main Camera 따라가기
        Camera mainCam = Camera.main;
        waterCamera.orthographic = true;
        waterCamera.orthographicSize = mainCam.orthographicSize;
        waterCamera.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y, waterCamera.transform.position.z);
    }
}