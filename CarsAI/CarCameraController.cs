using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraController : MonoBehaviour
{
    public Transform carTransform;
    public float distanceBehind = 5f;
    public float height = 2f;
    public float smoothSpeed = 1f;

    void LateUpdate() {
        Vector3 desiredPosition = carTransform.position - carTransform.forward * distanceBehind + Vector3.up * height;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothPosition;
        transform.LookAt(carTransform.position);
    }
}
