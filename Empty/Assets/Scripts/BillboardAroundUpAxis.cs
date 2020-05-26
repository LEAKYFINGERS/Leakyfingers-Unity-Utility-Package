//////////////////////////////////////////////////
// Author/s:            LEAKYFINGERS
// Date created:        20.01.19
// Date last edited:    25.05.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeakyfingersUtility
{
    // A script which causes the gameobject to billboard towards the camera while remaining locked around the local up axis.
    public class BillboardAroundUpAxis : MonoBehaviour
    {
        private void Update()
        {
            UpdateBillboarding();
        }

        // Updates the rotation of the gameobject so that it billboards along the local x and z axis towards the player camera.
        private void UpdateBillboarding()
        {
            Vector3 dir = Camera.main.transform.position - this.transform.position; // A vector from the gameobject to the camera.       
            float angle = Vector3.SignedAngle(dir, this.transform.right, this.transform.up);  // The angle along the local up axis of the sprite between the vector to the player and the vector to the right of the sprite (along the sprite texture).        
            transform.Rotate(this.transform.up, 90.0f - angle, Space.World); // Rotates the sprite along the up axis so that the angle between the two vectors is exactly 90 degrees, meaning the sprite is billboarded directly towards the player camera on both axis other than the local up-axis.
        }
    }
}
