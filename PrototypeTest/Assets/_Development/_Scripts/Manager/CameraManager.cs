using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = desiredPosition;
    }

    public void SetTarget(Transform t)
    {
        target = t;
        offset.y = 13;
        offset.z = -22;
    }
}
