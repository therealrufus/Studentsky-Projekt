using UnityEngine;

public class NetworkPlayerCamera : MonoBehaviour
{
    public float mouseSensitivity;
    public Transform cam;

    float yRotation = 0f;

    private void Start()
    {
        if (NetworkPlayerMovement.LocalPlayerInstance == gameObject)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else 
        {
            cam.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (NetworkPlayerMovement.LocalPlayerInstance == gameObject)
        {
            Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * mouseSensitivity * Time.deltaTime;

            yRotation -= mouseInput.y;
            yRotation = Mathf.Clamp(yRotation, -90f, 90f);

            cam.localRotation = Quaternion.Euler(yRotation, 0, 0);
            transform.Rotate(Vector3.up * mouseInput.x);
        }
    }
}
