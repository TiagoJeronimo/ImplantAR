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


    // Use this for initialization
    void Start () {
        InitialScale = transform.localScale;
            }
	
	// Update is called once per frame
	void Update () {
        if (Scale) {
            Vector3 delta = Camera.main.transform.position - this.transform.position;
            float currentDistance = delta.magnitude;
            //Debug.Log("Distance: " + currentDistance);

            if (currentDistance <= MinDistance) {
                transform.localScale = InitialScale;
            } else if (currentDistance >= MaxDistance) {
                Scale = false;
            } else if (currentDistance > MinDistance && currentDistance < MaxDistance) {
                transform.localScale = InitialScale * currentDistance * ScaleFactor;
            }
           
        }
    }
}
