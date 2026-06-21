using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class UIManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject gamePanel;
    public GameObject gameOverPanel;
    public GameObject recordsPanel;
    public GameObject rulesPanel;
    public GameObject settingsPanel;
    public GameObject gameField;

    public TextMeshProUGUI starsText; 
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalHighScoreText;
    public TextMeshProUGUI recordRow1Text;
    public Image star1;
    public Image star2;
    public Image star3;

    public float fadeDuration = 0.3f;

    private bool settingsOpenedFromGame = false;
    public void ShowSettingsFromGame()
    {
        settingsOpenedFromGame = true;
        ShowSettings();
    }
    private void Start()
    {
        ShowMenu();
    }

    private void HideAllPanels()
    {
        mainMenu.SetActive(false);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        recordsPanel.SetActive(false);
        rulesPanel.SetActive(false);
        settingsPanel.SetActive(false);
        gameField.SetActive(false);
    }
    void ShowPanelWithFade(GameObject panel)
    {
        panel.SetActive(true);
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 0f;
            StartCoroutine(FadeIn(cg));
        }
    }

    IEnumerator FadeIn(CanvasGroup cg)
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            yield return null;
        }
        cg.alpha = 1f;
    }

    public void ShowMenu()
    {
        SoundManager.instance.PlayButton();
        HideAllPanels();
        ShowPanelWithFade(mainMenu);
        if (GameManager.instance.gridManager != null)
            GameManager.instance.gridManager.ClearGrid();
    }

    public void StartGame()
    {
        SoundManager.instance.PlayButton();
        HideAllPanels();
        ShowPanelWithFade(gamePanel);
        gameField.SetActive(true);

        GameManager.instance.StartGame();
    }

    public void ShowRecords()
    {
        SoundManager.instance.PlayButton();
        HideAllPanels();
        ShowPanelWithFade(recordsPanel);

        recordRow1Text.text =
            "1        Player        Lvl 1        " +
            GameManager.instance.scoreManager.HighScore;
    }

    public void ShowRules()
    {
        SoundManager.instance.PlayButton();
        HideAllPanels();
        ShowPanelWithFade(rulesPanel);
    }

    public void ShowSettings()
    {
        SoundManager.instance.PlayButton();
        HideAllPanels();
        ShowPanelWithFade(settingsPanel);
    }
    public void BackFromSettings()
    {
        if (settingsOpenedFromGame)
        {
            settingsOpenedFromGame = false;
            ShowGame();
        }
        else
        {
            ShowMenu();
        }
    }

    public void ShowGameOver()
    {
        SoundManager.instance.PlayDefeat();
        HideAllPanels();
        ShowPanelWithFade(gameOverPanel);
        gameField.SetActive(true);

        resultText.text = "Поразка";
        finalScoreText.text = "Рахунок: " + GameManager.instance.scoreManager.Score;
        finalHighScoreText.text = "Найкращий результат: " + GameManager.instance.scoreManager.HighScore;

        SetStars(0); 
    }

    public void RestartGame()
    {
        SoundManager.instance.PlayButton();
        HideAllPanels();
        gamePanel.SetActive(true);
        gameField.SetActive(true);
        GameManager.instance.StartLevel(GameManager.instance.GetCurrentLevel());
    }

    public void ExitGame()
    {
        SoundManager.instance.PlayButton();
        Application.Quit();
    }
    public void ShowLevelCompleted()
    {
        SoundManager.instance.PlayVictory();
        HideAllPanels();

        ShowPanelWithFade(gameOverPanel);
        gameField.SetActive(true);

        resultText.text = "Перемога";

        finalScoreText.text = "Рахунок: " + GameManager.instance.scoreManager.Score;
        finalHighScoreText.text = "Найкращий результат: " + GameManager.instance.scoreManager.HighScore;

        int stars = GameManager.instance.CalculateStars();
        SetStars(stars);
    }

    void SetStars(int count)
    {
        star1.gameObject.SetActive(count >= 1);
        star2.gameObject.SetActive(count >= 2);
        star3.gameObject.SetActive(count >= 3);
    }
    string GetStarsString(int stars)
    {
        string result = "";
        for (int i = 0; i < 3; i++)
        {
            result += i < stars ? "★ " : "☆ ";
        }
        return result;
    }
    public void ShowGame()
    {
        SoundManager.instance.PlayButton();
        HideAllPanels();

        ShowPanelWithFade(gamePanel);
        gameField.SetActive(true);
    }
}