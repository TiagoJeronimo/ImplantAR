using UnityEngine;
using UnityEngine.UI;

public class ScaleImageTarget : MonoBehaviour
{
    public Slider slider;
    public Text SliderScaleText;
    public float InitialScale = 0.005f;

    // Update is called once per frame
    void Update()
    {
        ChangeScale();
    }

    private void ChangeScale() {
        transform.localScale = new Vector3 (InitialScale + slider.value*InitialScale, InitialScale+ slider.value*InitialScale, InitialScale + slider.value*InitialScale);
        SliderScaleText.text = "x" + (1 + slider.value).ToString("F1");
    }
}
