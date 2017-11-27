using UnityEngine;
using CnControls;
using UnityEngine.UI;

public class Transformer : MonoBehaviour {

    //Scale
    private Vector3 InitialScale;

    // Screw
    public Transform ImplantNewParent;
    ScaleImageTarget ScaleImageTargetScript;
    private bool ScrewFixed = false;

    public GameObject Joystick;
    public GameObject SpotLight;
    public GameObject Line;

    //Text
    public Text Angulation;

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

    private Transform PreviousParent;

    public Slider SliderObj;
    public Canvas CanvasObj;

	public bool MoveOneDirection = true;
	private int DirNumb;
	private bool Getdir = true; 

    private void Awake()
    {
        SendingRotation = Vector3.zero;
        SendingPosition = Vector3.zero;
    }

    void Start() {
        CanvasObj.enabled = true;
        InitialScale = transform.localScale;
        ScaleImageTargetScript = ImplantNewParent.transform.parent.GetComponent<ScaleImageTarget>();

       /* Line.transform.SetParent(this.transform);
        Line.transform.localPosition = Vector3.zero;
        Line.transform.localEulerAngles = new Vector3(0.0f,0.0f,90.0f);*/
        Line.SetActive(true);

        PreviousParent = this.transform.parent;
    }

    void Update() {

        if (!ScrewFixed) {
            this.transform.LookAt(new Vector3(ImplantNewParent.transform.parent.position.x, this.transform.position.y, ImplantNewParent.transform.parent.position.z));
           
            /*SpotLight.transform.SetParent(this.transform);
            SpotLight.transform.localPosition =  Vector3.zero;
            SpotLight.SetActive(true);*/

            ChangeScale(); 

        } else if (ScrewFixed) {
            MovX = transform.localPosition.x;
            MovY = transform.localPosition.y;
            MovZ = transform.localPosition.z;

            RotX = transform.localEulerAngles.z;
            RotY = transform.localEulerAngles.x;

            Joystick.SetActive(true);

            var horizDiffAngle = Vector3.SignedAngle(transform.parent.transform.right, Camera.main.transform.right, Vector3.up);

            if (Input.GetMouseButton(2)) {
                MovY += CnInputManager.GetAxis("Vertical");
                transform.localPosition = new Vector3(transform.localPosition.x, MovY, transform.localPosition.z);
            } 
            else if (Input.GetMouseButton(1)) {
				
                if(horizDiffAngle < 135 && horizDiffAngle > 45) {
                    MovX += CnInputManager.GetAxis("Vertical");
                    MovZ -= CnInputManager.GetAxis("Horizontal");
                } else  if(horizDiffAngle > -135 && horizDiffAngle < -45) {
                    MovX -= CnInputManager.GetAxis("Vertical");
                    MovZ += CnInputManager.GetAxis("Horizontal");
                } else if((horizDiffAngle <= 0 && horizDiffAngle >= -45) || (horizDiffAngle >0  && horizDiffAngle <= 45)) {
                    MovX += CnInputManager.GetAxis("Horizontal");
                    MovZ += CnInputManager.GetAxis("Vertical");
                } else if ((horizDiffAngle >= -180 && horizDiffAngle <= -135) || (horizDiffAngle <= 180  && horizDiffAngle >= 135)) {
                    MovX -= CnInputManager.GetAxis("Horizontal");
                    MovZ -= CnInputManager.GetAxis("Vertical");
                }

                transform.localPosition = new Vector3(MovX, transform.localPosition.y, MovZ);
            }
            // rotate 
            else if (Input.GetMouseButton(0)) {

                if (horizDiffAngle < 135 && horizDiffAngle > 45) {
                    RotX -= CnInputManager.GetAxis("Vertical");
                    RotY -= CnInputManager.GetAxis("Horizontal");
                } else if (horizDiffAngle > -135 && horizDiffAngle < -45) {
                    RotX += CnInputManager.GetAxis("Vertical");
                    RotY += CnInputManager.GetAxis("Horizontal");
                } else if ((horizDiffAngle <= 0 && horizDiffAngle >= -45) || (horizDiffAngle > 0 && horizDiffAngle <= 45)) {
                    RotX -= CnInputManager.GetAxis("Horizontal");
                    RotY += CnInputManager.GetAxis("Vertical");
                } else if ((horizDiffAngle >= -180 && horizDiffAngle <= -135) || (horizDiffAngle <= 180 && horizDiffAngle >= 135)) {
                    RotX += CnInputManager.GetAxis("Horizontal");
                    RotY -= CnInputManager.GetAxis("Vertical");
                }
                transform.localEulerAngles = new Vector3 (RotY, 0, RotX);

                // Angulation text
                float angleX = transform.localEulerAngles.x;
                angleX = (angleX > 180) ? angleX - 360 : angleX;
                float angleZ = transform.localEulerAngles.z;
                angleZ = (angleZ > 180) ? angleZ - 360 : angleZ;
                Angulation.text = "    X: " + angleX.ToString("F1") + " Z: " + angleZ.ToString("F1");
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

            if(LastLocalRotation != this.transform.localEulerAngles) {
                LastLocalRotation = this.transform.localEulerAngles;
                SendingRotation = this.transform.localEulerAngles;
            } 
            else if(LastServerLocalRotation != Client.LocalRotation) { //the server change position
                this.transform.localEulerAngles = Client.LocalRotation;
                LastLocalRotation = this.transform.localEulerAngles;
                LastServerLocalRotation = Client.LocalRotation;
            }
        }
    }

    public void DetachScrew()
    {
        if (!ScrewFixed)
        {
            this.transform.SetParent(ImplantNewParent);
            this.transform.localEulerAngles = Vector3.zero;
            Handheld.Vibrate();
            Client.LocalPosition = this.transform.localPosition;
            LastServerLocalPosition = Client.LocalPosition;
            Client.LocalRotation = this.transform.localEulerAngles;
            LastServerLocalRotation = Client.LocalRotation;
        } else
        {
            Handheld.Vibrate();
            transform.SetParent(PreviousParent);
            transform.localPosition = new Vector3(0, 10, 120);
        }

        ScrewFixed = !ScrewFixed;
    }

    private void ChangeScale() {
        transform.localScale = new Vector3 (InitialScale.x + SliderObj.value*InitialScale.x, InitialScale.y+ SliderObj.value*InitialScale.y, InitialScale.z + SliderObj.value*InitialScale.z);
    }

    void OnGUI() {
		//GUI.Label(new Rect(10, 150, 1000, 20), "ScrewFixed: " + ScrewFixed);

        /*GUI.Label(new Rect(10, 30, 1000, 20), "Pos: " + this.transform.position);
        GUI.Label(new Rect(10, 50, 1000, 20), "localPos: " + this.transform.localPosition);
        GUI.Label(new Rect(10, 100, 1000, 20), "scale: " + (this.transform.lossyScale - this.transform.localScale));*/
        //GUI.Label(new Rect(10, 120, 1000, 20), "localscale: " + this.transform.localScale);
    }
}