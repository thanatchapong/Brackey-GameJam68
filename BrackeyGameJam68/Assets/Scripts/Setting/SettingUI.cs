using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

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

    [Header("Audio Mixer")]
    public AudioMixer Mixer; // ลาก AudioMixer เข้า Inspector

    void Start()
    {
        // โหลดค่า PlayerPrefs (default = 100)
        if (MasterVolume_Slider != null)
            MasterVolume_Slider.value = PlayerPrefs.GetFloat("Master", 100f);
        if (BGM_Slider != null)
            BGM_Slider.value = PlayerPrefs.GetFloat("BGM", 100f);
        if (SFX_Slider != null)
            SFX_Slider.value = PlayerPrefs.GetFloat("SFX", 100f);

        if (MasterVolume_input != null)
            MasterVolume_input.text = MasterVolume_Slider.value.ToString("0") + "%";
        if (BGM_input != null)
            BGM_input.text = BGM_Slider.value.ToString("0") + "%";
        if (SFX_input != null)
            SFX_input.text = SFX_Slider.value.ToString("0") + "%";

        // อัปเดต Mixer ตามค่าเริ่มต้น
        if (MasterVolume_input != null && MasterVolume_Slider != null && Mixer != null)
            UpdateInputAndMixer(MasterVolume_input, MasterVolume_Slider.value, "Master");
        if (BGM_input != null && BGM_Slider != null && Mixer != null)
            UpdateInputAndMixer(BGM_input, BGM_Slider.value, "BGM");
        if (SFX_input != null && SFX_Slider != null && Mixer != null)
            UpdateInputAndMixer(SFX_input, SFX_Slider.value, "SFX");

        // Slider → InputField + Mixer + Save
        if (MasterVolume_Slider != null)
            MasterVolume_Slider.onValueChanged.AddListener((v) => {UpdateInputAndMixer(MasterVolume_input, v, "Master"); ClampBGMAndSFXToMaster();});
        if (BGM_Slider != null)
            BGM_Slider.onValueChanged.AddListener((v) => UpdateInputAndMixer(BGM_input, v, "BGM"));
        if (SFX_Slider != null)
            SFX_Slider.onValueChanged.AddListener((v) => UpdateInputAndMixer(SFX_input, v, "SFX"));

        // InputField → Slider + Mixer + Save
        if (MasterVolume_input != null)
            MasterVolume_input.onEndEdit.AddListener((v) => UpdateSliderFromInput(MasterVolume_Slider, MasterVolume_input, v, "Master"));
        if (BGM_input != null)
            BGM_input.onEndEdit.AddListener((v) => UpdateSliderFromInput(BGM_Slider, BGM_input, v, "BGM"));
        if (SFX_input != null)
            SFX_input.onEndEdit.AddListener((v) => UpdateSliderFromInput(SFX_Slider, SFX_input, v, "SFX"));
    }
    void ClampBGMAndSFXToMaster()
    {
        float master = MasterVolume_Slider.value;

        if (BGM_Slider.value > master) BGM_Slider.value = master;

        if (SFX_Slider.value > master) SFX_Slider.value = master;
    }
    // อัปเดต InputField + Mixer + Save
    void UpdateInputAndMixer(TMP_InputField input, float value, string key)
    {
        if (input != null)
            input.text = value.ToString("0") + "%";

        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();

        if (Audio_Setting.Instance != null)
            Audio_Setting.Instance.SetVolume(key, value);
    }

    // แปลง % → dB (100% = 0 dB, 200% = ~6 dB, 0% ~ mute)
    float PercentToDB(float percent)
    {
        if (percent <= 0f) return -80f; 
        float normalized = percent / 100f; 
        float db;
        if (normalized <= 1f)
        {
            db = Mathf.Lerp(-40f, 0f, Mathf.Log10(1 + 9 * normalized));
        }
        else
        {
            db = Mathf.Lerp(0f, 6f, normalized - 1f);
        }
        return db;
    }

    // อัปเดต Slider จาก InputField
    void UpdateSliderFromInput(Slider slider, TMP_InputField input, string value, string key)
    {
        string cleanValue = value.Replace("%", "");
        float val;
        if (float.TryParse(cleanValue, out val))
        {
            val = Mathf.Clamp(val, slider.minValue, slider.maxValue);
            if (slider != null)
                slider.value = val;
            UpdateInputAndMixer(input, val, key);
        }
        else
        {
            if (slider != null)
                input.text = slider.value.ToString("0") + "%";
        }
    }
}