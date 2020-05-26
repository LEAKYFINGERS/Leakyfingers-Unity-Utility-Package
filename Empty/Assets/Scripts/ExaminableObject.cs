/////////////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        15.01.20
// Date last edited:    26.05.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeakyfingersUtility
{
    // The script for an interactive object which will trigger the examine dialogs when interacted with by the player.
    public class ExaminableObject : InteractiveObject
    {
        protected override void OnInteract()
        {
            base.OnInteract();

            OnExamine();
        }
    }
}
