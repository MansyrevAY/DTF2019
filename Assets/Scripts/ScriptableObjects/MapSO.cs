using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapSO", menuName ="Scriptable objects/Map SO")]
public class MapSO : ScriptableObject
{
    Dictionary<Vector3, GameObject> map;

    public void RemoveTileFromMap(GameObject tile)
    {
        Vector3 coord3d = tile.transform.position;

        if (!map.ContainsKey(coord3d))
        {
            Debug.LogWarning("There is no tile at " + coord3d);
            return;
        }

        map.Remove(coord3d);
        Destroy(tile);
    }

    public void AddTileToMap(GameObject tile)
    {
        Vector3 coord3d = tile.transform.position;

        if (map.ContainsKey(coord3d))
        {
            Debug.LogWarning("There already is a tile at " + coord3d);
            return;
        }

        map.Add(coord3d, tile);
    }

    // TODO : переделать на координаты в карте
    public List<GameObject> GetNeighboorsOf(GameObject tile)
    {
        Vector2 coor2d = new Vector2(tile.transform.position.x, tile.transform.position.y);
        Collider2D[] neighboorColliders = Physics2D.OverlapCircleAll(coor2d, 0.5f);

        List<GameObject> neighboorTiles = new List<GameObject>();

        foreach (Collider2D collider in neighboorColliders)
        {
            neighboorTiles.Add(collider.gameObject);
        }

        return neighboorTiles;
    }
}
