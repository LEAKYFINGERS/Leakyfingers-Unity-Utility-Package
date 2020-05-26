//////////////////////////////////////////////////
// Author/s:            LEAKYFINGERS   
// Date created:        23.08.19
// Date last edited:    25.05.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeakyfingersUtility
{
    // A script which provides access to a coroutine that causes the gameobject to 'shake' by adjusting the local position for the specified duration - attach to a parent if the gameobject needs to move while shaking.
    public class Shake : MonoBehaviour
    {
        public bool IsShaking
        {
            get { return isShakeCoroutineRunning; }
        }

        public void StartShaking(float duration, float magnitude)
        {
            if (!isShakeCoroutineRunning)
            {
                timer = duration;
                StartCoroutine("ShakeCoroutine", magnitude);
            }
        }

        private bool isShakeCoroutineRunning;
        private float timer;

        private IEnumerator ShakeCoroutine(float magnitude)
        {
            isShakeCoroutineRunning = true;

            Vector3 initialPos = transform.localPosition;
            while (timer > 0.0f)
            {
                // If the gameobject isn't in the 'default' local position, moves it in a random direction according to the magnitude parameter.
                if (transform.localPosition.x == initialPos.x && transform.localPosition.y == initialPos.y)
                {
                    Vector3 translation = Vector3.zero;
                    while (translation == Vector3.zero)
                    {
                        translation = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
                    }
                    transform.localPosition += translation.normalized * magnitude;
                }
                else
                    transform.localPosition = initialPos;

                timer -= Time.deltaTime;

                yield return null;
            }
            transform.localPosition = initialPos;

            isShakeCoroutineRunning = false;
        }
    }
}
