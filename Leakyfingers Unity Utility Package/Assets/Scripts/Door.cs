//////////////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        04.01.20
// Date last edited:    28.05.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeakyfingersUtility
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Shake))]
    public class Door : InteractiveObject
    {
        public AudioClip OpenSound;
        public AudioClip CloseSound;
        public AudioClip LockedSound;
        public bool IsLocked;
        public float OpenedRotation = -90.0f; // The local rotation of the door around its y-axis when fully opened.
        public float OpenSpeed = 1.0f; // The duration it takes for the door to change between closed and open.
        public float CloseSpeed = 1.0f; // The duration it takes for the door to change between open and closed.
        public float RemainOpenDuration = -1.0f; // The duration which the door will remain open before closing automatically - set to a value less than zero to remain open indefinitely. 
        public float LockedInteractShakeDuration = 0.2f;
        public float LockedInteractShakeMagnitude = 0.025f;

        public bool IsOpen
        {
            get { return isOpen; }
        }

        protected override void Update()
        {
            base.Update();

            // Updates automatic closing of the door.
            if (!isPaused && RemainOpenDuration >= 0.0f && !isToggleOpenCoroutineRunning)
            {
                if (isOpen)
                {
                    remainOpenTimer += Time.deltaTime;
                    if (remainOpenTimer >= RemainOpenDuration)
                        Close();
                }
                else
                    remainOpenTimer = 0.0f;
            }
        }

        private AudioSource audioSource;
        private bool isOpen;
        private bool isToggleOpenCoroutineRunning;
        private float remainOpenTimer;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();

            OnInteractDelegate += ToggleOpened;
        }

        private void ToggleOpened()
        {
            if (!isOpen)
                Open();
            else
                Close();
        }

        private void Open()
        {
            if (!isToggleOpenCoroutineRunning)
            {
                if (IsLocked)
                {
                    audioSource.clip = LockedSound;
                    audioSource.Play();

                    GetComponent<Shake>().StartShaking(LockedInteractShakeDuration, LockedInteractShakeMagnitude);
                }
                else
                {
                    audioSource.clip = OpenSound;
                    audioSource.Play();

                    StartCoroutine("ToggleOpenCoroutine");                    
                }
            }
        }

        private void Close()
        {
            if (!isToggleOpenCoroutineRunning)
            {
                audioSource.clip = CloseSound;
                audioSource.Play();

                StartCoroutine("ToggleOpenCoroutine");
            }
        }

        // A coroutine which moves the door from open-to-closed or vice-versa.
        private IEnumerator ToggleOpenCoroutine()
        {
            isToggleOpenCoroutineRunning = true;
            IsInteractive = false;

            Quaternion initialRotation = transform.localRotation;
            Quaternion goalRotation;
            float toggleOpenDuration;
            if (initialRotation == Quaternion.identity)
            {
                goalRotation = Quaternion.AngleAxis(OpenedRotation, Vector3.up);
                toggleOpenDuration = OpenSpeed;
            }
            else
            {
                goalRotation = Quaternion.identity;
                toggleOpenDuration = CloseSpeed;
            }

            float timer = 0.0f;
            while (timer < toggleOpenDuration)
            {
                transform.localRotation = Quaternion.Lerp(initialRotation, goalRotation, timer / toggleOpenDuration);

                timer += Time.deltaTime;
                yield return null;
            }

            transform.localRotation = goalRotation;

            isToggleOpenCoroutineRunning = false;
            IsInteractive = true;
            isOpen = !isOpen;
        }
    }
}
