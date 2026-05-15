using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject gamePanel;
    public GameObject gameOverPanel;
    public GameObject recordsPanel;
    public GameObject rulesPanel;
    public GameObject settingsPanel;
    public GameObject gameField;

    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalHighScoreText;
    public TextMeshProUGUI recordRow1Text;

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

    public void ShowMenu()
    {
        HideAllPanels();
        mainMenu.SetActive(true);
    }

    public void StartGame()
    {
        HideAllPanels();
        gamePanel.SetActive(true);
        gameField.SetActive(true);

        GameManager.instance.StartGame();
    }

    public void ShowRecords()
    {
        HideAllPanels();
        recordsPanel.SetActive(true);

        recordRow1Text.text =
            "1        Player        Lvl 1        " +
            GameManager.instance.scoreManager.HighScore;
    }

    public void ShowRules()
    {
        HideAllPanels();
        rulesPanel.SetActive(true);
    }

    public void ShowSettings()
    {
        HideAllPanels();
        settingsPanel.SetActive(true);
    }

    public void ShowGameOver()
    {
        HideAllPanels();
        gameOverPanel.SetActive(true);
        gameField.SetActive(true);

        finalScoreText.text = "Рахунок: " + GameManager.instance.scoreManager.Score;
        finalHighScoreText.text = "Найкращий результат: " + GameManager.instance.scoreManager.HighScore;
    }

    public void RestartGame()
    {
        StartGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}