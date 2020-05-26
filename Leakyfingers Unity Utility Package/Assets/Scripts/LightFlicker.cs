//////////////////////////////////////////////////
// Author/s:            LEAKYFINGERS
// Date created:        29.01.19
// Date last edited:    23.05.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeakyfingersUtility
{
    [RequireComponent(typeof(Light))]
    public class LightFlicker : MonoBehaviour
    {
        public float Speed = 50.0f;
        public float Magnitude = 50.0f;

        private Light lightComponent;
        private static float memberMultiplier = 50.0f;
        private float baseIntensity; // The base-level intensity of the light around which the intensity will increase and decrease to create the flickering effect.

        private void Awake()
        {
            lightComponent = GetComponent<Light>();
            baseIntensity = lightComponent.intensity;
        }

        private void Update()
        {
            lightComponent.intensity = baseIntensity + (Mathf.Sin(Time.realtimeSinceStartup * memberMultiplier * Speed) / (memberMultiplier / Magnitude));
        }
    }
}
