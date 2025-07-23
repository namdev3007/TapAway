using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;       
    public float distance = 10f;     
    public float xSpeed = 120f;       
    public float ySpeed = 80f;        

    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        if (target == null)
        {
            Debug.LogWarning("Bạn chưa gán target cho CameraOrbit!");
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (Input.GetMouseButton(1)) // Giữ chuột phải để xoay
        {
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

            // ❌ Không giới hạn góc y nữa (xoay dọc 360 độ luôn)
        }

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }
}
