// adapted from various online sources
// Gilles Ferrand, University of Manitoba 2016

using UnityEngine;
using CnControls;

public class Transformer : MonoBehaviour {
	
	// rotate
	public float  rotSpeed = 4.0f;
	bool isRotating;
	Vector3 rotationAxisX;
	Vector3 rotationAxisY;
	Vector3 mouseOrigin;
	Vector3 angleDelta;
	GameObject rotationCentre;

	// Translate
	//public float panSpeed = 4.0f;
	//Vector3 translationAxis;

    //Scale
    private Vector3 InitialScale;

    // Screw
    public Transform NewParent;
    ScaleImageTarget ScaleImageTargetScript;
    bool ScrewFixed = false;

    public GameObject Joystick;
    public GameObject SpotLight;

    void Start() {
        InitialScale = transform.localScale;
        ScaleImageTargetScript = NewParent.GetComponent<ScaleImageTarget>();
        ScaleImageTargetScript.EnableScale();
    }

    void Update() {

        if (!ScrewFixed) {
            this.transform.LookAt(new Vector3(NewParent.position.x, this.transform.position.y, NewParent.position.z));

            SpotLight.transform.SetParent(this.transform);
            SpotLight.transform.localPosition =  Vector3.zero;
            SpotLight.SetActive(true);

            Vector3 maxScale = InitialScale * ScaleImageTargetScript.Scale;
            transform.localScale = Vector3.Lerp(InitialScale, maxScale, ScaleImageTargetScript.Norm);

            if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) {
                this.transform.SetParent(NewParent);
                ScrewFixed = true;
                Handheld.Vibrate();
                Debug.Log("Screw fixed");
            }
        } else if (ScrewFixed) {

            Joystick.SetActive(true);

            // rotate 
            if (Input.GetMouseButtonDown(0)) {
                isRotating = true;
                rotationCentre = this.gameObject;
                //mouseOrigin = Input.mousePosition;
            }

            if (isRotating) {
                rotationAxisX = Camera.main.transform.up;
                rotationAxisY = Camera.main.transform.right;
                /*angleDelta = (Input.mousePosition - mouseOrigin) / Screen.width;
                angleDelta *= rotSpeed;
                angleDelta.x *= -1;*/
                //this.transform.RotateAround(rotationCentre.transform.position, rotationAxisX, angleDelta.x);
                //this.transform.RotateAround(rotationCentre.transform.position, rotationAxisY, angleDelta.y);

                this.transform.RotateAround(rotationCentre.transform.position, -rotationAxisX, CnInputManager.GetAxis("Horizontal"));
                this.transform.RotateAround(rotationCentre.transform.position, rotationAxisY, CnInputManager.GetAxis("Vertical"));

                if (!Input.GetMouseButton(0)) isRotating = false;
            }

            // translate (not in use)
            /*if (Input.GetMouseButton(1)) {
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
            }*/
        }
    }
}
