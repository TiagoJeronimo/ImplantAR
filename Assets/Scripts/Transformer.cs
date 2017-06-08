// adapted from various online sources
// Gilles Ferrand, University of Manitoba 2016

using UnityEngine;
using CnControls;

public class Transformer : MonoBehaviour {

    //Scale
    private Vector3 InitialScale;

    // Screw
    public Transform NewParent;
    ScaleImageTarget ScaleImageTargetScript;
    bool ScrewFixed = false;

    public GameObject Joystick;
    public GameObject SpotLight;
    public GameObject Line;

    //Text
    public GameObject Angulation;

    private float RotX = 0.0f;
    private float RotY = 0.0f;
    private float MovX = 0.0f;
    private float MovY = 0.0f;
    private float MovZ = 0.0f;

    void Start() {
        InitialScale = transform.localScale;
        ScaleImageTargetScript = NewParent.GetComponent<ScaleImageTarget>();
        ScaleImageTargetScript.EnableScale();

        Line.transform.SetParent(this.transform);
        Line.transform.localPosition = Vector3.zero;
        Line.transform.localEulerAngles = new Vector3(0.0f,0.0f,90.0f);
        Line.SetActive(true);
    }

    void Update() {

        if (!ScrewFixed) {
            this.transform.LookAt(new Vector3(NewParent.position.x, this.transform.position.y, NewParent.position.z));
            //transform.rotation = Quaternion.LookRotation(NewParent.position);

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

            MovX = transform.localPosition.x;
            MovY = transform.localPosition.y;
            MovZ = transform.localPosition.z;

            Joystick.SetActive(true);

            // rotate 
            if (Input.GetMouseButton(0)) {
                
                // Angulation text
                Angulation.transform.position = this.transform.position;
                float angleX = transform.eulerAngles.x;
                angleX = (angleX > 180) ? angleX - 360 : angleX;
                float angleZ = transform.eulerAngles.z;
                angleZ = (angleZ > 180) ? angleZ - 360 : angleZ;
                Angulation.GetComponent<TextMesh>().text = "    X: " + angleX.ToString("F1") + " Z: " + angleZ.ToString("F1");

                //rotationAxisX = Camera.main.transform.up;
                //rotationAxisY = Camera.main.transform.right;

                /*this.transform.RotateAround(rotationCentre.transform.position, -rotationAxisX, CnInputManager.GetAxis("Horizontal"));
                this.transform.RotateAround(rotationCentre.transform.position, rotationAxisY, CnInputManager.GetAxis("Vertical"));*/

                RotX -= CnInputManager.GetAxis("Horizontal");
                RotY += CnInputManager.GetAxis("Vertical");
                transform.eulerAngles = new Vector3(RotY, 0, RotX);
            }

            if (Input.GetMouseButton(1)) {
                MovX -= CnInputManager.GetAxis("Horizontal");
                MovY -= CnInputManager.GetAxis("Vertical");
                transform.localPosition = new Vector3(MovX, MovY, transform.localPosition.z);
            }

            if (Input.GetMouseButton(2)) {
                MovZ -= CnInputManager.GetAxis("Vertical");
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, MovZ);
            }


        }
        
    }

}
