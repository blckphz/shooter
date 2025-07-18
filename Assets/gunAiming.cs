using UnityEngine;

public class gunAiming : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public Transform cameraTransform;
    public Transform gunPivot;
    public GameObject gameOverScreen;

    private float xRotation = 0f;

    void Start()
    {
        LockCursor(true);
    }

    void Update()
    {
        // Stop aiming if dead or Game Over screen active
        if (PlayerHealth.Health <= 0 || (gameOverScreen != null && gameOverScreen.activeSelf))
        {
            LockCursor(false);
            return;
        }
        else
        {
            LockCursor(true);
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        playerBody.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        gunPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void LockCursor(bool shouldLock)
    {
        Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !shouldLock;
    }
}
