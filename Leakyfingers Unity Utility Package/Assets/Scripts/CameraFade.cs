//////////////////////////////////////////////////
// Author/s:            LEAKYFINGERS   
// Date created:        12.09.19
// Date last edited:    25.05.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace LeakyfingersUtility
{
    // A script which uses the ColorTintPostProcessEffect as well as volume adjustment to create 'camera fade in/out' effects.
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(PostProcessVolume))]
    public class CameraFade : MonoBehaviour
    {
        // The direction of the camera fade effect.
        public enum FadeDirection
        {
            FadeToColor,
            FadeFromColor
        }

        public float FadeInCameraOnAwakeIntialPauseDuration; // The duration of the initial pause before the camera fade-in when the script is loaded.
        public float FadeInCameraOnAwakeDuration; // The duration of the camera fade-in when the script is loaded - set to 0.0f for no initial fade-in.

        public bool IsFadeCoroutineRunning
        {
            get { return isFadeCoroutineRunning; }
        }

        // Starts a coroutine which fades the camera and audio in or out.
        public void Fade(Color color, FadeDirection direction, float fadeDuration, float initialPauseDuration = 0.0f, bool fadeVolume = true)
        {
            ColorTintPostProcessEffect colorTint;
            postProcessVolume.profile.TryGetSettings(out colorTint);
            colorTint.Color.value = color;

            if (isFadeCoroutineRunning)
                StopCoroutine("FadeCoroutine");

            coroutineFadeDirection = direction;
            coroutineInitialPauseDuration = initialPauseDuration;
            coroutineFadeDuration = fadeDuration;
            if (Camera.main.isActiveAndEnabled)
                StartCoroutine("FadeCoroutine", fadeVolume);
        }

        private PostProcessVolume postProcessVolume;
        private FadeDirection coroutineFadeDirection; // The current value of the direction parameter of the Fade coroutine.    
        private bool isFadeCoroutineRunning;
        private float coroutineInitialPauseDuration;
        private float coroutineFadeDuration; // The current value of the duration parameter of the fade coroutine.

        // A coroutine which fades the camera and audio out or in depending on the member values being used as parameters.
        private IEnumerator FadeCoroutine(bool fadeVolume)
        {
            isFadeCoroutineRunning = true;

            ColorTintPostProcessEffect colorTint;
            postProcessVolume.profile.TryGetSettings(out colorTint);

            // Initial pause before the fade happens.
            float timer = 0.0f;
            if (coroutineFadeDirection == FadeDirection.FadeFromColor)
            {
                colorTint.Blend.value = 1.0f;
                if (fadeVolume)
                    AudioListener.volume = 0.0f;
            }
            while (timer < coroutineInitialPauseDuration)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            // Carries out the fade effect in the specified direction ('fade in' or 'fade out').
            timer = 0.0f;
            while (timer < coroutineFadeDuration)
            {
                if (coroutineFadeDirection == FadeDirection.FadeToColor)
                {
                    colorTint.Blend.value = Mathf.Lerp(0.0f, 1.0f, timer / coroutineFadeDuration);
                    if (fadeVolume)
                        AudioListener.volume = Mathf.Lerp(1.0f, 0.0f, timer / coroutineFadeDuration);
                }
                else
                {
                    colorTint.Blend.value = Mathf.Lerp(1.0f, 0.0f, timer / coroutineFadeDuration);
                    if (fadeVolume)
                        AudioListener.volume = Mathf.Lerp(0.0f, 1.0f, timer / coroutineFadeDuration);
                }

                timer += Time.deltaTime;
                yield return null;
            }

            // Ensures that the coroutine exits which the fade values correctly set.
            if (coroutineFadeDirection == FadeDirection.FadeToColor)
            {
                colorTint.Blend.value = 1.0f;
                if (fadeVolume)
                    AudioListener.volume = 0.0f;
            }
            else
            {
                colorTint.Blend.value = 0.0f;
                if (fadeVolume)
                    AudioListener.volume = 1.0f;
            }

            isFadeCoroutineRunning = false;
        }

        // Called when the script is loaded.
        private void Awake()
        {
            postProcessVolume = GetComponent<PostProcessVolume>();

            if (FadeInCameraOnAwakeDuration > 0.0f)
                Fade(Color.black, FadeDirection.FadeFromColor, FadeInCameraOnAwakeDuration, FadeInCameraOnAwakeIntialPauseDuration);
        }
    }
}
