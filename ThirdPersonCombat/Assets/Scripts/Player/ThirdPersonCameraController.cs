using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] float zoomSpeed;
    [SerializeField] float zoomLerpSpeed;
    [SerializeField] float minDist;
    [SerializeField] float maxDist;

    [SerializeField] private PlayerControls controls;
    [SerializeField] CinemachineCamera cam;
    [SerializeField] CinemachineOrbitalFollow orbital;

    private Vector2 scrollDelta;
    private float targetZoom;
    private float currentZoom;

    void Start()
    {
        controls = new PlayerControls();
        controls.Enable();
        controls.Camera.MouseZoom.performed += HandleMouseScroll;

        Cursor.lockState = CursorLockMode.Locked;

        targetZoom = currentZoom = orbital.Radius;
    }

    private void HandleMouseScroll(InputAction.CallbackContext context)
    {
        scrollDelta = context.ReadValue<Vector2>();
    }

    void Update()
    {
        if (scrollDelta.y != 0)
        {
            if(orbital != null)
            {
                targetZoom = Mathf.Clamp(orbital.Radius - scrollDelta.y * zoomSpeed, minDist, maxDist);
                scrollDelta = Vector2.zero;
            }
        }

        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomLerpSpeed);
        orbital.Radius = currentZoom;
    }
}
