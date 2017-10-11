using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairManager : MonoBehaviour {

    public GameObject Catalog;
    public Texture OpenHand;
    public Texture CloseHand;
    public GameObject LoginCanvas;

    private RaycastHit Hit;
    private Transform Implant;
    private Renderer Renderer;

	// Use this for initialization
	void Start () {
        Renderer = GetComponent<Renderer>();

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (LoginCanvas.activeSelf == false) { //after online/offline configuration
            Renderer.enabled = true;
            Transform cam = Camera.main.transform;
            //Debug.DrawRay(cam.position, cam.forward * 1000, Color.yellow);
            if (Physics.Raycast(cam.position, cam.forward, out Hit, 1000)) {
                if (Hit.transform.CompareTag("Implant")) {
                    Renderer.material.color = Color.green;
                    Renderer.material.mainTexture = CloseHand;
                    if (Input.GetMouseButton(0)) {
                        Renderer.enabled = false;
                        Implant = Hit.transform;
                        Implant.transform.tag = "CImp";
                        Implant.SetParent(this.transform.parent);
                        Implant.localPosition = new Vector3(0, 10, 120);
                        Invoke("EnableTransform", 1);
                    }
                }
            } else {
                Renderer.material.color = Color.white;
                Renderer.material.mainTexture = OpenHand;
            }
        }
    }

    void EnableTransform() {
        Implant.GetComponent<Transformer>().enabled = true;
        Catalog.SetActive(false);
        this.gameObject.SetActive(false);
    }


}
