using UnityEngine;

public class CameraCon : MonoBehaviour
{
    public Transform camTarget;
    public float pLerp = 0.2f;
    public float rLerp = 0.2f;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, camTarget.position, pLerp);
        transform.rotation = Quaternion.Lerp(transform.rotation, camTarget.rotation, rLerp);
    }
}
