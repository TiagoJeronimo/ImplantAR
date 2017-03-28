using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ScaleImageTarget : MonoBehaviour {

    public float MinDistance;
    public float MaxDistance;
    public float Scale;
    public float ScaleFactor = 0.01F;

    private bool ScaleOn = false;
    private Vector3 InitialScale;
    private float CurrentDistance;


    // Use this for initialization
    void Start () {
        InitialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update() {
        if (ScaleOn) {

            CurrentDistance = (transform.position - Camera.main.transform.position).magnitude;
            float norm = (CurrentDistance - MinDistance) / (MaxDistance - MinDistance);
            norm = Mathf.Clamp01(norm);

            Vector3 minScale = InitialScale;
            Vector3 maxScale = Vector3.one * ScaleFactor * Scale;

            transform.localScale = Vector3.Lerp(minScale, maxScale, norm);

            if (CurrentDistance >= MaxDistance) {
                ScaleOn = false;
            }
        }
    }

    public void ChangeScale() {
        ScaleOn = !ScaleOn;
    }

    public void EnableScale() {
        ScaleOn = true;
    }

    void OnGUI() {
        GUI.Label(new Rect(10, 10, Screen.width, Screen.height), "Distance from target: " + CurrentDistance);
    }
}
