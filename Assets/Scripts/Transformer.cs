using UnityEngine;
using CnControls;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Transformer : MonoBehaviour {

    //Scale
    private Vector3 InitialScale;

    // Screw
    public Transform ImplantNewParent;
    private bool ScrewFixed = false;

    public GameObject SpotLight;
    public GameObject Line;

    //Text
    public Text Angulation;

    public GameObject DetachButton;

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
    public static int AttachScrew;

    private Transform PreviousParent;

    public Slider SliderObj;
    public Canvas CanvasObj;

    public GameObject Joystick;
    public GameObject TwoWayJoystick;
    public GameObject RotationJoystick;

    public GameObject DoubleTouchButton;

    private int DirNumb;

    private bool DoubleTouch = false;

    private void Awake()
    {
        SendingRotation = Vector3.zero;
        SendingPosition = Vector3.zero;
    }

    void Start() {
        CanvasObj.enabled = true;
        InitialScale = transform.localScale;

       /* Line.transform.SetParent(this.transform);
        Line.transform.localPosition = Vector3.zero;
        Line.transform.localEulerAngles = new Vector3(0.0f,0.0f,90.0f);*/
        Line.SetActive(true);

        PreviousParent = this.transform.parent;
    }

    void Update() {
        if (!ScrewFixed) {

            AttachScrew = 0;
            DoubleTouchButton.SetActive(false);
            Joystick.SetActive(false);
            TwoWayJoystick.SetActive(false);
            RotationJoystick.SetActive(false);

            transform.LookAt(new Vector3(ImplantNewParent.transform.parent.position.x, this.transform.position.y, ImplantNewParent.transform.parent.position.z));
            ChangeScale();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!EventSystem.current.IsPointerOverGameObject(0)) {
                if (Physics.Raycast(ray, out hit)) {
                    if (Input.GetMouseButtonDown(0)) {
                        transform.position = hit.point;
                        DetachScrew();
                        DetachButton.GetComponent<ChangeSprite>().Change();
                    }
                }
            }
        } else if (ScrewFixed) {
            AttachScrew = 1;

            MovX = transform.localPosition.x;
            MovY = transform.localPosition.y;
            MovZ = transform.localPosition.z;

            RotX = transform.localEulerAngles.z;
            RotY = transform.localEulerAngles.x;
   
            TwoWayJoystick.SetActive(true);

            if (DoubleTouch) {
                RotationJoystick.SetActive(false);
                Joystick.SetActive(true);
            }
            else {
                Joystick.SetActive(false);
                RotationJoystick.SetActive(true);
            }
            var horizDiffAngle = Vector3.SignedAngle(transform.parent.transform.right, Camera.main.transform.right, Vector3.up);

            if (DoubleTouch && Input.GetMouseButton(0)) {
                MovY += CnInputManager.GetAxis("2DVertical");
                transform.localPosition = new Vector3(transform.localPosition.x, MovY, transform.localPosition.z);
            }
            //tranlation
            if (DoubleTouch && Input.GetMouseButton(0)) {

                float VertAxis = CnInputManager.GetAxis("Vertical");
                float HorAxis = CnInputManager.GetAxis("Horizontal");

                if (Mathf.Abs(VertAxis) > Mathf.Abs(HorAxis))
                {
                    HorAxis = 0;
                }
                else
                {
                    VertAxis = 0;
                }

                if (horizDiffAngle < 135 && horizDiffAngle > 45)
                {
                    MovX += VertAxis;
                    MovZ -= HorAxis;
                }
                else if (horizDiffAngle > -135 && horizDiffAngle < -45)
                {
                    MovX -= VertAxis;
                    MovZ += HorAxis;
                }
                else if ((horizDiffAngle <= 0 && horizDiffAngle >= -45) || (horizDiffAngle > 0 && horizDiffAngle <= 45))
                {
                    MovX += HorAxis;
                    MovZ += VertAxis;
                }
                else if ((horizDiffAngle >= -180 && horizDiffAngle <= -135) || (horizDiffAngle <= 180 && horizDiffAngle >= 135))
                {
                    MovX -= HorAxis;
                    MovZ -= VertAxis;
                }

                transform.localPosition = new Vector3(MovX, transform.localPosition.y, MovZ);
            }
            // rotate 
            else if (!DoubleTouch && Input.GetMouseButton(0)) {

                float VertAxis = CnInputManager.GetAxis("RotVertical");
                float HorAxis = CnInputManager.GetAxis("RotHorizontal");

                if(Mathf.Abs(VertAxis) > Mathf.Abs(HorAxis))
                {
                    HorAxis = 0;
                } else
                {
                    VertAxis = 0;
                }

                if (horizDiffAngle < 135 && horizDiffAngle > 45) {
                    RotX -= VertAxis;
                    RotY -= HorAxis;
                } else if (horizDiffAngle > -135 && horizDiffAngle < -45) {
                    RotX += VertAxis;
                    RotY += HorAxis;
                } else if ((horizDiffAngle <= 0 && horizDiffAngle >= -45) || (horizDiffAngle > 0 && horizDiffAngle <= 45)) {
                    RotX -= HorAxis;
                    RotY += VertAxis;
                } else if ((horizDiffAngle >= -180 && horizDiffAngle <= -135) || (horizDiffAngle <= 180 && horizDiffAngle >= 135)) {
                    RotX += HorAxis;
                    RotY -= VertAxis;
                }
				Debug.Log ("RotY: " + RotY);
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
        ScrewFixed = !ScrewFixed;

        if (ScrewFixed)
        {
            DoubleTouchButton.SetActive(true);
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
    }

    private void ChangeScale() {
        transform.localScale = new Vector3 (InitialScale.x + SliderObj.value*InitialScale.x, InitialScale.y+ SliderObj.value*InitialScale.y, InitialScale.z + SliderObj.value*InitialScale.z);
    }

    public void DTouchDown()
    {
        DoubleTouch = true;
    }

    public void DTouchUp()
    {
        DoubleTouch = false;
    }

    void OnGUI() {
        //GUI.Label(new Rect(10, 150, 1000, 20), "ScrewFixed: " + ScrewFixed);

        /*GUI.Label(new Rect(10, 30, 1000, 20), "Pos: " + this.transform.position);
        GUI.Label(new Rect(10, 50, 1000, 20), "localPos: " + this.transform.localPosition);
        GUI.Label(new Rect(10, 100, 1000, 20), "scale: " + (this.transform.lossyScale - this.transform.localScale));*/
        //GUI.Label(new Rect(10, 120, 1000, 20), "localscale: " + this.transform.localScale);
    }
}