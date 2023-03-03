using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public PlayerObject playerObject;
    public bool isCursorLocked = false;

    public bool enableRotation = true;
    public bool enableMovement = true;
    public bool enableCursor = true;

    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;

    public Vector3 eulerRotation;

    private void Start()
    {
        if (playerObject == null) return;
        eulerRotation = playerObject.transform.localRotation.eulerAngles;
        RandomizeColor();
    }

    public void RandomizeColor()
    {
        Color customColor = new Color(Random.value, Random.value, Random.value, 1.0f);

        playerObject.hexColor = ColorUtility.ToHtmlStringRGB(customColor);
    }

    void Update()
    {
        if (playerObject == null) return;

        HandleMouse();
        HandleRotation();
        HandleMovement();
    }

    void LateUpdate()
    {
        if (playerObject == null) return;

        playerCamera.transform.position = playerObject.transform.position;
        playerCamera.transform.rotation = playerObject.transform.rotation;
    }

    public void HandleMouse()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            isCursorLocked = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            isCursorLocked = false;
        }
    }

    public void HandleRotation()
    {
        float rotationX = 0;
        float rotationY = 0;

        if (Input.GetMouseButton(1) && isCursorLocked)
        {
            rotationX = Input.GetAxis("Mouse X") * rotationSpeed * Time.fixedDeltaTime;
            rotationY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.fixedDeltaTime;
        }

        eulerRotation.x -= rotationY;
        eulerRotation.y += rotationX * (1 + Mathf.Abs(eulerRotation.x / 90));

        eulerRotation.x = Mathf.Clamp(eulerRotation.x, -90, 90);

        playerObject.transform.localRotation = Quaternion.Euler(eulerRotation);
    }

    public void HandleMovement()
    {
        float translation = 0;
        float straffe = 0;
        float upDown = 0;

        if (Input.GetMouseButton(1) && isCursorLocked)
        {
            translation = Input.GetAxis("Vertical") * speed;
            straffe = Input.GetAxis("Horizontal") * speed;

            if (Input.GetKey(KeyCode.Q)) upDown = -speed * Time.fixedDeltaTime;
            else if (Input.GetKey(KeyCode.E)) upDown = speed * Time.fixedDeltaTime;
        }

        translation *= Time.fixedDeltaTime;
        straffe *= Time.fixedDeltaTime;

        playerObject.transform.Translate(straffe, upDown, translation);
    }
}
