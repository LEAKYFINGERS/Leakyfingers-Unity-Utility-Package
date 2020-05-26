//////////////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        01.01.20
// Date last edited:    25.05.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LeakyfingersUtility
{
    // A script used to display an interaction reticle when the player is within range and looking at an object with the InteractiveObject script attached.
    [RequireComponent(typeof(InteractiveObject))]
    public class InteractiveObjectReticle : MonoBehaviour
    {
        public Canvas DisplayCanvas; // The UI canvas used to display the interaction reticle.
        public Image ReticlePrefab; // The UI image prefab used to spawn the reticle which appears when interaction is possible.

        private Image reticle;
        private InteractiveObject interactiveObject;
        private bool isVisible;
        private bool isPaused;

        public void Start()
        {
            interactiveObject = GetComponent<InteractiveObject>();

            reticle = Instantiate(ReticlePrefab, DisplayCanvas.GetComponent<RectTransform>());
            reticle.enabled = false;

            interactiveObject.OnHoverStartDelegate += SetVisible;
            interactiveObject.OnHoverEndDelegate += SetInvisible;
        }

        private void Update()
        {
            if (isVisible && !isPaused && interactiveObject.IsInteractive)
                reticle.enabled = true;
            else
                reticle.enabled = false;
        }

        private void Pause()
        {
            isPaused = true;
        }

        private void Unpause()
        {
            isPaused = false;
        }

        private void SetVisible()
        {
            isVisible = true;
        }

        private void SetInvisible()
        {
            isVisible = false;
        }
    }
}
