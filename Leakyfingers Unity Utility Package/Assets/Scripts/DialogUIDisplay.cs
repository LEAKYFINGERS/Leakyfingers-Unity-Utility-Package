//////////////////////////////////////////////////
// Author/s:            LEAKYFINGERS   
// Date created:        09.01.20
// Date last edited:    25.05.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LeakyfingersUtility
{
    // A script used to display dialog snippets to the player through a UI text object.
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Text))]
    public class DialogUIDisplay : MonoBehaviour
    {
        public ScenePause ScenePauseScript; // The script used to pause the scene when dialog is being displayed.
        public Image BackgroundImage; // The optional UI image to display as a background behind the dialog.

        public bool IsDisplayingDialog
        {
            get { return isDisplaySingleDialogCoroutineRunning; }
        }

        // Pauses the scene to display the dialog snippet and play the associated sound effect.
        public void DisplayDialog(Dialog dialog, bool pauseSceneWhileDialogIsDisplayed = true)
        {
            if (!isDisplaySingleDialogCoroutineRunning)
            {
                List<Dialog> dialogs = new List<Dialog>();
                dialogs.Add(dialog);

                pauseSceneWhileCoroutineIsRunning = pauseSceneWhileDialogIsDisplayed;

                StartCoroutine("DisplayDialogsCoroutine", dialogs);
            }
        }
        // Pauses the scene to display each of the dialog snippets and play the associated sound effects.
        public void DisplayDialog(DialogList dialogList, bool pauseSceneWhileDialogIsDisplayed = true)
        {
            if (!isDisplaySingleDialogCoroutineRunning)
            {
                pauseSceneWhileCoroutineIsRunning = pauseSceneWhileDialogIsDisplayed;

                StartCoroutine("DisplayDialogsCoroutine", dialogList.Dialogs);
            }
        }

        private AudioSource audioSource;
        private Text text;
        private bool isDisplaySingleDialogCoroutineRunning;
        private bool pauseSceneWhileCoroutineIsRunning;

        private void Awake()
        {
            if (BackgroundImage)
                BackgroundImage.enabled = false;

            audioSource = GetComponent<AudioSource>();
            text = GetComponent<Text>();
            text.enabled = false;
        }

        // A coroutine which pauses the scene to display each of the dialog snippets in the list until the player presses the 'cycle dialog' input to move through them.
        private IEnumerator DisplayDialogsCoroutine(List<Dialog> dialogs)
        {
            if (pauseSceneWhileCoroutineIsRunning)
                ScenePauseScript.PauseScene();
            if (BackgroundImage)
                BackgroundImage.enabled = true;
            text.enabled = true;

            foreach (Dialog dialog in dialogs)
            {
                text.text = "";

                StartCoroutine("DisplaySingleDialogCoroutine", dialog);
                while (isDisplaySingleDialogCoroutineRunning)
                {
                    yield return null;
                }
            }
            if (pauseSceneWhileCoroutineIsRunning)
                ScenePauseScript.UnpauseScene();

            if (BackgroundImage)
                BackgroundImage.enabled = false;
            text.enabled = false;
        }

        // A coroutine which displays an individual dialog snippet and plays the associated sound effect until the player presses the 'examine' input.
        private IEnumerator DisplaySingleDialogCoroutine(Dialog dialog)
        {
            isDisplaySingleDialogCoroutineRunning = true;

            audioSource.clip = dialog.SoundEffect;
            audioSource.loop = dialog.LoopSoundEffectUntilTextIsRevealed;
            audioSource.pitch = dialog.SoundEffectPitch;
            audioSource.Play();

            bool cycleDialogInputReleased = false; // Used to tell whether the cycle dialog input was released since the last time it was pressed.

            // Displays the text instantly if the character display interval isn't greater than zero.
            if (dialog.CharacterRevealInterval <= 0.0f)
                text.text = dialog.DisplayedText;
            // Else, adds each character to the displayed text one at a time.
            else
            {
                float timer = 0.0f;
                int c = 0; // The index of the next character in the dialog string to add to the displayed text.
                while (text.text != dialog.DisplayedText)
                {
                    // If the interact or examine input has been released and re-pressed, skips to displaying the entire dialog string at once.
                    if (Input.GetAxis("Cycle Dialog") == 1.0f && cycleDialogInputReleased)
                    {
                        text.text = dialog.DisplayedText;
                        break;
                    }
                    else if (timer >= dialog.CharacterRevealInterval)
                    {
                        text.text += dialog.DisplayedText[c++];
                        timer = 0.0f;
                    }

                    if (Input.GetAxis("Cycle Dialog") == 0.0f)
                        cycleDialogInputReleased = true;

                    timer += Time.deltaTime;
                    yield return null;
                }
            }
            audioSource.Stop();

            cycleDialogInputReleased = false;
            // Once the entire dialog string is being displayed, waits for the player to press the cycle dialog input before ending the coroutine.
            while (true)
            {
                if (Input.GetAxis("Cycle Dialog") == 1.0f && cycleDialogInputReleased)
                    break;
                else if (Input.GetAxis("Cycle Dialog") == 0.0f)
                    cycleDialogInputReleased = true;

                yield return null;
            }

            isDisplaySingleDialogCoroutineRunning = false;
        }
    }
}
