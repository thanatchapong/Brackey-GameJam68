using System.Collections.Generic;
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

    [SerializeField] Slider ultSlider;

    public PlayableDirector openTl;
    public PlayableDirector closeTl;
    float timeUseUlt;
    public bool stopTime = false;
    public bool isUpgrading = false;

    private UpgradeSystemAudio upgradeSystemAudio;

    void Start()
    {
        upgradeSystemAudio = gameObject.GetComponent<UpgradeSystemAudio>();
    }

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

                upgradeSystemAudio.PlayPerkAudio();

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

        upgradeSystemAudio.StopPerkAudio();
    }

}
