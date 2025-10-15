using UnityEngine;

public class CameraLook : MonoBehaviour
{


    [SerializeField] private Transform playerBody;
    [SerializeField] private float smoothTime = 0.05f; // plus petit = plus réactif
    [SerializeField] private float mouseSensitivity = 120f;

    private float xRotation = 0f;
    private float xRotationVelocity = 0f;
    private float yRotationVelocity = 0f;
    private float currentYRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentYRotation = playerBody.eulerAngles.y;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotation verticale (caméra)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        float smoothX = Mathf.SmoothDampAngle(transform.localEulerAngles.x, xRotation, ref xRotationVelocity, smoothTime);
        transform.localRotation = Quaternion.Euler(smoothX, 0f, 0f);

        // Rotation horizontale (player)
        currentYRotation += mouseX;
        float smoothY = Mathf.SmoothDampAngle(playerBody.eulerAngles.y, currentYRotation, ref yRotationVelocity, smoothTime);
        playerBody.rotation = Quaternion.Euler(0f, smoothY, 0f);
    }
}
