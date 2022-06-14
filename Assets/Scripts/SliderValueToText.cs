using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderValueToText : MonoBehaviour
{
    public Slider sliderUI;
    private Text textSliderValue;

    void Start()
    {
        textSliderValue = GetComponent<Text>();
        ShowSliderValue();
    }

    private void Update()
    {
        ShowSliderValue();
    }

    public void ShowSliderValue()
    {
        string sliderMessage = "" + sliderUI.value.ToString("0.##");
        textSliderValue.text = sliderMessage;
    }
}