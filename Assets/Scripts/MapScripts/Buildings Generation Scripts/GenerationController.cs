using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationController : MonoBehaviour
{
    public GameObject grass;
    public TextAsset cityNamesAsset;

    public TileRuntimeSet grassTiles;

    [SerializeField]
    internal Building[] buildings;
    private string[] _cityNames;

    private void Awake()
    {
        _cityNames = cityNamesAsset.text.Split(',');
    }


    // Start is called before the first frame update
    void Start()
    {
        foreach (Building b in buildings)
        {
            Build(b);
        }
    }

    private void Build(Building building)
    {
        int randomIndex = -1;
        TileController buildTile;

        
        for (int i = 0; i < building.amount; i++)
        {
            randomIndex = Random.Range(0, grassTiles.Items.Count);
            buildTile = grassTiles.Items[randomIndex];

            buildTile.Build(building.buildingPrefab, GetRandomName());

            grassTiles.Remove(buildTile);
            //ClearCloseTiles(buildTile, building.noNeighboorsInXTiles);
        }

    }

    private string GetRandomName()
    {
        int index = Random.Range(0, _cityNames.Length);
        return _cityNames[index];
    }
    
}

[System.Serializable]
internal class Building
{
    public string name;
    public GameObject buildingPrefab;
    public int amount;
    public int noNeighboorsInXTiles;
}


