using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileController))]
public class AddToTileRuntimeSet : MonoBehaviour
{
    public TileRuntimeSet set;

    private void OnEnable()
    {
        set.Add(gameObject.GetComponent<TileController>());
    }

    private void OnDisable()
    {
        set.Remove(gameObject.GetComponent<TileController>());
    }
}
