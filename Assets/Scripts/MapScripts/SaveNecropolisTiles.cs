using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileController))]
public class SaveNecropolisTiles : MonoBehaviour
{
    List<GameObject> neighboorTiles = new List<GameObject>();
    public GameEvent deathEvent;
    public TileControllerSO controllerSO;

    private float oldSouls = -1;
    private bool hasChecked = false;

    private void OnDestroy()
    {
        neighboorTiles.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        // Это нужно для добавления в список тех тайлов, которые расставленны в редакторе
        if (!hasChecked)
            CheckForExistingCorruptedTiles();

        if (oldSouls < 0)
            oldSouls = controllerSO.stats.GetParameter("souls");

        float souls = controllerSO.stats.GetParameter("souls");

        if (souls < oldSouls)
        {
            UnsubscribeFromDeath();
        }

        oldSouls = souls;
    }

    private void CheckForExistingCorruptedTiles()
    {
        Collider2D[] neighboorColliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);

        TileController controllerToCheck = null;

        foreach (Collider2D collider in neighboorColliders)
        {

            controllerToCheck = collider.gameObject.GetComponent<TileController>();

            if (controllerToCheck != null && controllerToCheck.IsCorrupted())
                AddToNeighboors(collider.gameObject);

            //Debug.Log(corruptedTilesNear);
        }

        hasChecked = true;
    }

    // Obsolete
    private void AddToNeighboors()
    {
        Vector2 coor2d = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] neighboorColliders = Physics2D.OverlapCircleAll(coor2d, 0.5f);


        foreach (Collider2D collider in neighboorColliders)
        {
            if (!neighboorTiles.Contains(collider.gameObject))
                neighboorTiles.Add(collider.gameObject);
        }

        List<GameObject> nullTiles = new List<GameObject>();

        foreach (GameObject tile in neighboorTiles)
        {
            if (tile == null)
                nullTiles.Add(tile);
        }

        foreach(GameObject tile in nullTiles)
        {
            neighboorTiles.Remove(tile);
        }

        UnsubscribeFromDeath();
    }

    public void RemoveFromNeighboors(GameObject tile)
    {
        neighboorTiles.Remove(tile);
    }

    public void AddToNeighboors(GameObject tile)
    {
        neighboorTiles.Add(tile);
        UnsubscribeFromDeath();
    }

    /// <summary>
    /// Добавленные тайлы не уничтожаются при смерти
    /// </summary>
    private void UnsubscribeFromDeath()
    {
        float f = controllerSO.stats.GetParameter("tech6");

        if (f != 1)
            return;

        GameEventListener[] listeners = null;

        foreach (GameObject tile in neighboorTiles)
        {
            listeners = tile.GetComponents<GameEventListener>();

            foreach (GameEventListener listener in listeners)
            {
                if(listener.IsRegisteredTo(deathEvent))
                    deathEvent.UnregisterListener(listener);
            }            
        }
    }
}
