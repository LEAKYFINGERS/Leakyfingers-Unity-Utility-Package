//////////////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        27.03.20
// Date last edited:    28.05.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeakyfingersUtility
{
    // A script which allows the player to control a camera which orbits around a specified target using the mouse. 
    [RequireComponent(typeof(Camera))]
    public class OrbitingCamera : MonoBehaviour
    {
        public Transform Target;
        public float OrbitDistance = 5.0f;
        public float MouseSensitivity = 3.0f;
        public float VerticalClampAngle = 80.0f; // The maximum possible angle between the horizontal plane of the target and a vector to the camera position.

        private Vector3 offset;
        private bool isPaused;

        private void Awake()
        {
            offset = Vector3.back * OrbitDistance;
            transform.LookAt(Target);

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            UpdateMouseLockedState();
            if (!isPaused && Cursor.lockState == CursorLockMode.Locked)
                UpdateOrbiting();
        }

        private void Pause()
        {
            isPaused = true;
        }

        private void Unpause()
        {
            isPaused = false;
        }

        private void UpdateMouseLockedState()
        {
            if (Debug.isDebugBuild)
            {
                if (Input.GetKeyDown(KeyCode.Escape) && Cursor.lockState == CursorLockMode.Locked)
                    Cursor.lockState = CursorLockMode.None;
                else if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
                    Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void UpdateOrbiting()
        {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * MouseSensitivity, Vector3.up) * Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * MouseSensitivity, transform.right) * offset; // Rotates the camera offset around the global up-axis and its own right-axis according to the mouse position delta.      
                                                                                                                                                                                                   // Clamps the angle between the horizontal plane of the target and a vector to the camera position. 
            if (Vector3.Angle(offset.normalized, new Vector3(offset.x, 0.0f, offset.z).normalized) > VerticalClampAngle)
            {
                if (offset.y > 0.0f)
                    offset = Quaternion.AngleAxis(VerticalClampAngle, transform.right) * new Vector3(offset.x, 0.0f, offset.z).normalized * OrbitDistance;
                else
                    offset = Quaternion.AngleAxis(-VerticalClampAngle, transform.right) * new Vector3(offset.x, 0.0f, offset.z).normalized * OrbitDistance;
            }

            if (offset.magnitude != OrbitDistance)
                offset = offset.normalized * OrbitDistance;

            transform.position = Target.position + offset;
            transform.LookAt(Target);
        }
    }
}
