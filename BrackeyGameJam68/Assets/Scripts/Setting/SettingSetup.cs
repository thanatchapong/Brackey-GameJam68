using UnityEngine;

public class SettingSetup : MonoBehaviour
{
    public GameObject Text_SFXPrefab;
    public GameObject MasterVolume_SliderPrefab;
    public GameObject Text_BGMPrefab;
    public GameObject BGM_inputPrefab;
    public GameObject MasterVolume_inputPrefab;
    public GameObject SFX_inputPrefab;
    public GameObject SFX_SliderPrefab;
    public GameObject BGM_SliderPrefab;
    public GameObject Text_MasterVolumePrefab;


    void Start()
    {
        Instantiate(Text_SFXPrefab);
        Instantiate(MasterVolume_SliderPrefab);
        Instantiate(Text_BGMPrefab);
        Instantiate(BGM_inputPrefab);
        Instantiate(MasterVolume_inputPrefab);
        Instantiate(SFX_inputPrefab);
        Instantiate(SFX_SliderPrefab);
        Instantiate(BGM_SliderPrefab);
        Instantiate(Text_MasterVolumePrefab);
    }
}
