using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileController", menuName ="Scriptable objects/Tile ControllerSO")]
public class TileControllerSO : ScriptableObject
{
    public TileController controller;
    public Stats stats;
    public GameEvent death;
}
