using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform cam;

    float yRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Vector2 mouseInput = InputManager.GetMouseInput() * Time.deltaTime;

        yRotation -= mouseInput.y;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        cam.localRotation = Quaternion.Euler(yRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseInput.x);
    }
}
