using UnityEngine;

public class RocketEffect : BonusEffect
{
    public override void Activate(GridManager grid, Tile tile)
    {
        grid.ActivateRocket(tile);
    }
}