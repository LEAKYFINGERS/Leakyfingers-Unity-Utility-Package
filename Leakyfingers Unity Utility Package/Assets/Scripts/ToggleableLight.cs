//////////////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        08.03.20
// Date last edited:    28.05.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeakyfingersUtility
{
    // The script for a light object within the scene which can be toggled on and off and changes materials accordingly.
    [RequireComponent(typeof(Light))]
    [RequireComponent(typeof(MeshRenderer))]
    public class ToggleableLight : MonoBehaviour
    {
        public Material UnlitMaterial;
        public Material LitMaterial;
        public bool StartsLit;

        public bool IsLit
        {
            get { return isLit; }
            set
            {
                isLit = value;

                if (isLit)
                    lightSource.enabled = true;
                else
                    lightSource.enabled = false;

                UpdateMaterial();
            }
        }

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            lightSource = GetComponent<Light>();
            IsLit = StartsLit;
        }

        private void UpdateMaterial()
        {
            if (IsLit)
                GetComponent<MeshRenderer>().material = LitMaterial;
            else
                GetComponent<MeshRenderer>().material = UnlitMaterial;
        }

        private MeshRenderer meshRenderer;
        private Light lightSource;
        private bool isLit;
    }
}
