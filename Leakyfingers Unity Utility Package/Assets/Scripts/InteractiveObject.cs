//////////////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        07.12.19
// Date last edited:    26.05.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LeakyfingersUtility
{
    // A base script for an object which the player can interact with and examine.
    [RequireComponent(typeof(Collider))]
    public class InteractiveObject : MonoBehaviour
    {
        public delegate void InteractiveObjectEventDelegate(); // The description of the delegate format used to inform other objects of the interactive object's events.

        public DialogUIDisplay ExamineDialogUIDisplay; // The UI object used to display the examine dialogs when the interactive object is examined.
        public DialogList ExamineDialogs; // The list of dialogs that are displayed when the object is examined.
        public InteractiveObjectEventDelegate OnHoverStartDelegate; // A delegate called during the first frame when the player camera is within interaction range and aiming at the interactive object's collider.
        public InteractiveObjectEventDelegate OnHoverContinueDelegate; // A delegate called during the each subsequent frame after the first when the player camera is within interaction range and aiming at the interactive object's collider.
        public InteractiveObjectEventDelegate OnHoverEndDelegate; // A delegate called when the player camera stops being within interaction range and/or aiming at the interactive object's collider.
        public InteractiveObjectEventDelegate OnInteractDelegate;  // A delegate called when the player presses the 'interact' input while hovering over the interactive object.     
        public InteractiveObjectEventDelegate OnExamineDelegate;  // A delegate called when the player presses the 'examine' input while hovering over the interactive object.   
        public bool IsInteractive = true;
        public float InteractRange = 1.5f; // The range of distance between the player camera and the interactive object within which the player can interact with it.

        // A property used to get whether the interactive object is currently paused or not.
        public bool IsPaused
        {
            get { return isPaused; }
        }

        // A property used to get whether the player camera is within the interaction range and aiming at the interactive object's collider.
        public bool IsPlayerHovering
        {
            get { return isPlayerHovering; }
        }

        protected bool isPaused;
        protected bool isPlayerHovering; // Whether the player camera is within the interaction range and aiming at the interactive object's collider.
        protected bool wasPlayerHoveringDuringPreviousUpdate; // Whether the player camera was within the interaction range and aiming at the interactive object's collider during the previous update.
        protected bool wasInteractPressedDuringPreviousUpdate;
        protected bool wasExaminePressedDuringPreviousUpdate;

        protected virtual void Update()
        {
            if (IsInteractive && !isPaused)
            {
                // Checks whether the player camera is within the interaction range and aiming at the interactive object's collider.
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));  // A ray cast from the center of the viewport.
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject == this.gameObject && hit.distance <= InteractRange)
                    {
                        isPlayerHovering = true;
                        if (!wasPlayerHoveringDuringPreviousUpdate)
                            OnHoverStart();
                        else
                            OnHoverContinue();
                    }
                    else
                        isPlayerHovering = false;
                }
                else
                    isPlayerHovering = false;

                if (!isPlayerHovering && wasPlayerHoveringDuringPreviousUpdate)
                    OnHoverEnd();
                if (isPlayerHovering && Input.GetAxis("Interact") == 1.0f && !wasInteractPressedDuringPreviousUpdate)
                    OnInteract();
                if (isPlayerHovering && Input.GetAxis("Examine") == 1.0f && !wasExaminePressedDuringPreviousUpdate)
                    OnExamine();
            }
        }

        protected virtual void LateUpdate()
        {
            if (!isPaused)
            {
                wasPlayerHoveringDuringPreviousUpdate = isPlayerHovering;
                wasInteractPressedDuringPreviousUpdate = Input.GetAxis("Interact") == 1.0f;
                wasExaminePressedDuringPreviousUpdate = Input.GetAxis("Examine") == 1.0f;
            }
        }

        protected virtual void Pause()
        {
            isPaused = true;
        }

        protected virtual void Unpause()
        {
            isPaused = false;
        }

        // Called during the first frame when the player camera is within interaction range and aiming at the interactive object's collider.
        protected virtual void OnHoverStart()
        {
            if (OnHoverStartDelegate != null)
                OnHoverStartDelegate();
        }

        // Called during the each subsequent frame after the first when the player camera is within interaction range and aiming at the interactive object's collider.
        protected virtual void OnHoverContinue()
        {
            if (OnHoverContinueDelegate != null)
                OnHoverContinueDelegate();
        }

        // Called when the player camera stops being within interaction range and/or aiming at the interactive object's collider.
        protected virtual void OnHoverEnd()
        {
            if (OnHoverEndDelegate != null)
                OnHoverEndDelegate();
        }

        // Called when the player presses the interact input while hovering over the interactive object.
        protected virtual void OnInteract()
        {
            if (OnInteractDelegate != null)
                OnInteractDelegate();
        }

        // Called when the player presses the examine input while hovering over the interactive object.
        protected virtual void OnExamine()
        {
            ExamineDialogUIDisplay.DisplayDialog(ExamineDialogs);

            if (OnExamineDelegate != null)
                OnExamineDelegate();
        }
    }
}
