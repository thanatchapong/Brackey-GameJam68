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
    public List<UpgradeObject> upgInUse = new List<UpgradeObject>();
    [SerializeField] List<Transform> card = new List<Transform>();

    public PlayableDirector openTl;
    public PlayableDirector closeTl;
    float timeUseUlt;
    public bool stopTime = false;

    void Update()
    {
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

        if (Input.GetKey(KeyCode.Space) && (currentExp >= requireExp))
        {
            timeUseUlt += Time.deltaTime;

            if (timeUseUlt >= 3)
            {
                stopTime = true;

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

            UpgradeObject upgObj = upgAvailable[randCard];

            card[i].GetChild(0).GetComponent<TMP_Text>().text = upgObj.upgradeName; //CardName
            card[i].GetChild(1).GetComponent<TMP_Text>().text = upgObj.upgradeDetails; //CardDetails
            card[i].GetChild(2).GetComponent<RawImage>().texture = upgObj.upgradeIcon; //CardImage
        }
    }

    public void Upg(int index)
    {
        stopTime = false;
    }
}
