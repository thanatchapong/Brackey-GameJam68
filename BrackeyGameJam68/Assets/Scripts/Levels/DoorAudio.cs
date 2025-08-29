using UnityEngine;
using System.Collections.Generic;

public class DoorAudio : MonoBehaviour
{
    [SerializeField] List<AudioClip> EasyAudio = new List<AudioClip>();
    [SerializeField] List<AudioClip> HardAudio = new List<AudioClip>();

    public void PlayDoorSound(bool isHard)
    {
        if (isHard)
        {
            AudioManager.instance.PlaySound(HardAudio[Random.Range(0, HardAudio.Count)]);
        }
        else
        {
            AudioManager.instance.PlaySound(EasyAudio[Random.Range(0, EasyAudio.Count)]);
        }
    }
}
