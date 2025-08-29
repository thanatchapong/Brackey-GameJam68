using System.Collections.Generic; using UnityEngine; using UnityEngine.UI; using TMPro; using UnityEngine.Playables;

public class UpgradeSystem : MonoBehaviour
{
    [SerializeField] float requireExp = 100;
    [SerializeField] int currentExp = 0;
    [SerializeField] List<UpgradeObject> upgAvailable = new List<UpgradeObject>();
    [SerializeField] List<UpgradeObject> riskAvailable = new List<UpgradeObject>();
    [SerializeField] List<UpgradeObject> upgSelecting = new List<UpgradeObject>();
    public List<UpgradeObject> upgInUse = new List<UpgradeObject>();
    [SerializeField] List<Transform> card = new List<Transform>();
    [SerializeField] WeaponController weaponSys;
    [SerializeField] PlayerHP plrHp;

    [SerializeField] Slider ultBar;
    [SerializeField] Slider ultSlider;
    [SerializeField] TMP_Text expText;

    [SerializeField] Texture forcedImage;
    [SerializeField] Color perkCol;
    [SerializeField] Color riskCol;

    public PlayableDirector openTl;
    public PlayableDirector closeTl;
    float timeUseUlt;
    public bool stopTime = false;
    public bool isUpgrading = false;

    private UpgradeSystemAudio upgradeSystemAudio;
    private bool MaxExpSoundPlayed = false;

    // 🔥 new risk tracking
    private int riskCount = 1; // always start with 1 risk card
    private bool forceRisk = false;

    void Start()
    {
        upgradeSystemAudio = gameObject.GetComponent<UpgradeSystemAudio>();
        ultBar.maxValue = requireExp;

        ultSlider.maxValue = 1.5f;
    }

    void Update()
    {
        ultSlider.value = timeUseUlt;
        ultBar.value = currentExp;

        //Play SFX when max exp is reached
        if (currentExp >= requireExp && !MaxExpSoundPlayed)
        {
            MaxExpSoundPlayed = true;
            upgradeSystemAudio.PlayMaxExpSound();
        }

        expText.text = currentExp + "/" + requireExp;

        if (stopTime && isUpgrading)
        {
            if (Time.timeScale > 0)
                Time.timeScale -= Time.unscaledDeltaTime;
        }
        else if (!stopTime && isUpgrading)
        {
            if (Time.timeScale < 1)
                Time.timeScale += Time.unscaledDeltaTime;
            else if (Time.timeScale >= 1)
            {
                isUpgrading = false;
                Time.timeScale = 1;
            }
        }

        if (Input.GetKey(KeyCode.Space) && (currentExp >= requireExp) && !isUpgrading)
        {
            timeUseUlt += Time.deltaTime;

            if (timeUseUlt >= 1.5f)
            {
                timeUseUlt = 0;
                isUpgrading = true;
                stopTime = true;

                upgradeSystemAudio.PlayPerkAudio(); //slow down BGM and play ticking sound
                SetUpCard();

                openTl.Play();
                currentExp = 0;
                requireExp = Mathf.Round(requireExp * 1.25f);
                ultBar.maxValue = requireExp;
            }
        }
        else
        {
            timeUseUlt -= Time.deltaTime * 1.25f;
            timeUseUlt = Mathf.Max(0, timeUseUlt);
        }
    }

    // ===========================
    // Setup Upgrade & Risk Cards
    // ===========================
    public void SetUpCard()
    {
        upgSelecting.Clear();

        // if forced, only give 1 random risk
        if (forceRisk)
        {
            UpgradeObject forced = riskAvailable[Random.Range(0, riskAvailable.Count)];
            upgSelecting.Add(forced);

            // fill rest with placeholders disabled
            for (int i = 0; i < card.Count; i++)
            {
                if (i == 0)
                {
                    AssignCardUI(card[i], forced);
                    card[i].GetComponent<Button>().enabled = true;
                }
                else
                {
                    card[i].GetChild(0).GetComponent<TMP_Text>().text = "---";
                    card[i].GetChild(1).GetComponent<TMP_Text>().text = "Forced Risk";
                    card[i].GetChild(1).GetComponent<TMP_Text>().color = riskCol;

                    card[i].GetChild(3).GetComponent<RawImage>().texture = forcedImage;
                    card[i].GetComponent<Button>().enabled = false;
                }
            }

            return;
        }

        // normal setup with riskCount
        List<int> usedIndexes = new List<int>();

        for (int i = 0; i < card.Count; i++)
        {
            UpgradeObject chosen;

            if (i < riskCount) // inject risk first
                chosen = riskAvailable[Random.Range(0, riskAvailable.Count)];
            else
                chosen = upgAvailable[Random.Range(0, upgAvailable.Count)];

            upgSelecting.Add(chosen);
            AssignCardUI(card[i], chosen);
            card[i].GetComponent<Button>().enabled = true;
        }
    }

    private void AssignCardUI(Transform cardTransform, UpgradeObject obj)
    {
        cardTransform.GetChild(0).GetComponent<TMP_Text>().text = obj.upgradeName;
        cardTransform.GetChild(1).GetComponent<TMP_Text>().text = obj.upgradeDetails;
        
        if(obj.isRisk)cardTransform.GetChild(1).GetComponent<TMP_Text>().color = riskCol;
        else cardTransform.GetChild(1).GetComponent<TMP_Text>().color = perkCol;

        cardTransform.GetChild(3).GetComponent<RawImage>().texture = obj.upgradeIcon;
    }

    // ===========================
    // Handle Player Choice
    // ===========================
    public void Upg(int index)
    {
        stopTime = false;

        MaxExpSoundPlayed = false;

        UpgradeObject chosen = upgSelecting[index];
        upgInUse.Add(chosen);

        AudioManager.instance.PlaySound(chosen.audio);
        weaponSys.getUpgraded();
        plrHp.GetUpgrade();
        upgradeSystemAudio.StopPerkAudio();

        // check if chosen is risk or normal
        if (riskAvailable.Contains(chosen))
        {
            // picked a risk → reset to 1
            riskCount = 1;
            forceRisk = false;
        }
        else
        {
            // picked normal → increase riskCount
            riskCount++;

            if (riskCount >= 3)
            {
                forceRisk = true; // next time force risk
                riskCount = 1;    // reset after force
            }
        }
    }

    public void GetExp(int amount)
    {
        currentExp += Random.Range(amount, amount + 6);
    }
}
