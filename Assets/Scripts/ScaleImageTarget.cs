using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ScaleImageTarget : MonoBehaviour {

    public float MinDistance;
    public float MaxDistance;
    public float ScaleFactor;

    Vector3 InitialScale;
    bool Scale = true;
    float CurrentDistance;


    // Use this for initialization
    void Start () {
        InitialScale = transform.localScale;
            }
	
	// Update is called once per frame
	void Update () {
        if (Scale) {
            Vector3 delta = Camera.main.transform.position - this.transform.position;
            CurrentDistance = delta.magnitude;
            //Debug.Log("Distance: " + currentDistance);

            if (CurrentDistance <= MinDistance) {
                transform.localScale = InitialScale;
            } else if (CurrentDistance >= MaxDistance) {
                Scale = false;
            } else if (CurrentDistance > MinDistance && CurrentDistance < MaxDistance) {
                transform.localScale = InitialScale * CurrentDistance * ScaleFactor;
            }
           
        }
    }

    public void EnableScale() {
        Scale = !Scale;
    }

    void OnGUI() {
        GUI.Label(new Rect(10, 10, Screen.width, Screen.height), "Distance from target: " + CurrentDistance);
    }
}
