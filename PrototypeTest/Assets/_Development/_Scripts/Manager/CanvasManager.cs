using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : Singleton<CanvasManager>
{
    [SerializeField] CanvasGroup tutorialGroup;
    [SerializeField] TextMeshProUGUI currencyTMP;
    [SerializeField] CanvasGroup upgradesGroup;
    [SerializeField] CanvasGroup gameoverGroup;
    [SerializeField] CanvasGroup levelcompleteGroup;
    [SerializeField] TextMeshProUGUI unitTMP;
    [SerializeField] TextMeshProUGUI bonusTMP;
    [SerializeField] TextMeshProUGUI coinEarnedTMP;
    [SerializeField] TextMeshProUGUI currentLevelTMP;

    [SerializeField] int unitPrice = 100;
    [SerializeField] int bonusPrice = 100;
    [SerializeField] float handMaxPos = 280;
    [SerializeField] float handMoveDuration = 1;
    [SerializeField] Image handTutorial;

    GameManager game;

    private void Start()
    {
        game = GameManager.Instance;
        handTutorial.rectTransform.DOAnchorPos(new(handMaxPos, handTutorial.rectTransform.localPosition.y), handMoveDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    public void SetCurrency(int value)
    {
        currencyTMP.text = value.ToString();
    }

    public void HideTutorial()
    {
        ToggleGroup(tutorialGroup, false);
    }

    public static void ToggleGroup(CanvasGroup group, bool value)
    {
        float fade = value ? 1 : 0;
        group.DOFade(fade, .25f);
        group.interactable = value;
        group.blocksRaycasts = value;
    }

    public void SetUpgradeLevel()
    {
        int unit = GameManager.Instance.upgradeUnit + 1;
        int bonus = GameManager.Instance.upgradeBonus + 1;
        unitTMP.text = "LEVEL " + unit;
        bonusTMP.text = "LEVEL " + bonus;
        currentLevelTMP.text = "LEVEL " + (game.level + 1);
    }

    public void HideUpgrades()
    {
        ToggleGroup(upgradesGroup, false);
    }

    public void UpgradeUnit()
    {
        if (game.currency < unitPrice) return;

        game.UpgradeUnit(unitPrice);
        unitPrice += unitPrice;
        SetUpgradeLevel();
        PlayerManager.Instance.Spawn(1);
    }

    public void UpgradeBonus()
    {
        if (game.currency < bonusPrice) return;

        game.UpgradeBonus(bonusPrice);
        bonusPrice += bonusPrice;
        SetUpgradeLevel();
    }

    public void GameOver()
    {
        ToggleGroup(gameoverGroup, true);
    }

    public void TryAgain()
    {
        GameManager.Instance.RestartLevel(true);
    }

    public void LevelComplete()
    {
        ToggleGroup(levelcompleteGroup, true);

        int reward = PlayerManager.Instance.mobList.Count * (game.level + 1) + game.upgradeBonus + 1;
        coinEarnedTMP.text = reward.ToString();
        GameManager.Instance.LevelComplete(reward);
    }

    public void NextLevel()
    {
        GameManager.Instance.RestartLevel();
    }
}
