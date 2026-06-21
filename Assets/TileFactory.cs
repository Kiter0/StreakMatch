using UnityEngine;

public class TileFactory : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject destroyEffectPrefab;
    public Transform tileParent;

    public Color[] colors =
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.magenta
    };

    public Tile CreateTile(Vector2 position, TileType type)
    {
        GameObject tileObj = Instantiate(tilePrefab, position, Quaternion.identity, tileParent);

        Tile tile = tileObj.GetComponent<Tile>();
        tile.SetType(type, colors[(int)type]);
        tile.destroyEffectPrefab = destroyEffectPrefab;

        return tile;
    }

    public TileType GetRandomType()
    {
        int chance = Random.Range(1, 101); 

        if (chance <= 25)
            return TileType.Red;        

        if (chance <= 45)
            return TileType.Blue;       

        if (chance <= 65)
            return TileType.Green;      

        if (chance <= 85)
            return TileType.Yellow;     

        return TileType.Purple;         
    }
}