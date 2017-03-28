using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairManager : MonoBehaviour {

    public GameObject Catalog;

    private RaycastHit Hit;
    private Transform Implant;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Transform cam = Camera.main.transform;
        //Debug.DrawRay(cam.position, cam.forward * 1000, Color.yellow);
        if (Physics.Raycast(cam.position, cam.forward, out Hit, 1000)) {
            if (Hit.transform.CompareTag("Implant")) {
                GetComponent<Renderer>().material.color = Color.green;
                if (Input.GetMouseButton(0)) {
                    Implant = Hit.transform;
                    Implant.SetParent(this.transform.parent);
                    Implant.localPosition = new Vector3(0, 10, 60);
                    Invoke("EnableTransform", 1);
                }
            } else {
                GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    void EnableTransform() {
        Implant.GetComponent<Transformer>().enabled = true;
        Catalog.SetActive(false);
        this.gameObject.SetActive(false);
    }


}
