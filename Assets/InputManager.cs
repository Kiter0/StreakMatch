using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public GameManager gameManager;

    private void Awake()
    {
        instance = this;
    }

    public void HandleTileClick(Tile tile)
    {
        if (tile == null || gameManager == null)
            return;

        gameManager.SelectTile(tile);
    }
}