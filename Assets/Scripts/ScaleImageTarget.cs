using UnityEngine;

public class ScaleImageTarget : MonoBehaviour
{

    public float MinDistance;
    public float MaxDistance;
    public float Scale;

    [HideInInspector]
    public float Norm;

    private bool ScaleOn = false;
    private Vector3 InitialScale;
    private float CurrentDistance;

    private float DisFactor;

    // Use this for initialization
    void Start()
    {
        InitialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        if (ScaleOn)
        {
            var distance = (transform.position - Camera.main.transform.position).magnitude;
            DisFactor = distance / 5;
            Norm = (distance - MinDistance) / (MaxDistance - MinDistance);
            Norm = Mathf.Clamp01(Norm);

            transform.localScale = Vector3.Lerp(InitialScale, InitialScale * Scale, Norm);
        }
    }

    public void EnableScale()
    {
        ScaleOn = true;
    }

    public void DisableScale()
    {
        ScaleOn = false;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 150, 1000, 20), "Norm: " + Norm);
    }
}
