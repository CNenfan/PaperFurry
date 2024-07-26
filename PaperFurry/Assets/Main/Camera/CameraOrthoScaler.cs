using UnityEngine;
using System.Collections;

public class CameraOrthoScaler : MonoBehaviour
{
    [SerializeField]
    private float orthoSize = 1f; // Default orthographic size
    [SerializeField]
    private float maxOrthoSize = 10f; // Maximum allowed orthographic size
    [SerializeField]
    private float minOrthoSize = 0.01f; // Minimum allowed orthographic size
    [Space][Space]
    [SerializeField]
    private bool isDynamicAdjustments = false;

    private IEnumerator _dynamicAdjustments()
    {
        while (isDynamicAdjustments)
        {
            ChangeOrthoSize(orthoSize);
            yield return new WaitForSeconds(0.02f);;
        }
        yield return null;
    }

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        ChangeOrthoSize(orthoSize); // Initialize with default orthographic size
        StartCoroutine(_dynamicAdjustments());
    }

    private Camera _camera;

    /// <summary>
    /// Changes the orthographic size of the camera.
    /// </summary>
    /// <param name="delta">The amount by which to change the orthographic size.</param>
    /// <param name="isRelative">If true, changes the orthographic size relative to its current value; otherwise, sets it to the exact value.</param>
    
    public void ChangeOrthoSize(float delta, bool isRelative = false)
    {
        if (!_camera.orthographic) return;

        if (isRelative)
        {
            // If delta is positive, it will decrease the orthographicSize (zoom in),
            // and if delta is negative, it will increase the orthographicSize (zoom out).
            _camera.orthographicSize += delta; // Note the negative sign before delta
        }
        else
        {
            _camera.orthographicSize = delta;
        }

        // Clamp the orthographic size between min and max
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, minOrthoSize, maxOrthoSize);
    }

    /// <summary>
    /// Sets the limits for the orthographic size.
    /// </summary>
    /// <param name="newMaxOrthoSize">New maximum allowed orthographic size.</param>
    /// <param name="newMinOrthoSize">New minimum allowed orthographic size.</param>
    public void SetOrthoSizeLimits(float newMaxOrthoSize, float newMinOrthoSize)
    {
        maxOrthoSize = newMaxOrthoSize;
        minOrthoSize = newMinOrthoSize;
    }


}