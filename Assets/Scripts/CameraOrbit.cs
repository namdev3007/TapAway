using UnityEngine;
using DG.Tweening;

public class CameraOrbit : MonoBehaviour
{
    public float distance = 10f;
    public float xSpeed = 120f;
    public float ySpeed = 80f;

    [Header("Auto Zoom Settings")]
    public float minDistance = 15f;
    public float maxDistance = 40f;
    public float zoomPadding = 1.2f;

    [Header("Scroll Zoom Settings")]
    public float scrollZoomSpeed = 10f;
    public float zoomTweenDuration = 0.3f;

    private float x = 0.0f;
    private float y = 0.0f;
    private Tween zoomTween;

    private Vector3 targetCenter;


    private LevelCameraSettings currentLevelSettings;


    private void Awake()
    {
        currentLevelSettings = Object.FindFirstObjectByType<LevelCameraSettings>(); 
    }
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        AutoZoomToTarget();
    }

    void LateUpdate()
    {
        if (currentLevelSettings == null || currentLevelSettings.cameraTarget == null) return;

        Transform target = currentLevelSettings.cameraTarget;

        // Xử lý xoay
        if (currentLevelSettings.allowRotation && Input.GetMouseButton(1))
        {
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
        }

        // Xử lý zoom bằng chuột
        if (currentLevelSettings.allowZoom)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                float targetDistance = Mathf.Clamp(distance - scroll * scrollZoomSpeed, minDistance, maxDistance);

                if (zoomTween != null && zoomTween.IsActive()) zoomTween.Kill();

                zoomTween = DOTween.To(() => distance, d => distance = d, targetDistance, zoomTweenDuration)
                                   .SetEase(Ease.OutQuad);
            }
        }

        // Cập nhật vị trí camera
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 negDistance = new Vector3(0, 0, -distance);
        Vector3 position = rotation * negDistance + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }

    public void SetLevel(LevelCameraSettings levelSettings)
    {
        currentLevelSettings = levelSettings;

        if (currentLevelSettings == null || currentLevelSettings.cameraTarget == null)
        {
            Debug.LogWarning("LevelCameraSettings hoặc cameraTarget chưa được gán.");
            return;
        }

        AutoZoomToTarget();
    }

    public void AutoZoomToTarget()
    {
        if (currentLevelSettings == null || currentLevelSettings.cameraTarget == null) return;

        Transform target = currentLevelSettings.cameraTarget;

        Collider targetCollider = target.GetComponent<Collider>();
        if (targetCollider != null)
        {
            Bounds bounds = targetCollider.bounds;
            targetCenter = bounds.center; // ✅ Tính toán trung tâm hình học thực tế

            float size = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);

            float fov = Camera.main.fieldOfView;
            float aspect = Camera.main.aspect;

            float distanceByHeight = (size * zoomPadding) / (2f * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad));
            float distanceByWidth = (size * zoomPadding) / (2f * Mathf.Tan((fov * aspect) * 0.5f * Mathf.Deg2Rad));

            float requiredDistance = Mathf.Max(distanceByHeight, distanceByWidth);
            distance = Mathf.Clamp(requiredDistance, minDistance, maxDistance);
        }
        else
        {
            targetCenter = target.position; // fallback
            distance = minDistance;
        }
    }

}
