using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeCardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI detailsText;
    [SerializeField] private RawImage image;

    public void SetUpgrade(UpgradeObject upgrade) {
        nameText.text = upgrade.upgradeName;
        detailsText.text = upgrade.upgradeDetails;
        image.texture = upgrade.upgradeIcon;
    }
}
