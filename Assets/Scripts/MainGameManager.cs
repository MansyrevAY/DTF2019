using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{

    public GameObject decisionObject;
    public GameObject upperPanel;
    public GameEvent nextTurn;

    private DecisionManager decisionManager; //управление системой решений
    ParametersShower parametersShower;
    Stats stats;
    TechsManager techsManager;

    public DecisionManager DecisionManager { get => decisionManager; }

    // Start is called before the first frame update
    void Start()
    {
        decisionManager = decisionObject.GetComponent<DecisionManager>();
        parametersShower = upperPanel.GetComponent<ParametersShower>();
        techsManager = gameObject.GetComponent<TechsManager>();
        stats = gameObject.GetComponent<Stats>();
        stats.Initialize();
        decisionManager.Initialize();
        parametersShower.Initialize();
        techsManager.Initialize();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Reincarnation()
    {
        stats.ParametersChanger("rebirth + 1");

        //ивенты
        if (stats.GetParameter("rebirth") == 2)
        {
            decisionManager.Decision2Pack("6", true);
        }
        if (stats.GetParameter("rebirth") == 5)
        {
            decisionManager.Decision2Pack("7", true);
        }

        //мана
        if (stats.GetParameter("tech8") == 1)
        {
            stats.ParametersManualChanger("mana", "=", 10.0f);
        } else
        {
            stats.ParametersManualChanger("mana", "=", 5.0f);
        }

        //армия
        if (stats.GetParameter("tech11") == 1)
        {
            stats.ParametersManualChanger("force", "=", 10.0f);
        }
        else
        {
            stats.ParametersManualChanger("force", "=", 5.0f);
        }

        //угроза
        stats.ParametersManualChanger("danger", "=", 0);
    }

    public void EndTurn()
    {
        if (decisionManager.IsRemainingDecisions() == false) //проверка, нет ли неотвеченных ивентов
        {
            Debug.Log("Начался новый ход");
            stats.ParametersChanger("turn + 1");
            stats.ParametersChanger("mana + 1");
            nextTurn.Raise();
            if (stats.GetParameter("danger") > 10)
            {
                decisionManager.Decision2Pack("20", false);
            }
        } else { Debug.Log("Остались ивенты, требующие решения"); }
    }
}
