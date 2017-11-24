using UnityEngine;
using System.Collections;
using Vuforia;
 
public class BackgroundOff : MonoBehaviour {
 
    private bool mBackgroundWasSwitchedOff = false;
 
    // Update is called once per frame
    void Update () {
        if (!mBackgroundWasSwitchedOff) {
            BackgroundPlaneBehaviour bgPlane = transform.GetComponentInChildren<BackgroundPlaneBehaviour> ();
            if (bgPlane.enabled) {
                // switch it off
                bgPlane.enabled = false;
            }
            mBackgroundWasSwitchedOff = true;
        }
    }
}