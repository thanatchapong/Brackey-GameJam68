using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeCardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI detailsText;
    [SerializeField] private RawImage image;
    [SerializeField] Color riskColor;

    public void SetUpgrade(UpgradeObject upgrade, bool isRisk)
    {
        nameText.text = upgrade.upgradeName;
        detailsText.text = upgrade.upgradeDetails;
        if(isRisk) detailsText.color = riskColor;
        image.texture = upgrade.upgradeIcon;
    }
}
