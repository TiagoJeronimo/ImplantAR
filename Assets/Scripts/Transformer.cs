﻿// transforms an object: 
// - hold and drag primary mouse button to rotate the cube around its centre
// - hold and drag primary mouse button to rotate the cube around the camera (useful when inside the cube)
// - use arrow keys to translate the cube left/right and forward/backward
// - scroll wheel to scale the cube up/down
// adapted from various online sources
// Gilles Ferrand, University of Manitoba 2016

using UnityEngine;

public class Transformer : MonoBehaviour {
	
	// rotate
	public float  rotSpeed = 4.0f;
	bool isRotating;
	Vector3 rotationAxisX;
	Vector3 rotationAxisY;
	Vector3 mouseOrigin;
	Vector3 angleDelta;
	GameObject rotationCentre;

	// translate
	public float panSpeed = 4.0f;
	Vector3 translationAxis;

    // screw
    public Transform NewParent;
    bool ScrewFixed = false;

    void Update() {

        if (!ScrewFixed) {

            if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) {
                this.transform.SetParent(NewParent);
                ScrewFixed = true;
                Debug.Log("Screw fixed");
            }
        } else if (ScrewFixed) {

            // rotate 
            if (Input.GetMouseButtonDown(0)) {
                isRotating = true;
                rotationCentre = this.gameObject;
                mouseOrigin = Input.mousePosition;
            }

            if (isRotating) {
                rotationAxisX = Camera.main.transform.up;
                rotationAxisY = Camera.main.transform.right;
                angleDelta = (Input.mousePosition - mouseOrigin) / Screen.width;
                angleDelta *= rotSpeed;
                angleDelta.x *= -1;
                this.transform.RotateAround(rotationCentre.transform.position, rotationAxisX, angleDelta.x);
                this.transform.RotateAround(rotationCentre.transform.position, rotationAxisY, angleDelta.y);
                if (!Input.GetMouseButton(0)) isRotating = false;
            }

            // translate (not in use)
            if (Input.GetMouseButton(1)) {
                float distance;
                float rotX = Input.GetAxis("Mouse X");
                float rotY = Input.GetAxis("Mouse Y");

                #if UNITY_EDITOR || UNITY_STANDALONE

                    translationAxis = Camera.main.transform.right;
                    distance = panSpeed * rotX * Time.deltaTime;
                    this.transform.position += translationAxis * distance;

                    translationAxis = Camera.main.transform.up;
                    distance = panSpeed * rotY * Time.deltaTime;
                    this.transform.position += translationAxis * distance;

                #else

                    if (Input.touchCount > 0) {

                        rotX = Input.touches[0].deltaPosition.x;
                        rotY = Input.touches[0].deltaPosition.y;

                        translationAxis = Camera.main.transform.right;
                        distance = panSpeed * rotX * Time.deltaTime;
                        this.transform.position += translationAxis * distance;

                        translationAxis = Camera.main.transform.up;
                        distance = panSpeed * rotY * Time.deltaTime;
                        this.transform.position += translationAxis * distance;
                    }
                #endif
            }
        }
    }
}
