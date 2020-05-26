//////////////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        04.02.20
// Date last edited:    25.05.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeakyfingersUtility
{
    [RequireComponent(typeof(Camera))]
    public class MouseLookCamera : MonoBehaviour
    {
        public float MouseSensitivity = 100.0f;
        public float HorizontalClampAngle = 80.0f;
        public float LowerVerticalClampAngle = 80.0f;
        public float UpperVerticalClampAngle = 80.0f;

        public void ResetMouseLookRotation()
        {
            MouseLookRotation = Vector2.zero;
        }

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
                UpdateMouseLook();
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
                MouseLookRotation.x = Mathf.Clamp(MouseLookRotation.x, -UpperVerticalClampAngle, LowerVerticalClampAngle);
                MouseLookRotation.y = Mathf.Clamp(MouseLookRotation.y, -HorizontalClampAngle, HorizontalClampAngle);
                Quaternion localRotation = Quaternion.Euler(MouseLookRotation.x, MouseLookRotation.y, 0.0f);
                transform.localRotation = localRotation;
            }
        }
    }
}
