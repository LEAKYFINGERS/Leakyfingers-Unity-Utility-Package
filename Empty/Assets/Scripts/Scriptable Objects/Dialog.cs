//////////////////////////////////////////////////
// Author/s:            LEAKYFINGERS   
// Date created:        11.01.20
// Date last edited:    25.05.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeakyfingersUtility
{
    // A scriptable object used to store a dialog snippet that can be displayed using the DialogUIDisplay script.
    [CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Dialog")]
    public class Dialog : ScriptableObject
    {
        public AudioClip SoundEffect;
        public bool LoopSoundEffectUntilTextIsRevealed; // If true, continues looping the sound effect until all of the dialog text is displayed - else if false plays only once.
        public float CharacterRevealInterval; // The duration of the pause between each character of the displayed dialog text being revealed.
        public float SoundEffectPitch = 1.0f;
        public string DisplayedText = "DIALOG TEXT";
    }
}
