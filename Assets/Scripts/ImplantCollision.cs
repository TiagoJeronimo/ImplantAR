using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImplantCollision : MonoBehaviour {

    public Color CollisionColor;

    private Color InitialColor;

    public GameObject Mandible;

    void Start() {
        InitialColor = GetComponent<Renderer>().material.color;
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Nerve")) {
            GetComponent<Renderer>().material.color = CollisionColor;

            Renderer[] renderers = Mandible.GetComponentsInChildren<Renderer>();
            renderers[0].sharedMaterial.color = new Color(1, 1, 1, 0.8f);

        } else {
            GetComponent<Renderer>().material.color = InitialColor;

            Renderer[] renderers = Mandible.GetComponentsInChildren<Renderer>();
            renderers[0].sharedMaterial.color = new Color(1, 1, 1, 1);
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.CompareTag("Nerve")) {
            GetComponent<Renderer>().material.color = InitialColor;

            Renderer[] renderers = Mandible.GetComponentsInChildren<Renderer>();
            renderers[0].sharedMaterial.color = new Color(1, 1, 1, 1);

        }
    }
}
