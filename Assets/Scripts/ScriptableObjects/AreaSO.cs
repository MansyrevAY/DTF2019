using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AreaScriptableObject", menuName ="Scriptable objects/AreaSO")]
public class AreaSO : ScriptableObject
{
    public bool IsCorrupted = false;

    [SerializeField]
    public int population;

    public int defence;

    //public GameObject[] tilesToReplaceInTime;
    public TileAction[] PossibleActions;

    public int[] eventNumbers;

    public bool CanDoAction(string actionName)
    {
        foreach (TileAction action in PossibleActions)
        {
            if (action.name == actionName)
                return true;
        }

        return false;
    }
}

[System.Serializable]
public class TileAction
{
    [SerializeField]
    public string name;

    [SerializeField]
    public int manaCost;    

    [SerializeField]
    public int soulCost;

    [SerializeField]
    public int techId = -1;

    [SerializeField]
    public GameObject tileToReplace;
}
