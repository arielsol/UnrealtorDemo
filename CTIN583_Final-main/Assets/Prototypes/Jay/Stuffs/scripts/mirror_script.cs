using UnityEngine;

public class MirrorReflection : MonoBehaviour
{
    public Transform playerCamera;
    public Camera mirrorCamera;
    public Transform mirror;

    void LateUpdate()
    {
        Vector3 camDir = playerCamera.forward;
        Vector3 camPos = playerCamera.position;

        Vector3 normal = mirror.forward;
        Vector3 mirrorPos = mirror.position;

        Vector3 reflectedPos = camPos - 2 * Vector3.Dot(camPos - mirrorPos, normal) * normal;
        Vector3 reflectedDir = Vector3.Reflect(camDir, normal);

        mirrorCamera.transform.position = reflectedPos;
        mirrorCamera.transform.rotation = Quaternion.LookRotation(reflectedDir, Vector3.up);
    }
}
