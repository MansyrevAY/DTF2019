using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public AreaSO tileArea;
    public GameEvent tileClicked;

    public string cityName;
    public int population;
    public int defence;

    public int Population
    {
        get { return population; }
        set
        {
            population = value;
            if (population <= 0)
                population = 0;
            // put a city failure event here (?)
        }
    }

    public int Defence
    {
        get { return defence; }
        set
        {
            defence = value;
            if (defence <= 0)
                defence = 0;                
        }
    }

    private bool summonedArmyThisTurn = false;
    private SaveNecropolisTiles _saveNecropolisTiles;
    

    // Start is called before the first frame update
    void Start()
    {
        Population = tileArea.population;
        Defence = tileArea.defence;
    }

    public void ChooseAction(SaveNecropolisTiles saveNecropolisTiles)
    {
        tileClicked.Raise();
        _saveNecropolisTiles = saveNecropolisTiles;
    }

    // Возможные действия с тайлами
    #region Actions with tiles
    public void Corrupt(Stats stats)
    {
        TileAction corruptAction = GetActionWithName("Corrupt");

        float mana = stats.GetParameter("mana");
        if (mana - corruptAction.manaCost < 0)
            return;

        GameObject go = Instantiate(corruptAction.tileToReplace, transform.position, Quaternion.identity, transform.parent);

        SaveTileNearNecropolis(go);

        //Debug.Log(go.name);

        stats.ParametersChanger("mana - " + corruptAction.manaCost);
        stats.ParametersChanger("danger + 1");
        Destroy(gameObject);
    }    
    
    public void BuildCemetry(Stats stats)
    {
        if (tileArea.IsCorrupted)
        {
            TileAction corruptAction = GetActionWithName("Build cemetry");

            float mana = stats.GetParameter("mana");
            if (mana - corruptAction.manaCost < 0)
                return;

            GameObject go = Instantiate(corruptAction.tileToReplace, transform.position, Quaternion.identity, transform.parent);
            SaveTileNearNecropolis(go);

            stats.ParametersChanger("mana - " + corruptAction.manaCost);
            Destroy(gameObject);
        }
    }

    public void BuildZiggurat(Stats stats)
    {
        //Debug.Log("Building ziggurat");
        if (tileArea.IsCorrupted)
        {

            TileAction corruptAction = GetActionWithName("Build ziggurat");


            float souls = stats.GetParameter("souls");
            if (souls - corruptAction.soulCost < 0)
                return;

            GameObject go = Instantiate(corruptAction.tileToReplace, transform.position, Quaternion.identity, transform.parent);
            SaveTileNearNecropolis(go);

            stats.ParametersChanger("souls - " + corruptAction.soulCost);
            Destroy(gameObject);
        }
    }

    public void SummonArmy(Stats stats)
    {
        if (summonedArmyThisTurn)
            return;
        
        TileAction action = GetActionWithName("Summon army");

        float mana = stats.GetParameter("mana");
        if (mana - action.manaCost < 0)
            return;

        stats.ParametersChanger("mana - " + action.manaCost);
        stats.ParametersChanger("force + 1");
        summonedArmyThisTurn = true;
    }

    public void Demolish(Stats stats)
    {
        TileAction action = GetActionWithName("Demolish");

        _saveNecropolisTiles.RemoveFromNeighboors(gameObject);

        Instantiate(action.tileToReplace, transform.position, Quaternion.identity, transform.parent);
        Destroy(gameObject);
    }

    public void Weaken(Stats stats)
    {

    }

    public int GetRandomEventNumber()
    {
        int eventIndex = 0;

        if (tileArea.eventNumbers.Length > 1)
        {
            eventIndex = Random.Range(0, tileArea.eventNumbers.Length - 1);
        }

        return tileArea.eventNumbers[eventIndex];
    }


    public void SpreadDisease(Stats stats)
    {
        float percentToDamage = Random.Range(10, 33);
        float toDamage = percentToDamage / Population;
        int popToDamage = Mathf.RoundToInt(toDamage);


        float mana = stats.GetParameter("mana");
        if (mana - popToDamage < 0)
            return;

        //Debug.Log(popToDamage);
        Population -= popToDamage;

        stats.ParametersChanger("souls + " + popToDamage);
        stats.ParametersChanger("mana - " + popToDamage);

        if(Population <= 0)
        {
            DestroyVillage();
        }

    }

    public void Investigate()
    {
        //Debug.Log("Investigated");
        TileAction action = GetActionWithName("Investigate");

        Instantiate(action.tileToReplace, transform.position, Quaternion.identity, transform.parent);
        Destroy(gameObject);
    }

    public void CorruptSanctum(Stats stats)
    {
        TileAction action = GetActionWithName("Corrupt sanctum");

        float mana = stats.GetParameter("mana");
        if (mana - action.manaCost < 0)
            return;

        stats.ParametersChanger("souls - " + action.soulCost);
        Instantiate(action.tileToReplace, transform.position, Quaternion.identity, transform.parent);
        Destroy(gameObject);
    }

    public void Attack(Stats stats)
    {
        if( stats.GetParameter("force") >= tileArea.defence)
        {
            stats.ParametersChanger("force - " + tileArea.defence);
            stats.ParametersChanger("danger + " + Population);
            stats.ParametersChanger("souls + " + Population);
            DestroyVillage();
        }
    }

    private void DestroyVillage()
    {
        TileAction corruptAction = GetActionWithName("Destroy village");

        Instantiate(corruptAction.tileToReplace, transform.position, Quaternion.identity, transform.parent);
        Destroy(gameObject);
    }

    public void OnVillageReborn()
    {
        if (Population > 10)
        {
            TileAction action = GetActionWithName("Reborn village");

            Instantiate(action.tileToReplace, transform.position, Quaternion.identity, transform.parent);
            Destroy(gameObject);
        }
        else
            Population++;
    }

    public void OnCorruptedReborn()
    {
        TileAction action = GetActionWithName("Reborn corrupted");

        Instantiate(action.tileToReplace, transform.position, Quaternion.identity, transform.parent);
        Destroy(gameObject);
    }

    public void Build(GameObject building, string cityName)
    {
        GameObject go = Instantiate(building, transform.position, Quaternion.identity, transform.parent);

        TileController controller = go.GetComponent<TileController>();
        controller.cityName = cityName;

        Destroy(gameObject);
    }

    // В конце хода
    public void UnlockCemetry()
    {
        summonedArmyThisTurn = false;
    }

    public void ProduceMana(TileControllerSO SO)
    {
        Debug.Log("Mana");
        SO.stats.ParametersChanger("mana + 1");
    }

    private TileAction GetActionWithName(string name)
    {
        foreach (TileAction action in tileArea.PossibleActions)
        {
            if (action.name == name)
                return action;
        }

        return null;
    }

    private void SaveTileNearNecropolis(GameObject tile)
    {
        if (_saveNecropolisTiles == null) // This tile is not near the Necropolis
            return;

        _saveNecropolisTiles.RemoveFromNeighboors(gameObject);
        _saveNecropolisTiles.AddToNeighboors(tile);
    }
    #endregion //Actions with tiles

    public int GetTechs(string str)
    {
        foreach (TileAction action in tileArea.PossibleActions)
        {
            if (action.name == str)
                return action.techId;
        }

        return -1;
    }

    public int GetManaCost()
    {
        //return tileArea.manaCost;
        return -1;
    }

    public bool IsCorrupted()
    {
        return tileArea.IsCorrupted;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
