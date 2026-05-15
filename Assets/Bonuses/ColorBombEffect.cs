using UnityEngine;

public class ColorBombEffect : BonusEffect
{
    public override void Activate(GridManager grid, Tile tile)
    {
        grid.ActivateColorBomb(tile);
    }
}