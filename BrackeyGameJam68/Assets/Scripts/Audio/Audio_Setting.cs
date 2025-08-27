using UnityEngine;
using UnityEngine.Audio;

public class Audio_Setting : MonoBehaviour
{
    public static Audio_Setting Instance;

    [Header("Audio Mixer")]
    public AudioMixer Mixer; // ใส่ Mixer ของคุณใน Inspector

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // อยู่ตลอดเกม
        }
        else
        {
            Destroy(gameObject); // กันซ้ำ
        }
    }

    public void SetVolume(string key, float percent)
    {
        float db = PercentToDB(percent);
        if (!Mixer.SetFloat(key, db))
        {
            Debug.LogWarning($"Parameter {key} not found in AudioMixer!");
        }
        else
        {
            PlayerPrefs.SetFloat(key, percent);
            PlayerPrefs.Save();
            Debug.Log($"Set {key} = {percent}% ({db} dB)");
        }
    }

    private float PercentToDB(float percent)
    {
        if (percent <= 0f) return -80f;
        float normalized = percent / 100f;
        float db;
        if (normalized <= 1f)
            db = Mathf.Lerp(-40f, 0f, Mathf.Log10(1 + 9 * normalized));
        else
            db = Mathf.Lerp(0f, 6f, normalized - 1f);
        return db;
    }
}


