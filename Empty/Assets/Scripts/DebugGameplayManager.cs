using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeakyfingersUtility;

public class DebugGameplayManager : MonoBehaviour
{
    public DialogList TestDialogList;
    public DialogUIDisplay DialogDisplay;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            DialogDisplay.DisplayDialog(TestDialogList);
    }
}
