using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeakyfingersUtility;

public class DebugGameplayManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!ScenePause.Instance.IsScenePaused)
            {
                ScenePause.Instance.PauseScene();
            }
            else
            {
                ScenePause.Instance.UnpauseScene();
            }
        }
    }
}
