using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float mouseSensitivity = 100.0f;
    public float distanceFromPlayer = 5.0f;
    public float heightOffset = 2.0f;
    private float pitch = 0.0f;
    private float yaw = 0.0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void LateUpdate()
    {
        if (player != null)
        {
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Update yaw and pitch
            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -35.0f, 60.0f);

            // Calculate camera position and rotation
            Vector3 direction = new Vector3(0, 0, -distanceFromPlayer);
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            transform.position = player.transform.position + rotation * direction + Vector3.up * heightOffset;
            transform.LookAt(player.transform.position + Vector3.up * heightOffset);
        }
    }
}
