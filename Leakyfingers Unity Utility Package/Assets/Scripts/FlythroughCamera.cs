//////////////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        03.11.19
// Date last edited:    23.05.19
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace LeakyfingersUtility
{
    // A script which allows the camera to be rotated using the mouse as well as being moved relative to it's orientation using the W, A, S, D, Q, and E keys.
    [RequireComponent(typeof(Camera))]
    public class FlythroughCamera : MonoBehaviour
    {
        public float MouseSensitivity = 100.0f;
        public float VerticalClampAngle = 80.0f;
        public float MoveSpeed = 5;

        private Vector2 MouseLookRotation; // The rotation of the camera around the X and Y axis according to the mouse position.
        private bool isPaused;

        private void Awake()
        {
            Vector3 rotation = transform.localRotation.eulerAngles;
            MouseLookRotation.x = rotation.x;
            MouseLookRotation.y = rotation.y;

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            UpdateMouseLockedState();
            if (!isPaused)
            {
                UpdateMouseLook();
                UpdateMovement();
            }
        }

        private void Pause()
        {
            isPaused = true;
        }

        private void Unpause()
        {
            isPaused = false;
        }

        // Allows the mouse to be locked and unlocked when running in the Unity Editor.
        private void UpdateMouseLockedState()
        {
            if (Debug.isDebugBuild)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                    Cursor.lockState = CursorLockMode.None;
                if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
                    Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void UpdateMouseLook()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Vector2 mousePos = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
                MouseLookRotation.x += mousePos.y * MouseSensitivity * Time.deltaTime;
                MouseLookRotation.y += mousePos.x * MouseSensitivity * Time.deltaTime;
                MouseLookRotation.x = Mathf.Clamp(MouseLookRotation.x, -VerticalClampAngle, VerticalClampAngle);
                Quaternion localRotation = Quaternion.Euler(MouseLookRotation.x, MouseLookRotation.y, 0.0f);
                transform.rotation = localRotation;
            }
        }

        private void UpdateMovement()
        {
            Vector3 moveDirection = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
                moveDirection += Vector3.forward;
            if (Input.GetKey(KeyCode.S))
                moveDirection += Vector3.back;
            if (Input.GetKey(KeyCode.A))
                moveDirection += Vector3.left;
            if (Input.GetKey(KeyCode.D))
                moveDirection += Vector3.right;
            if (Input.GetKey(KeyCode.E))
                moveDirection += Vector3.up;
            if (Input.GetKey(KeyCode.Q))
                moveDirection += Vector3.down;

            float speedMultiplier = 1.0f;
            if (Input.GetKey(KeyCode.LeftShift))
                speedMultiplier = 2.0f;

            if (moveDirection != Vector3.zero)
                transform.Translate(moveDirection.normalized * MoveSpeed * speedMultiplier * Time.deltaTime);
        }
    }
}
