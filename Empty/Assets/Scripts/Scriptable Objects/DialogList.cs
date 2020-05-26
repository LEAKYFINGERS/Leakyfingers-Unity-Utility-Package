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
    // A scriptable object used to store a list of dialog snippets that can be displayed using the DialogUIDisplay script.
    [CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Dialog List")]
    public class DialogList : ScriptableObject
    {
        public List<Dialog> Dialogs;
    }
}
