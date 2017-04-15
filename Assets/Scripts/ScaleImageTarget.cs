using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ScaleImageTarget : MonoBehaviour {

    public float MinDistance;
    public float MaxDistance;
    public float Scale;

    [HideInInspector]
    public float Norm;

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
            Norm = (CurrentDistance - MinDistance) / (MaxDistance - MinDistance);
            Norm = Mathf.Clamp01(Norm);

            Vector3 maxScale = InitialScale * Scale;

            transform.localScale = Vector3.Lerp(InitialScale, maxScale, Norm);

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
        GUIStyle style = new GUIStyle();
        style.fontSize = Screen.height * 2 / 30;
        style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        GUI.Label(new Rect(0, 100, Screen.width, Screen.height), "Distance from target: " + CurrentDistance, style);
    }
}
