using UnityEngine;

public class BombEffect : BonusEffect
{
    public override void Activate(GridManager grid, Tile tile)
    {
        grid.ActivateBomb(tile);
    }
}