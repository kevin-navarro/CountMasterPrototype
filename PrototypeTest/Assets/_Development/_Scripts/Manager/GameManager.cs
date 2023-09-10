using UnityEngine;
using GameAnalyticsSDK;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    public int level { get; private set; }
    public int currency { get; private set; }
    public int upgradeUnit { get; private set; }
    public int upgradeBonus { get; private set; }

    [SerializeField] GameObject[] levels;
    public bool levelComplete { get; private set; }

    void Start()
    {
        LoadSave();
        LoadSettings();
    }

    void LoadSettings()
    {
        GameAnalytics.Initialize();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level " + level + " started");
        CanvasManager.Instance.SetCurrency(currency);
        CanvasManager.Instance.SetUpgradeLevel();
        PlayerManager.Instance.LoadMobs();

        levels[level].SetActive(true);
    }

    void LoadSave()
    {
        upgradeUnit = PlayerPrefs.GetInt("unit");
        upgradeBonus = PlayerPrefs.GetInt("bonus");
        currency = PlayerPrefs.GetInt("currency");
        level = PlayerPrefs.GetInt("level");
    }

    public void UpgradeUnit(int value)
    {
        SetCurrency(value);
        upgradeUnit += 1;
        PlayerPrefs.SetInt("unit", upgradeUnit);
    }

    public void UpgradeBonus(int value)
    {
        SetCurrency(value);
        upgradeBonus += 1;
        PlayerPrefs.SetInt("bonus", upgradeBonus);
    }

    public void SetCurrency(int value, bool reward = false)
    {
        if (!reward)
            currency -= value;
        else currency += value;
        CanvasManager.Instance.SetCurrency(currency);
        PlayerPrefs.SetInt("currency", currency);
    }

    public void LevelComplete(int reward)
    {
        SetCurrency(reward, true);
        levelComplete = true;
        if (level < 2) level += 1;
        else level = 0;
        PlayerPrefs.SetInt("level", level);
    }

    public void RestartLevel(bool failed = false)
    {
        DOTween.KillAll();
        if (failed)
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "level " + level + " failed");
        else
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level " + level + " completed");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}