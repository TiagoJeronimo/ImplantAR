using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImplantCollision : MonoBehaviour {

    public Color CollisionColor;

    private Color InitialColor;

    void Start() {
        InitialColor = GetComponent<Renderer>().material.color;
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Nerve")) {
            GetComponent<Renderer>().material.color = CollisionColor;
        } else {
            GetComponent<Renderer>().material.color = InitialColor;
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.CompareTag("Nerve")) {
            GetComponent<Renderer>().material.color = InitialColor;
        }
    }
}
