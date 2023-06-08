using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartCamera : MonoBehaviour
{
    public Transform Target;

    public bool LockOffset = false;
    public Vector2 Offset;
    public float smoothSpeed = 0.1f;
    public float maximumSpeed = 20f;
    public float CorrectionMagnitude = 0.1f;
    public bool hardFollow = false;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        if (LockOffset)
        {
            Offset = transform.position - Target.position;
        }
    }

    private void Update()
    {
        if (Target != null)
        {
            if (hardFollow)
                HardFollow();
            else
                SmoothFollow();
        }

        ClampCameraPositionAccordingToRoomBoundaries();
    }

    public void HardFollow()
    {
        Vector2 targetPos = (Vector2)Target.position + (LockOffset ? Offset : Vector2.zero);
        transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
    }

    public void SmoothFollow()
    {
        Vector2 targetPos = (Vector2)Target.position + (LockOffset ? Offset : Vector2.zero);
        Vector2 smoothFollow = Vector3.SmoothDamp(transform.position, Target.position,
            ref velocity, smoothSpeed, maximumSpeed);
        if (Mathf.Abs((targetPos - smoothFollow).magnitude) > CorrectionMagnitude)
        {
            transform.position = new Vector3(smoothFollow.x, smoothFollow.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
            velocity = Vector3.zero;
            hardFollow = true;
        }

    }

    private void ClampCameraPositionAccordingToRoomBoundaries()
    {
        if (!GameManager.Hr.CurrentRoom.HasBoundaries)
            return;

        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        Vector4 boundaries = new Vector4();

        boundaries.x = GameManager.Hr.CurrentRoom.Boundaries.x + width / 2F;
        boundaries.y = GameManager.Hr.CurrentRoom.Boundaries.y - height / 2F;
        boundaries.z = GameManager.Hr.CurrentRoom.Boundaries.z - width / 2F;
        boundaries.w = GameManager.Hr.CurrentRoom.Boundaries.w + height / 2F;

        float clampedX = Mathf.Clamp(transform.position.x, boundaries.x, boundaries.z);
        float clampedY = Mathf.Clamp(transform.position.y, boundaries.w, boundaries.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    public void ChangeTarget(Transform target)
    {
        hardFollow = false;
        Target = target;
    }
}
