using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingUI : MonoBehaviour
{
    [Header("Sliders")]
    public Slider MasterVolume_Slider;
    public Slider BGM_Slider;
    public Slider SFX_Slider;

    [Header("Input Fields")]
    public TMP_InputField MasterVolume_input;
    public TMP_InputField BGM_input;
    public TMP_InputField SFX_input;

    void Start()
    {
        // โหลดค่าที่บันทึกไว้ (default = 100)
        MasterVolume_Slider.value = PlayerPrefs.GetFloat("MasterVolume", 100f);
        BGM_Slider.value = PlayerPrefs.GetFloat("BGMVolume", 100f);
        SFX_Slider.value = PlayerPrefs.GetFloat("SFXVolume", 100f);

        MasterVolume_input.text = MasterVolume_Slider.value.ToString("0") + "%";
        BGM_input.text = BGM_Slider.value.ToString("0") + "%";
        SFX_input.text = SFX_Slider.value.ToString("0") + "%";

        // Slider → InputField + Save
        MasterVolume_Slider.onValueChanged.AddListener((v) => { UpdateInputField(MasterVolume_input, v, "MasterVolume"); });
        BGM_Slider.onValueChanged.AddListener((v) => { UpdateInputField(BGM_input, v, "BGMVolume"); });
        SFX_Slider.onValueChanged.AddListener((v) => { UpdateInputField(SFX_input, v, "SFXVolume"); });

        // InputField → Slider + Save
        MasterVolume_input.onEndEdit.AddListener((v) => UpdateSliderFromInput(MasterVolume_Slider, MasterVolume_input, v, "MasterVolume"));
        BGM_input.onEndEdit.AddListener((v) => UpdateSliderFromInput(BGM_Slider, BGM_input, v, "BGMVolume"));
        SFX_input.onEndEdit.AddListener((v) => UpdateSliderFromInput(SFX_Slider, SFX_input, v, "SFXVolume"));
    }

    void UpdateInputField(TMP_InputField input, float value, string key)
    {
        input.text = value.ToString("0") + "%";
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save(); // บันทึกค่า
    }

    void UpdateSliderFromInput(Slider slider, TMP_InputField input, string value, string key)
    {
        string cleanValue = value.Replace("%", "");
        float val;
        if (float.TryParse(cleanValue, out val))
        {
            val = Mathf.Clamp(val, slider.minValue, slider.maxValue);
            slider.value = val;
            input.text = val.ToString("0") + "%";
            PlayerPrefs.SetFloat(key, val);
            PlayerPrefs.Save(); // บันทึกค่า
        }
        else
        {
            input.text = slider.value.ToString("0") + "%";
        }
    }
}