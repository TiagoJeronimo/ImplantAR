using UnityEngine;
using CnControls;

public class Transformer : MonoBehaviour {

    //Scale
    private Vector3 InitialScale;

    // Screw
    public Transform ImplantNewParent;
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

    //Client information
    private Vector3 LastLocalPosition;
    private Vector3 LastServerLocalPosition;

    private Vector3 LastLocalRotation;
    private Vector3 LastServerLocalRotation;

	public static Vector3 SendingRotation;
    public static Vector3 SendingPosition;

    void Start() {
        InitialScale = transform.localScale;
        ScaleImageTargetScript = ImplantNewParent.transform.parent.GetComponent<ScaleImageTarget>();
        ScaleImageTargetScript.EnableScale();

        Line.transform.SetParent(this.transform);
        Line.transform.localPosition = Vector3.zero;
        Line.transform.localEulerAngles = new Vector3(0.0f,0.0f,90.0f);
        Line.SetActive(true);
    }

    void Update() {

        if (!ScrewFixed) {
            this.transform.LookAt(new Vector3(ImplantNewParent.transform.parent.position.x, this.transform.position.y, ImplantNewParent.transform.parent.position.z));
            //transform.rotation = Quaternion.LookRotation(ImplantNewParent.transform.parent.position);

            SpotLight.transform.SetParent(this.transform);
            SpotLight.transform.localPosition =  Vector3.zero;
            SpotLight.SetActive(true);

            Vector3 maxScale = InitialScale * ScaleImageTargetScript.Scale;
            transform.localScale = Vector3.Lerp(InitialScale, maxScale, ScaleImageTargetScript.Norm);

            if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) {
                this.transform.SetParent(ImplantNewParent);
                this.transform.localEulerAngles = Vector3.zero;
                ScrewFixed = true;
                Handheld.Vibrate();
                Debug.Log("Screw fixed");
                Client.LocalPosition = this.transform.localPosition;
                LastServerLocalPosition = Client.LocalPosition;
                Client.LocalRotation = this.transform.localEulerAngles;
                LastServerLocalRotation= Client.LocalRotation;
            }

        } else if (ScrewFixed) {  
            MovX = transform.localPosition.x;
            MovY = transform.localPosition.y;
            MovZ = transform.localPosition.z;

            Joystick.SetActive(true);

            if (Input.GetMouseButton(2)) {
                MovY += CnInputManager.GetAxis("Vertical");
                transform.localPosition = new Vector3(transform.localPosition.x, MovY, transform.localPosition.z);
            } 
            else if (Input.GetMouseButton(1)) {
                MovX -= CnInputManager.GetAxis("Horizontal");
                MovZ -= CnInputManager.GetAxis("Vertical");
                transform.localPosition = new Vector3(MovX, transform.localPosition.y, MovZ);
            }
            // rotate 
            else if (Input.GetMouseButton(0)) {

                RotX += CnInputManager.GetAxis("Horizontal");
                RotY -= CnInputManager.GetAxis("Vertical");
				transform.localEulerAngles = new Vector3 (RotY, 0, RotX);

                // Angulation text
                Angulation.transform.position = this.transform.position;
                float angleX = transform.localEulerAngles.x;
                angleX = (angleX > 180) ? angleX - 360 : angleX;
                float angleZ = transform.localEulerAngles.z;
                angleZ = (angleZ > 180) ? angleZ - 360 : angleZ;
                Angulation.GetComponent<TextMesh>().text = "    X: " + angleX.ToString("F1") + " Z: " + angleZ.ToString("F1");
            }

            if(LastLocalPosition != this.transform.localPosition) { //this side(Client) changed position
                LastLocalPosition = this.transform.localPosition;
                SendingPosition = this.transform.localPosition;
            } 
            else if(LastServerLocalPosition != Client.LocalPosition) { //the server change position
                this.transform.localPosition = Client.LocalPosition;
                LastLocalPosition = Client.LocalPosition;
                LastServerLocalPosition = Client.LocalPosition;
            }

            //depois de passar o primeiro if ele vai ao segundo else, pois o Client.localPosition não é alterado

            if(LastLocalRotation != this.transform.localEulerAngles) {
                LastLocalRotation = this.transform.localEulerAngles;
                SendingRotation = this.transform.localEulerAngles;
            } 
            else if(LastServerLocalRotation != Client.LocalRotation) { //the server change position
                this.transform.localEulerAngles = Client.LocalRotation;
                LastLocalRotation = Client.LocalRotation;
                LastServerLocalRotation = Client.LocalPosition;
            }
        }  
    }

	void OnGUI() {
		/*GUI.Label(new Rect(10, 10, 1000, 20), "localRot: " + this.transform.localEulerAngles);
        GUI.Label(new Rect(10, 30, 1000, 20), "Pos: " + this.transform.position);
        GUI.Label(new Rect(10, 50, 1000, 20), "localPos: " + this.transform.localPosition);
        GUI.Label(new Rect(10, 100, 1000, 20), "scale: " + (this.transform.lossyScale - this.transform.localScale));*/
        //GUI.Label(new Rect(10, 120, 1000, 20), "localscale: " + this.transform.localScale);
    }
}