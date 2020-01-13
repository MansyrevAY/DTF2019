using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Actions : MonoBehaviour
{
    public Camera mainCamera;
    public Stats stats;
    public TileControllerSO tileControllerSO;

    private RaycastHit2D[] hit;
    private Vector3 mousepos;
    private Vector2 mousePos2D;
    private GameObject objToCorrupt;
    private Collider2D[] neighboorColliders;

    // Start is called before the first frame update
    void Start()
    {
        tileControllerSO.stats = stats;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickAtTile();
        }
    }

    private void ClickAtTile()
    {
        if (UIClicked())
            return;

        
        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos2D = new Vector2(mousepos.x, mousepos.y);

        hit = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hit.Length == 0)
            return;

        objToCorrupt = null;

        //Debug.Log(hit.Length + " объектов задело");


        SetObjectToCorrupt();
        

        if (objToCorrupt == null)
        {
            Debug.LogWarning("Клик не попал ни во что");
            return;
        }

        InteractWithTile();
    }

    /// <summary>
    /// Проверка на попадание в интерфейс
    /// </summary>
    /// <returns>true, если попали в интерфейс</returns>
    private bool UIClicked()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (RaycastResult res in results)
        {
            if (res.gameObject.layer == 5)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Проверка на многослойные тайлы
    /// </summary>
    private void SetObjectToCorrupt()
    {
        objToCorrupt = hit[0].transform.gameObject;

        if (hit.Length > 1)
        {
            foreach (RaycastHit2D hit2D in hit)
            {
                if (hit2D.transform.tag != "Plains")
                {
                    objToCorrupt = hit2D.transform.gameObject;
                }
            }
        }
    }

    /// <summary>
    /// Запуск выбора действия с тайлом
    /// </summary>
    private void InteractWithTile()
    {
        TileController controller = objToCorrupt.GetComponent<TileController>();

        tileControllerSO.controller = controller;

        if (controller != null && CanCorrupt(objToCorrupt))
        {
            //Debug.Log(CanCorrupt(objToCorrupt));
            controller.ChooseAction(IsNearNecropolis());
        }

        else
            tileControllerSO.controller = null;
    }

    /// <summary>
    /// Проверка на наличие зараженных соседних тайлов
    /// </summary>
    /// <param name="inPoint">В этой точке на расстоянии в 1 тайл</param>
    /// <returns>true, если рядом есть зараженные тайлы</returns>
    private bool CanCorrupt(GameObject inPoint)
    {
        neighboorColliders = Physics2D.OverlapCircleAll(inPoint.transform.position, 0.5f);

        bool corruptedTilesNear = false;
        TileController controllerToCheck = null;

        foreach (Collider2D collider in neighboorColliders)
        {

            controllerToCheck = collider.gameObject.GetComponent<TileController>();

            if (controllerToCheck != null && controllerToCheck.IsCorrupted())
                corruptedTilesNear = true;

            //Debug.Log(corruptedTilesNear);
        }

        Debug.Log(corruptedTilesNear);
        return corruptedTilesNear;
    }

    private SaveNecropolisTiles IsNearNecropolis()
    {
        SaveNecropolisTiles saver = null;
        foreach (Collider2D collider in neighboorColliders)
        {
            if (collider.gameObject.name == "Necropolis")
                saver = collider.gameObject.GetComponent<SaveNecropolisTiles>();
        }

        return saver;
    }
}
