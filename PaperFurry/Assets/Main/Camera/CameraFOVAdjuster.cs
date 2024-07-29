using UnityEngine;

//用于透视摄像头，弃用
public class CameraFOVAdjuster : MonoBehaviour
{
    [SerializeField]
    private float fov = 60f; // Default field of view
    [SerializeField]
    private float maxFov = 120f; // Maximum allowed field of view
    [SerializeField]
    private float minFov = 10f; // Minimum allowed field of view

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        ChangeFOV(fov); // Initialize with default FOV
    }

    /// <summary>
    /// Changes the field of view of the camera.
    /// </summary>
    /// <param name="delta">The amount by which to change the FOV.</param>
    /// <param name="isRelative">If true, changes the FOV relative to its current value; otherwise, sets it to the exact value.</param>
    public void ChangeFOV(float delta, bool isRelative = true)
    {
        if (_camera.orthographic) return; // Do nothing if the camera is orthographic

        if (isRelative)
        {
            _camera.fieldOfView += delta;
        }
        else
        {
            _camera.fieldOfView = delta;
        }

        // Clamp the FOV between min and max
        _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView, minFov, maxFov);
    }

    /// <summary>
    /// Sets the limits for the field of view.
    /// </summary>
    /// <param name="newMaxFov">New maximum allowed field of view.</param>
    /// <param name="newMinFov">New minimum allowed field of view.</param>
    public void SetFOVLimits(float newMaxFov, float newMinFov)
    {
        maxFov = newMaxFov;
        minFov = newMinFov;
    }

    public void Update()
    {
        if(Input.GetKey(KeyCode.N))
        {
            ChangeFOV(-0.5f, true);
        }
        else if(Input.GetKey(KeyCode.M))
        {
            ChangeFOV(0.5f, true);
        }
    }
}