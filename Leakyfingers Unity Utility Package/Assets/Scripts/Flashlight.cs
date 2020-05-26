//////////////////////////////////////////////////
// Author/s:            LEAKYFINGERS
// Date created:        29.01.19
// Date last edited:    26.05.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeakyfingersUtility
{
    // The script used to handle a player flashlight which slowly orients itself in the same direction of the player camera and can be switched on and off using the 'Flashlight' input.
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Light))]
    public class Flashlight : MonoBehaviour
    {
        public AudioClip ToggleSound; // The sound played when the flashlight is turned on or off.
        public Transform PlayerCamera; // The player camera from which the flashlight derives its orientation.    
        public float AimLag = 7.5f; // Modifies the speed at which the flashlight will catch up to the current orientation of the player.

        private AudioSource audioSource;
        private Light lightComponent;
        private Quaternion rotationDuringPreviousUpdate; 
        private bool wasFlashlightInputDownDuringPreviousUpdate;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = ToggleSound;
            lightComponent = GetComponent<Light>();
        }

        private void Update()
        {
            // Toggles the flashlight on/off if the appropriate input is pressed.
            if (Input.GetAxis("Flashlight") == 1.0f && !wasFlashlightInputDownDuringPreviousUpdate)
            {
                lightComponent.enabled = !lightComponent.enabled;
                GetComponent<AudioSource>().Play();
            }
        }

        private void LateUpdate()
        {
            // Updates the rotation of the flashlight to match that of the player camera.
            transform.rotation = Quaternion.Lerp(rotationDuringPreviousUpdate, PlayerCamera.rotation, AimLag * Time.deltaTime);
            rotationDuringPreviousUpdate = transform.rotation;

            wasFlashlightInputDownDuringPreviousUpdate = Input.GetAxis("Flashlight") == 1.0f;
        }
    }
}
