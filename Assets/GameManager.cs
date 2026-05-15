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
    public bool isGameActive = false;
    public int moves = 20;


    private Tile firstTile;
    private Tile secondTile;

    private void Awake()
    {
        instance = this;
    }

    public void SelectTile(Tile tile)
    {
        if (!isGameActive)
            return;

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

        StartCombo();

        Vector3 firstPos = firstTile.transform.position;
        Vector3 secondPos = secondTile.transform.position;

        StartCoroutine(firstTile.MoveTo(secondPos, 0.25f));
        StartCoroutine(secondTile.MoveTo(firstPos, 0.25f));

        yield return new WaitForSeconds(0.25f);

        GridManager gm = gridManager;

        for (int x = 0; x < gm.width; x++)
        {
            for (int y = 0; y < gm.height; y++)
            {
                if (gm.grid[x, y] == firstTile)
                    gm.grid[x, y] = secondTile;
                else if (gm.grid[x, y] == secondTile)
                    gm.grid[x, y] = firstTile;
            }
        }

        gridManager.CheckMatches();

        moves--;
        UpdateUI();

        if (moves <= 0)
        {
            isGameActive = false;
            uiManager.ShowGameOver();
        }

        ResetSelection();
    }

    void ResetSelection()
    {
        firstTile.ResetScale();
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

        return (dx < 1.2f && dy < 0.1f) || (dx < 0.1f && dy < 1.2f);
    }
    public void StartCombo()
    {
        scoreManager.StartCombo();
    }
    public void StartGame()
    {
        scoreManager.ResetScore();
        moves = 20;
        isGameActive = true;

        UpdateUI();

        Debug.Log("Game Started");
    }
    void UpdateUI()
    {
        scoreText.text = "Score: " + scoreManager.Score;
        comboText.text = "Combo: x" + scoreManager.Combo;
        movesText.text = "Moves: " + moves;
        highScoreText.text = "High Score: " + scoreManager.HighScore;
    }
}