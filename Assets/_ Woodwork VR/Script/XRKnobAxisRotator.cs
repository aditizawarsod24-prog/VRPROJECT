using UnityEngine;

public class XRKnobAxisRotator : MonoBehaviour
{
    public enum LocalAxis
    {
        X,
        Y,
        Z
    }

    [Header("Object to Rotate")]
    public Transform target;

    [Header("Local Rotation Axis")]
    public LocalAxis rotationAxis = LocalAxis.Y;

    [Header("Rotation Limits")]
    public bool useLimits = true;
    public float minAngle = 0f;
    public float maxAngle = 360f;

    [Header("Options")]
    public bool invert = false;

    private Quaternion initialRotation;

    private void Awake()
    {
        if (target != null)
            initialRotation = target.localRotation;
    }

    // Connect this method to XR Knob -> On Value Changed
    public void SetRotation(float value)
    {
        if (target == null)
            return;

        // XR Knob normally provides a value between 0 and 1
        if (invert)
            value = 1f - value;

        float angle;

        if (useLimits)
        {
            // Clamp between Min and Max
            value = Mathf.Clamp01(value);
            angle = Mathf.Lerp(minAngle, maxAngle, value);
        }
        else
        {
            // Unclamped rotation
            angle = value * 360f;
        }

        Vector3 axis;

        switch (rotationAxis)
        {
            case LocalAxis.X:
                axis = Vector3.right;
                break;

            case LocalAxis.Y:
                axis = Vector3.up;
                break;

            case LocalAxis.Z:
                axis = Vector3.forward;
                break;

            default:
                axis = Vector3.up;
                break;
        }

        target.localRotation =
            initialRotation * Quaternion.AngleAxis(angle, axis);
    }
}
