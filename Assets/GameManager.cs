using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GridManager gridManager;
    public ScoreManager scoreManager;
    public UIManager uiManager;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI movesText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI levelText;
    public bool isGameActive = false;
    public int moves = 20;
    public LevelData[] levels;

    private int currentLevel = 0;
    private Tile firstTile;
    private Tile secondTile;

    private void Awake()
    {
        instance = this;
    }
    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void SelectTile(Tile tile)
    {
        if (!isGameActive) return;

        gridManager.ResetIdleTimer();

        BonusEffect effect = null;

        switch (tile.bonusType)
        {
            case BonusType.ColorBomb:
                effect = new ColorBombEffect();
                break;

            case BonusType.RocketHorizontal:
            case BonusType.RocketVertical:
                effect = new RocketEffect();
                break;

            case BonusType.Bomb:
                effect = new BombEffect();
                break;
        }

        if (effect != null)
        {
            effect.Activate(gridManager, tile);
            return;
        }

        if (firstTile == null)
        {
            firstTile = tile;
            firstTile.Select();
        }
        else if (secondTile == null && tile != firstTile)
        {
            secondTile = tile;
            secondTile.Select();

            if (AreAdjacent(firstTile, secondTile))
            {
                StartCoroutine(SwapTiles());
            }
            else
            {
                ResetSelection();
            }
        }
    }

    IEnumerator SwapTiles()
    {
        if (!isGameActive) yield break;
        if (firstTile == null || secondTile == null) yield break;

        StartCombo();

        Vector3 firstPos = firstTile.transform.position;
        Vector3 secondPos = secondTile.transform.position;

        StartCoroutine(firstTile.MoveTo(secondPos, 0.25f));
        StartCoroutine(secondTile.MoveTo(firstPos, 0.25f));

        yield return new WaitForSeconds(0.25f);

        if (firstTile == null || secondTile == null) yield break;
        if (gridManager == null) yield break;

        for (int x = 0; x < gridManager.width; x++)
        {
            for (int y = 0; y < gridManager.height; y++)
            {
                if (gridManager.grid[x, y] == firstTile)
                    gridManager.grid[x, y] = secondTile;
                else if (gridManager.grid[x, y] == secondTile)
                    gridManager.grid[x, y] = firstTile;
            }
        }

        gridManager.CheckMatches();

        moves--;
        UpdateUI();

        if (moves <= 0)
        {
            yield return new WaitForSeconds(0.8f);
            isGameActive = false;

            if (scoreManager.Score >= levels[currentLevel].targetScore)
                LevelCompleted(); // победа — показываем звёзды
            else
                uiManager.ShowGameOver(); // поражение — цель не достигнута
        }

        ResetSelection();
    }

    void ResetSelection()
    {
        if (firstTile != null)
            firstTile.ResetScale();

        if (secondTile != null)
            secondTile.ResetScale();

        firstTile = null;
        secondTile = null;
    }

    bool AreAdjacent(Tile a, Tile b)
    {
        Vector2 posA = a.transform.position;
        Vector2 posB = b.transform.position;

        float dx = Mathf.Abs(posA.x - posB.x);
        float dy = Mathf.Abs(posA.y - posB.y);

        return (dx < 1.5f && dy < 0.1f) || (dx < 0.1f && dy < 1.5f);
    }
    public void StartCombo()
    {
        scoreManager.StartCombo();
    }
    public void StartGame()
    {
        StartLevel(0);
    }
    void UpdateUI()
    {
        scoreText.text = "Score: " + scoreManager.Score;
        comboText.text = "Combo: x" + scoreManager.Combo;
        movesText.text = "Moves: " + moves;
        highScoreText.text = "High Score: " + scoreManager.HighScore;
        targetText.text = "Target: " + levels[currentLevel].targetScore;
        levelText.text = "Рівень: " + levels[currentLevel].levelNumber;
    }
    public void LevelCompleted()
    {
        isGameActive = false;

        uiManager.ShowLevelCompleted();
    }
    public void StartLevel(int levelIndex)
    {
        if (levelIndex >= levels.Length)
        {
            Debug.LogError("Уровень " + levelIndex + " не существует!");
            return;
        }

        currentLevel = levelIndex;

        if (gridManager != null)
            gridManager.ResetGrid();
        else
            Debug.LogError("GridManager не назначен!");

        moves = levels[levelIndex].moves;

        scoreManager.ResetScore();

        firstTile = null;
        secondTile = null;

        isGameActive = true;

        UpdateUI();

        Debug.Log("Запущено рівень " + (levelIndex + 1));
    }
    public void NextLevel()
    {
        currentLevel++;
        if (currentLevel < levels.Length)
        {
            uiManager.ShowGame();   
            StartLevel(currentLevel); 
        }
    }
    public int CalculateStars()
    {
        int target = levels[currentLevel].targetScore;
        int score = scoreManager.Score;

        if (score >= target * 1.5f) return 3;
        if (score >= target * 1.2f) return 2;
        if (score >= target) return 1;
        return 0;
    }

}