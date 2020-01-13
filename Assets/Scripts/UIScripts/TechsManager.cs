using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TechsManager : MonoBehaviour
{
    List<Tech> techsList;

    public GameObject techWindow;
    public TextMeshProUGUI title, description, warning;
    public GameObject researchButton;

    Stats stats;

    Tech currentTech;

    private bool notOpened = true;
    private bool noClosed = true;

    // Start is called before the first frame update
    void Awake()
    {
        stats = gameObject.GetComponent<Stats>();
    }

    public void Initialize()
    {
        techsList = new List<Tech>
        {
            new Tech
            {
                item = 0,
                title = "Ритуал Вечной Ночи",
                description = "Наконец-то! Спустя десятилетия исследований вы воспроизвели давно забытый ритуал бессмертия. Пускай теперь эти напыщенные профессора из магического университета подавятся своими волшебными шляпами! Отныние вы - лич и смерть вам не страшна. Потерпев поражение, вы впадете в подобие спячки. Пара-тройка столетий и вот вы уже как огурчик, вновь готовы покорять мир!",
                price = 0,
                conditions = "0",
                isResearched = true
            },
            new Tech
            {
                item = 1,
                title = "Порча",
                description = "Позволяет распространять порчу на равнины и леса",
                conditions = "tech0 = 1",
                price = 1,
                isResearched = false
            },
            new Tech
            {
                item = 2,
                title = "Экстракция маны",
                description = "Мана - основной инструмент любого мага. Одно из главных таинств некроманта - извлечение маны из душ. В промышленных масштабах! Позволяет строить зиккурат",
                conditions = "tech0 = 1",
                price = 1,
                isResearched = false
            },
            new Tech
            {
                item = 3,
                title = "Магическая маскировка",
                description = "Позволяет ослаблять поселение",
                conditions = "tech0 = 1",
                price = 2,
                isResearched = false
            },
            new Tech
            {
                item = 4,
                title = "Наслать чуму",
                description = "Позволяет убить часть населения города (от 20% до 80% в зависимости от решения). Затрачивает ману в зависимости от общего населения",
                conditions = "tech1 = 1",
                price = 5,
                isResearched = false
            },
            new Tech
            {
                item = 5,
                title = "Ритуал призыва",
                description = "Кладбище - не просто так излюбленное место для некромантов. Местные жители горят желанием поскорее присоединиться к вашим легионам ужаса! Позволяет строить кладбище",
                conditions = "tech2 = 1",
                price = 2,
                isResearched = false
            },
            new Tech
            {
                item = 6,
                title = "Вечное проклятье",
                description = "На ближайших тайлах вокруг башни некроманта остается порча после поражения",
                conditions = "tech2 = 1",
                price = 5,
                isResearched = false
            },
            new Tech
            {
                item = 7,
                title = "Ритуал осквернения",
                description = "Позволяет осквернять святилища",
                conditions = "tech3 = 1",
                price = 5,
                isResearched = false
            },
            new Tech
            {
                item = 8,
                title = "Облачное манохранилище",
                description = "Позволяет сохранять до 10 маны после поражения",
                conditions = "tech6 = 1",
                price = 8,
                isResearched = false
            },
            new Tech
            {
                item = 9,
                title = "Тёмное пророчество",
                description = "Открывает местонахождение случайного артефакта",
                conditions = "tech7 = 1",
                price = 8,
                isResearched = false
            },
            new Tech
            {
                item = 10,
                title = "Жертвоприношение",
                description = "Позволяет пополнять свою орду нежити за счет душ",
                conditions = "tech4 = 1,tech5 = 1",
                price = 12,
                isResearched = false
            },
            new Tech
            {
                item = 11,
                title = "Призрачное воинство",
                description = "Позволяет сохранять до 10 армии после поражения",
                conditions = "tech6 = 1",
                price = 10,
                isResearched = false
            },
        };
        currentTech = techsList[0];
    }

    public void OpenTechInfo(int item)
    {
        if (GetTechByItem(item) != null)
        {
            currentTech = GetTechByItem(item);
        } else { currentTech = techsList[0]; }

        title.text = currentTech.title;
        description.text = currentTech.description + "\n\nСтоимость: " + currentTech.price.ToString() + " душ";
        researchButton.SetActive(false);
        warning.text = null;
        if (currentTech.isResearched == true)
        {
            warning.text = "Вы уже владеете этими знаниями";
        } else
        {
            if (stats.ConditionsVerifier(currentTech.conditions) == true)
            {
                if (stats.GetParameter("souls") >= currentTech.price)
                {
                    researchButton.SetActive(true);
                } else { warning.text = "Недостаточно душ"; }
            } else
            {
                warning.text = "Сначала нужно изучить предыдущие технологии";
            }
        }
    }

    public void OpenTechWindow()
    {
        Debug.Log("Открываем окно технологий");
        techWindow.SetActive(true);
        OpenTechInfo(0);

        //if (notOpened == true)
        //{
        //    player.GetComponent<MainGameManager>().DecisionManager.Decision2Pack("2", true);
        //    notOpened = false;
        //}
    }

    public void CloseTechWindow()
    {
        techWindow.SetActive(false);
    }

    public void Research()
    {
        currentTech.isResearched = true;
        stats.ParametersManualChanger("tech" + currentTech.item.ToString(), "=", 1);
        stats.ParametersManualChanger("souls", "-", currentTech.price);
        OpenTechInfo(currentTech.item);

        if (currentTech.item == 1)
        {
            gameObject.GetComponent<MainGameManager>().DecisionManager.Decision2Pack("3", false);
        }
    }

    Tech GetTechByItem(int item)
    {
        foreach (Tech tech in techsList)
        {
            if (item == tech.item)
            {
                return tech;
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Tech
{
    public int item;
    public string title;
    public string description;
    public string conditions;

    public float price;
    public bool isResearched;
}
