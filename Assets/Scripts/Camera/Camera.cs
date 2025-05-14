using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 10f, -10f);
    [SerializeField] private float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target no asignado en la cámara");
            return;
        }

        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);

        transform.LookAt(target);

        Vector3 lookDirection = transform.forward;
        lookDirection.y = 0;
        if (lookDirection != Vector3.zero)
        {
            target.forward = lookDirection;
        }
    }
}