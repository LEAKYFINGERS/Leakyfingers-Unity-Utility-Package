//////////////////////////////////////////////////
// Author/s:            LEAKYFINGERS   
// Date created:        28.05.20
// Date last edited:    28.05.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace LeakyfingersUtility
{
    // The script used to switch between all the EquippableObjects that have been picked up by the player - must be attached to the main camera in order for any EquippableObjects to be used in the scene.
    [RequireComponent(typeof(Camera))]
    public class EquippableObjectsManager : MonoBehaviour
    {
        public float EquippedObjectSwayMagnitude = 0.01f; // The distance each currently-equipped object moves up and down relative to the camera when equipped. 
        public float EquippedObjectSwaySpeed = 1.0f; // The speed at which each currently-equipped object moves up and down relative to the camera when equipped. 

        // Adds an EquippableObject to the list and sets it as the currently-equipped item.
        public void AddAndEquipObject(EquippableObject equippableObject)
        {
            equippableObjects.Add(equippableObject);

            EquipObject(equippableObjects.Count - 1);
        }

        private List<EquippableObject> equippableObjects;
        private bool isPaused;
        private int currentlyEquippedObjectIndex;

        private void Awake()
        {
            equippableObjects = new List<EquippableObject>();
        }

        private void Update()
        {
            if (!isPaused)
            {
                if (Input.GetAxis("Cycle Inventory") > 0.0f)
                    CycleToNextEquippedObjectInList();
                else if (Input.GetAxis("Cycle Inventory") < 0.0f)
                    CycleToPreviousEquippedObjectInList();

                // Updates the swaying movement of the currently-equipped object.
                if (equippableObjects.Count > 0)
                    equippableObjects[currentlyEquippedObjectIndex].transform.localPosition = equippableObjects[currentlyEquippedObjectIndex].EquippedCameraOffsetPosition + (EquippedObjectSwayMagnitude * Vector3.up * Mathf.Sin(Time.timeSinceLevelLoad * EquippedObjectSwaySpeed));
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

        // Increments the currently-equipped object within the list.
        private void CycleToNextEquippedObjectInList()
        {
            if (equippableObjects.Count <= 1)
                return;

            if (++currentlyEquippedObjectIndex > equippableObjects.Count - 1)
                currentlyEquippedObjectIndex = 0;
            EquipObject(currentlyEquippedObjectIndex);
        }

        // Decrements the currently-equipped object within the list.
        private void CycleToPreviousEquippedObjectInList()
        {
            if (equippableObjects.Count <= 1)
                return;

            if (--currentlyEquippedObjectIndex < 0)
                currentlyEquippedObjectIndex = equippableObjects.Count - 1;
            EquipObject(currentlyEquippedObjectIndex);
        }

        // Activates the equippable object with the specified index while deactivating all others in the list and updates the 'currently equipped' index.
        private void EquipObject(int indexInList)
        {
            Assert.IsTrue(indexInList < equippableObjects.Count);

            for(int i = 0; i < equippableObjects.Count; ++i)
            {
                if (i == indexInList)
                {
                    equippableObjects[i].gameObject.SetActive(true);
                    equippableObjects[i].GetComponent<AudioSource>().PlayOneShot(equippableObjects[i].EquipSound);
                }
                else
                    equippableObjects[i].gameObject.SetActive(false);                
            }

            currentlyEquippedObjectIndex = indexInList;
        }
    }
}
