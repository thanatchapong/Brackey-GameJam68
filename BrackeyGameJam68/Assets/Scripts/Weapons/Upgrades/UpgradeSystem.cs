using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine.Playables;

public class UpgradeSystem : MonoBehaviour
{
    [SerializeField] float requireExp = 100;
    [SerializeField] int currentExp = 0;
    [SerializeField] List<UpgradeObject> upgAvailable = new List<UpgradeObject>();
    [SerializeField] List<UpgradeObject> upgSelecting = new List<UpgradeObject>();
    public List<UpgradeObject> upgInUse = new List<UpgradeObject>();
    [SerializeField] List<Transform> card = new List<Transform>();
    [SerializeField] WeaponController weaponSys;

    [SerializeField] AudioClip PerkAudio;

    [SerializeField] Slider ultSlider;

    public PlayableDirector openTl;
    public PlayableDirector closeTl;
    float timeUseUlt;
    public bool stopTime = false;
    public bool isUpgrading = false;

    private Coroutine fadeCoroutine;

    void Update()
    {
        ultSlider.value = timeUseUlt;

        if (stopTime == true)
        {
            if (Time.timeScale > 0)
            {
                Time.timeScale -= Time.unscaledDeltaTime;
            }
        }
        else
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale += Time.unscaledDeltaTime;
            }
            else if (Time.timeScale >= 1)
            {
                Time.timeScale = 1;
            }
        }

        if (Input.GetKey(KeyCode.Space) && (currentExp >= requireExp) && isUpgrading == false)
        {
            timeUseUlt += Time.deltaTime;

            if (timeUseUlt >= 3)
            {
                timeUseUlt = 0;
                isUpgrading = true;

                stopTime = true;

                PlayPerkAudio();

                SetUpCard();

                openTl.Play();
                currentExp = 0;
                requireExp = Mathf.Round(requireExp * 1.25f);
            }
        }
        else
        {
            timeUseUlt -= Time.deltaTime * 1.5f;
            timeUseUlt = Mathf.Max(0, timeUseUlt);
        }
    }

    public void SetUpCard()
    {
        for (int i = 0; i < card.Count; i++)
        {
            int randCard = Random.Range(0, upgAvailable.Count);

            upgSelecting[i] = upgAvailable[randCard];

            card[i].GetChild(0).GetComponent<TMP_Text>().text = upgSelecting[i].upgradeName; //CardName
            card[i].GetChild(1).GetComponent<TMP_Text>().text = upgSelecting[i].upgradeDetails; //CardDetails
            card[i].GetChild(3).GetComponent<RawImage>().texture = upgSelecting[i].upgradeIcon; //CardImage

            card[i].GetComponent<Button>().enabled = true;
        }
    }

    public void Upg(int index)
    {
        stopTime = false;

        upgInUse.Add(upgSelecting[index]);

        AudioManager.instance.PlaySound(upgSelecting[index].audio);

        weaponSys.getUpgraded();

        isUpgrading = false;

        StopPerkAudio();
    }

    //Play ticking sound effect and adjust BGM when selecting perks
    void PlayPerkAudio()
    {
        AudioManager.instance.PlayLoop(PerkAudio);

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(MusicFade(1f, 0.0250f, 0.01f, 1f, 0.8f));
    }

    void StopPerkAudio()
    {
        AudioManager.instance.StopLoop();
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(MusicFade(1f, 0.01f, 0.0250f, 0.8f, 1f));
    }

    private IEnumerator MusicFade(float duration, float volstart, float voltarget, float pitchstart, float pitchtarget)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += 0.0025f;
            float newVolume = Mathf.Lerp(volstart, voltarget, timeElapsed / duration);
            float newPitch = Mathf.Lerp(pitchstart, pitchtarget, timeElapsed / duration);
            AudioManager.instance.BGM.volume = newVolume;
            AudioManager.instance.BGM.pitch = newPitch;
            yield return null;
        }

        AudioManager.instance.BGM.volume = voltarget;
        AudioManager.instance.BGM.pitch = pitchtarget;
    }

}
