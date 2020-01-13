using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ParametersShower : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI[] parameters;
    public TextMeshProUGUI rebirths;

    Stats stats;
    bool isActive;

    // Start is called before the first frame update
    void Awake()
    {
        stats = player.GetComponent<Stats>();
    }

    public void Initialize()
    {
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive == true)
        {
            parameters[0].text = "Души: " + Mathf.Round(stats.GetParameter("souls")).ToString();
            parameters[1].text = "Мана: " + Mathf.Round(stats.GetParameter("mana")).ToString();
            parameters[2].text = "Армия: " + Mathf.Round(stats.GetParameter("force")).ToString();
            parameters[3].text = "Угроза: " + Mathf.Round(stats.GetParameter("danger")).ToString();
            rebirths.text = stats.GetParameter("rebirth").ToString();
        }
    }
}
