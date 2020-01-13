using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileActionsPanelScript : MonoBehaviour
{
    public Image image;
    public TileControllerSO controllerSO;

    public TextMeshProUGUI defenceText;
    public TextMeshProUGUI populationText;
    public TextMeshProUGUI cityName;

    public GameObject cityPanel;
    public DecisionManager decisionManager;

    private List<GameObject> childrenToEnable;

    // Start is called before the first frame update
    void Start()
    {
        childrenToEnable = new List<GameObject>();

        foreach (Transform t in transform)
        {
            childrenToEnable.Add(t.gameObject);
        }

        Transform tr = cityPanel.transform.Find("CityActionsPanel");

        foreach (Transform t in tr)
        {
            childrenToEnable.Add(t.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTileClicked()
    {
        DisableMenu();
        MakeVisible();
    }

    private void MakeVisible()
    {
        ButtonScript script = null;

        foreach (GameObject button in childrenToEnable)
        {
            script = button.GetComponent<ButtonScript>();

            int techIdRequired = controllerSO.controller.GetTechs(script.actionName);
            bool isOpen = isTechOpen(techIdRequired);
            Debug.Log(script.actionName + " " + isOpen);


            if (controllerSO.controller.tileArea.CanDoAction(script.actionName) && isOpen)
            {
                if (button.name == "CityPanel")
                    OnCityPanelOpened();
                button.SetActive(true);
            }
        }
    }

    public void OnCorruptClicked()
    {
        if (controllerSO.controller == null)
        {
            DisableMenu();
            return;
        }

        controllerSO.controller.Corrupt(controllerSO.stats);
        DisableMenu();
    }

    public void OnCemetryClicked()
    {
        if (controllerSO.controller == null)
        {
            DisableMenu();
            return;
        }

        controllerSO.controller.BuildCemetry(controllerSO.stats);
        DisableMenu();
    }
    public void OnZigguratClicked()
    {
        if (controllerSO.controller == null)
        {
            DisableMenu();
            return;
        }

        controllerSO.controller.BuildZiggurat(controllerSO.stats);
        DisableMenu();
    }

    public void OnSummonArmyClicked()
    {
        if (controllerSO.controller == null)
        {
            DisableMenu();
            return;
        }

        controllerSO.controller.SummonArmy(controllerSO.stats);
        DisableMenu();
    }

    public void OnDemolishClicked()
    {
        if (controllerSO.controller == null)
        {
            DisableMenu();
            return;
        }

        controllerSO.controller.Demolish(controllerSO.stats);
        DisableMenu();
    }

    public void OnAttackVillageClicked()
    {
        if (controllerSO.controller == null)
        {
            DisableMenu();
            return;
        }

        controllerSO.controller.Attack(controllerSO.stats);
        DisableMenu();
    }

    public void OnDiseaseClicked()
    {
        if (controllerSO.controller == null)
        {
            DisableMenu();
            return;
        }

        controllerSO.controller.SpreadDisease(controllerSO.stats);
        DisableMenu();
    }

    public void OnVillageWeakenClicked()
    {
        if (controllerSO.controller == null)
        {
            DisableMenu();
            return;
        }

        //Debug.Log("Village weakened");
        decisionManager.WeakVillage();
        DisableMenu();
    }

    public void OnCityWeakenClicked()
    {
        if (controllerSO.controller == null)
        {
            DisableMenu();
            return;
        }

        //Debug.Log("City weakened");
        decisionManager.WeakCity();
        DisableMenu();
    }

    public void OnInvestigateClicked()
    {
        if (controllerSO.controller == null)
        {
            DisableMenu();
            return;
        }

        //Debug.Log("Investigated");
        decisionManager.ExploreRuins();
        controllerSO.controller.Investigate();
        DisableMenu();
    }

    public void OnCorruptSantum()
    {
        if (controllerSO.controller == null)
        {
            DisableMenu();
            return;
        }

        //Debug.Log("Corrupted");
        controllerSO.controller.CorruptSanctum(controllerSO.stats);
        DisableMenu();
    }

    public void OnCityPanelOpened()
    {
        defenceText.text = controllerSO.controller.Defence.ToString();
        populationText.text = controllerSO.controller.Population.ToString();
        cityName.text = controllerSO.controller.cityName;
    }

    private void DisableMenu()
    {
        foreach (GameObject button in childrenToEnable)
        {
            button.SetActive(false);
        }

        float army = controllerSO.stats.GetParameter("force");

        if (army <= 0)
            controllerSO.death.Raise();
    }

    private bool isTechOpen(int techId)
    {
        float f = controllerSO.stats.GetParameter("tech" + techId);

        if (f == 1)
            return true;
        else
            return false;
    }
}
