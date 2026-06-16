using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 8;
    public int height = 8;

    public Tile[,] grid;
    public TileFactory tileFactory;
    public GameObject tilePrefab;
    

    float offset = 1f;

    Color[] colors = new Color[]
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.magenta
    };

    void Start()
    {
        grid = new Tile[width, height];
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 position = new Vector2(
                    x * offset - width / 2f,
                    y * offset - height / 2f
                );

                TileType randomType;

                do
                {
                    randomType = tileFactory.GetRandomType();
                }
                while (CreatesMatch(x, y, randomType));

                Tile tile = tileFactory.CreateTile(position, randomType);
                grid[x, y] = tile;
            }
        }
    }

    public void CheckMatches()
    {
        bool[,] matched = new bool[width, height];
        bool foundMatch = false;

        // горизонтальні
        for (int y = 0; y < height; y++)
        {
            int x = 0;

            while (x < width)
            {
                Tile current = grid[x, y];

                if (!IsNormalTile(current))
                {
                    x++;
                    continue;
                }

                int count = 1;

                while (x + count < width &&
                        IsNormalTile(grid[x + count, y]) &&
                        grid[x + count, y].type == current.type)
                {
                    count++;
                }

                if (count >= 5)
                {
                    current.SetBonus(BonusType.ColorBomb);

                    for (int i = 1; i < count; i++)
                        matched[x + i, y] = true;

                    foundMatch = true;
                }
                else if (count == 4)
                {
                    current.SetBonus(BonusType.RocketHorizontal);

                    for (int i = 1; i < count; i++)
                        matched[x + i, y] = true;

                    foundMatch = true;
                }
                else if (count == 3)
                {
                    for (int i = 0; i < count; i++)
                        matched[x + i, y] = true;

                    foundMatch = true;
                }

                x += count;
            }
        }

        // вертикальні
        for (int x = 0; x < width; x++)
        {
            int y = 0;

            while (y < height)
            {
                Tile current = grid[x, y];

                if (!IsNormalTile(current))
                {
                    y++;
                    continue;
                }

                int count = 1;

                while (y + count < height &&
                        IsNormalTile(grid[x, y + count]) &&
                        grid[x, y + count].type == current.type)
                {
                    count++;
                }

                if (count >= 5)
                {
                    current.SetBonus(BonusType.ColorBomb);

                    for (int i = 1; i < count; i++)
                        matched[x, y + i] = true;

                    foundMatch = true;
                }
                else if (count == 4)
                {
                    current.SetBonus(BonusType.RocketVertical);

                    for (int i = 1; i < count; i++)
                        matched[x, y + i] = true;

                    foundMatch = true;
                }
                else if (count == 3)
                {
                    for (int i = 0; i < count; i++)
                        matched[x, y + i] = true;

                    foundMatch = true;
                }

                y += count;
            }
        }
        CheckShapeBombs(matched);

        // знищення позначених плиток
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (matched[x, y] && grid[x, y] != null)
                {
                    grid[x, y].DestroyWithAnimation();
                    grid[x, y] = null;

                    GameManager.instance.scoreManager.AddScore(10);
                }
            }
        }

        if (foundMatch)
        {
            GameManager.instance.scoreManager.IncreaseCombo();

            Invoke(nameof(DropTiles), 0.35f);
            Invoke(nameof(CheckMatches), 0.55f);
        }
        else
        {
            GameManager.instance.scoreManager.ResetCombo();
        }
    }

    bool CreatesMatch(int x, int y, TileType type)
    {
        // перевірка вліво
        if (x >= 2)
        {
            if (grid[x - 1, y] != null && grid[x - 2, y] != null)
            {
                if (grid[x - 1, y].type == type &&
                    grid[x - 2, y].type == type)
                {
                    return true;
                }
            }
        }

        // перевірка вниз
        if (y >= 2)
        {
            if (grid[x, y - 1] != null && grid[x, y - 2] != null)
            {
                if (grid[x, y - 1].type == type &&
                    grid[x, y - 2].type == type)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void DropTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null)
                {
                    for (int k = y + 1; k < height; k++)
                    {
                        if (grid[x, k] != null)
                        {
                            grid[x, y] = grid[x, k];
                            grid[x, k] = null;

                            Vector2 targetPosition = new Vector2(
                                x * offset - width / 2f,
                                y * offset - height / 2f
                            );

                            StartCoroutine(grid[x, y].MoveTo(targetPosition, 0.25f));

                            break;
                        }
                    }
                }
            }
        }

        FillEmpty();
    }

    void FillEmpty()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null)
                {
                    Vector2 position = new Vector2(
                        x * offset - width / 2f,
                        y * offset - height / 2f
                    );

                    TileType randomType = tileFactory.GetRandomType();
                    Tile tile = tileFactory.CreateTile(position, randomType);

                    grid[x, y] = tile;
                    StartCoroutine(tile.Bounce());

                    CountPossibleMoves();
                }
            }
        }
    }
    void CreateRocket(Tile tile, bool horizontal)
    {
        if (tile == null)
            return;

        if (horizontal)
            tile.SetBonus(BonusType.RocketHorizontal);
        else
            tile.SetBonus(BonusType.RocketVertical);
    }
    public void ActivateRocket(Tile rocket)
    {
        if (rocket == null)
            return;

        int rocketX = -1;
        int rocketY = -1;

        // координати ракети
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == rocket)
                {
                    rocketX = x;
                    rocketY = y;
                    break;
                }
            }
        }

        if (rocketX == -1 || rocketY == -1)
            return;

        // горизонтальна ракета
        if (rocket.bonusType == BonusType.RocketHorizontal)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, rocketY] != null)
                {
                    grid[x, rocketY].DestroyWithAnimation();
                    grid[x, rocketY] = null;

                    GameManager.instance.scoreManager.AddScore(20);
                }
            }
        }

        // вертикальна ракета
        if (rocket.bonusType == BonusType.RocketVertical)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[rocketX, y] != null)
                {
                    grid[rocketX, y].DestroyWithAnimation();
                    grid[rocketX, y] = null;

                    GameManager.instance.scoreManager.AddScore(20);
                }
            }
        }

        Invoke(nameof(DropTiles), 0.35f);
        Invoke(nameof(CheckMatches), 0.55f);
    }
    public void ActivateBomb(Tile bomb)
    {
        if (bomb == null)
            return;

        int bombX = -1;
        int bombY = -1;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == bomb)
                {
                    bombX = x;
                    bombY = y;
                    break;
                }
            }
        }

        if (bombX == -1 || bombY == -1)
            return;

        for (int x = bombX - 1; x <= bombX + 1; x++)
        {
            for (int y = bombY - 1; y <= bombY + 1; y++)
            {
                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    if (grid[x, y] != null)
                    {
                        grid[x, y].DestroyWithAnimation();
                        grid[x, y] = null;

                        GameManager.instance.scoreManager.AddScore(30);
                    }
                }
            }
        }

        Invoke(nameof(DropTiles), 0.35f);
        Invoke(nameof(CheckMatches), 0.55f);
    }
    bool IsNormalTile(Tile tile)
    {
        return tile != null && tile.bonusType == BonusType.None;
    }
    public void ActivateColorBomb(Tile colorBomb)
    {
        if (colorBomb == null)
            return;

        TileType targetType = colorBomb.type;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] != null && grid[x, y].type == targetType)
                {
                    grid[x, y].DestroyWithAnimation();
                    grid[x, y] = null;

                    GameManager.instance.scoreManager.AddScore(15);
                }
            }
        }

        colorBomb.DestroyWithAnimation();

        Invoke(nameof(DropTiles), 0.35f);
        Invoke(nameof(CheckMatches), 0.55f);
    }
    void CheckShapeBombs(bool[,] matched)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile center = grid[x, y];

                if (center == null || center.bonusType != BonusType.None)
                    continue;

                int horizontalCount = 1;
                int verticalCount = 1;

                // вліво
                for (int i = x - 1; i >= 0; i--)
                {
                    if (grid[i, y] != null && grid[i, y].type == center.type)
                        horizontalCount++;
                    else
                        break;
                }

                // вправо
                for (int i = x + 1; i < width; i++)
                {
                    if (grid[i, y] != null && grid[i, y].type == center.type)
                        horizontalCount++;
                    else
                        break;
                }

                // вниз
                for (int j = y - 1; j >= 0; j--)
                {
                    if (grid[x, j] != null && grid[x, j].type == center.type)
                        verticalCount++;
                    else
                        break;
                }

                // вверх
                for (int j = y + 1; j < height; j++)
                {
                    if (grid[x, j] != null && grid[x, j].type == center.type)
                        verticalCount++;
                    else
                        break;
                }

                if (horizontalCount >= 3 && verticalCount >= 3)
                {
                    center.SetBonus(BonusType.Bomb);

                    //  горизонталь
                    for (int i = x - 1; i >= 0; i--)
                    {
                        if (grid[i, y] != null && grid[i, y].type == center.type)
                            matched[i, y] = true;
                        else
                            break;
                    }

                    for (int i = x + 1; i < width; i++)
                    {
                        if (grid[i, y] != null && grid[i, y].type == center.type)
                            matched[i, y] = true;
                        else
                            break;
                    }

                    //  вертикаль
                    for (int j = y - 1; j >= 0; j--)
                    {
                        if (grid[x, j] != null && grid[x, j].type == center.type)
                            matched[x, j] = true;
                        else
                            break;
                    }

                    for (int j = y + 1; j < height; j++)
                    {
                        if (grid[x, j] != null && grid[x, j].type == center.type)
                            matched[x, j] = true;
                        else
                            break;
                    }

                    matched[x, y] = false;
                }
            }
        }
    }
    public int CountPossibleMoves()
    {
        int possibleMoves = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null)
                    continue;

                // перевірка вправо
                if (x < width - 1)
                {
                    SwapInArray(x, y, x + 1, y);

                    if (HasMatchAt(x, y) || HasMatchAt(x + 1, y))
                        possibleMoves++;

                    SwapInArray(x, y, x + 1, y);
                }

                // перевірка вверх
                if (y < height - 1)
                {
                    SwapInArray(x, y, x, y + 1);

                    if (HasMatchAt(x, y) || HasMatchAt(x, y + 1))
                        possibleMoves++;

                    SwapInArray(x, y, x, y + 1);
                }
            }
        }

        Debug.Log("Possible moves: " + possibleMoves);
        return possibleMoves;
    }
    void SwapInArray(int x1, int y1, int x2, int y2)
    {
        Tile temp = grid[x1, y1];
        grid[x1, y1] = grid[x2, y2];
        grid[x2, y2] = temp;
    }
    bool HasMatchAt(int x, int y)
    {
        Tile tile = grid[x, y];

        if (tile == null)
            return false;

        TileType type = tile.type;

        int horizontalCount = 1;

        for (int i = x - 1; i >= 0; i--)
        {
            if (grid[i, y] != null && grid[i, y].type == type)
                horizontalCount++;
            else
                break;
        }

        for (int i = x + 1; i < width; i++)
        {
            if (grid[i, y] != null && grid[i, y].type == type)
                horizontalCount++;
            else
                break;
        }

        int verticalCount = 1;

        for (int j = y - 1; j >= 0; j--)
        {
            if (grid[x, j] != null && grid[x, j].type == type)
                verticalCount++;
            else
                break;
        }

        for (int j = y + 1; j < height; j++)
        {
            if (grid[x, j] != null && grid[x, j].type == type)
                verticalCount++;
            else
                break;
        }

        return horizontalCount >= 3 || verticalCount >= 3;
    }
    public void ClearGrid()
    {
        foreach (Transform child in tileFactory.tileParent)
        {
            DestroyImmediate(child.gameObject);
        }

        grid = new Tile[width, height];
    }
    public void ResetGrid()
    {
        ClearGrid();
        GenerateGrid();
    }
}