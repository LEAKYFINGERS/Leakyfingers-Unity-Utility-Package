//////////////////////////////////////////////////
// Author/s:            LEAKYFINGERS   
// Date created:        27.05.20
// Date last edited:    28.05.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace LeakyfingersUtility
{
    // An interactive object which can be picked up and held by the player to be used via the 'Use Equipped' input - requires that the player camera has an EquippableObjectManager script attached so that multiple equippable objects can be cycled between.
    [RequireComponent(typeof(AudioSource))]
    public class EquippableObject : InteractiveObject
    {
        public AudioClip EquipSound;
        public InteractiveObjectEventDelegate OnUseEquippedDelegate; // A delegate called when the player presses the 'use equipped' input while the object is equipped.   
        public Vector3 EquippedCameraOffsetPosition; // The local position of the equipped object once it has been 'picked up' and is parented to the player camera.
        public Vector3 EquippedCameraOffsetRotation; // The local rotation of the equipped object once it has been 'picked up' and is parented to the player camera.        

        public bool IsEquipped
        {
            get { return isEquipped; }
        }

        protected AudioSource audioSource;
        protected bool isEquipped; // Whether or not the object is currently being held by the player and can be used using the 'use equipped' input.
        protected bool wasUseEquippedPressedDuringPreviousUpdate;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        protected override void Update()
        {
            base.Update();

            // Updates the swaying motion of the object while equipped.
            if (isEquipped)                

            if (!isPaused && Input.GetAxis("Use Equipped") == 1.0f && !wasUseEquippedPressedDuringPreviousUpdate)
                OnUseEquipped();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            if (!isPaused)
                wasUseEquippedPressedDuringPreviousUpdate = Input.GetAxis("Use Equipped") == 1.0f;
        }

        // Called when the player presses the interact input while hovering over the interactive object within the scene to pick it up.
        protected override void OnInteract()
        {
            Assert.IsNotNull(Camera.main.GetComponent<EquippableObjectsManager>(), "The main camera must have an EquippableObjectsManager script attached in order for the EquippableObject " + name + " to be utilised.");

            if (OnInteractDelegate != null)
                OnInteractDelegate();
            
            transform.SetParent(Camera.main.transform, false);
            transform.localPosition = EquippedCameraOffsetPosition;
            transform.localRotation = Quaternion.Euler(EquippedCameraOffsetRotation);

            audioSource.PlayOneShot(EquipSound);            

            // Stops the equippable object from casting shadows while equipped.
            foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }

            IsInteractive = false;
            isEquipped = true;

            Camera.main.GetComponent<EquippableObjectsManager>().AddAndEquipObject(this);
        }

        // Called when the player presses the use equipped input while the object is equipped.
        protected virtual void OnUseEquipped()
        {
            if (OnUseEquippedDelegate != null)
                OnUseEquippedDelegate();
        }
    }
}
