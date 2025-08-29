using UnityEngine;
using System.Collections.Generic;

public class UpgradeListUI : MonoBehaviour
{
    [SerializeField] private UpgradeSystem upgradeSystem;
    [SerializeField] private GameObject cardInventory;
    [SerializeField] private Transform gridBox;
    [SerializeField] private GameObject cardPrefab;

    public bool isActive = false;

    private List<GameObject> upgradeCards;

    void Awake() {
        upgradeCards = new List<GameObject>();
        cardInventory.SetActive(false);
    }

    void Clear()
    {
        foreach (GameObject card in upgradeCards)
        {
            Destroy(card);
        }

        upgradeCards = new List<GameObject>();

        for(int i = 0; i < upgradeSystem.upgInUse.Count; i++) {
            GameObject card = Instantiate(cardPrefab, gridBox);
            UpgradeCardUI cardUI = card.GetComponent<UpgradeCardUI>();
            cardUI.SetUpgrade(upgradeSystem.upgInUse[i], upgradeSystem.upgInUse[i].isRisk);
            upgradeCards.Add(card);
        }
    }

    public void ToggleMenu()
    {
        if(isActive) CloseMenu();
        else OpenMenu();
    }

    void OpenMenu() {
        Clear();
        cardInventory.SetActive(true);
        isActive = true;
    }

    void CloseMenu() {
        cardInventory.SetActive(false);
        isActive = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(isActive) CloseMenu();
            else OpenMenu();
        }
    }
}
