using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;

public class DecisionManager : MonoBehaviour
{
    public GameObject player;
    

    public TextAsset decisionsFile; //JSON-файл с решениями
    public TextAsset answersFile; //JSON-файл с ответами

    public GameObject[] notifs;
    public TextMeshProUGUI[] notifsTexts;

    List<Decision> allDecisions; //список всех решений
    List<Answer> allAnswers; //список всех ответов

    Decision currentDecision; //решение, с которым взаимодействует игрок
    List<Answer> answersPack; //набор ответов к currentDecision

    List<Decision> decisionsPack; //набор решений на ход
    List<Decision> decisionQueue; //очередь решений

    DecisionWindowControl decisionWindowControl;
    Stats stats;

    string[] ruinsDecisionsItems;
    string[] cityDecisionsItems;
    string[] villageDecisionsItems;

    private readonly char[] separator = new char[] { ',' }; //разделитель, используемый в форматировании (запятая)


    // Start is called before the first frame update
    void Awake()
    {
        decisionWindowControl = gameObject.GetComponent<DecisionWindowControl>();
        stats = player.GetComponent<Stats>();
    }

    public void Initialize() //функция вызывается из основного геймменеджера
    {
        Debug.Log("Начата инициализация системы решений");
        decisionsPack = new List<Decision>();
        decisionQueue = new List<Decision>();
        ruinsDecisionsItems = new string[] { "16", "17" };
        cityDecisionsItems = new string[] { "14" };
        villageDecisionsItems = new string[] { "11", "12", "13" };
        LoadDecisions();
        Decision2Pack("1", true);

    }

    private void LoadDecisions() //загрузка решений и ответов из JSON-файлов
    {
        string jsonDecisionsImport = decisionsFile.text;
        allDecisions = JsonConvert.DeserializeObject<List<Decision>>(jsonDecisionsImport);
        Debug.Log("Загружено " + allDecisions.Count.ToString() + " решений");

        string jsonAnswersImport = answersFile.text;
        allAnswers = JsonConvert.DeserializeObject<List<Answer>>(jsonAnswersImport);
        Debug.Log("Загружено " + allAnswers.Count.ToString() + " ответов");
    }

    void DrawNotifications()
    {
        Debug.Log("Отрисовка уведомлений началась");
        for (int x = 0; x < 4; x++)
        {
            if (x <= decisionsPack.Count - 1)
            {
                notifsTexts[x].text = decisionsPack[x].name;
                notifs[x].SetActive(true);
            }
            else { notifs[x].SetActive(false); }
        }

    }

    Decision GetDecisionByItem(string item) //поиск решения по его уникальному item'у
    {
        foreach (Decision decision in allDecisions)
        {
            if (item == decision.item)
            {
                return decision;
            }
        }
        return null;
    }

    public void ExploreRuins()
    {
        Decision2Pack(ruinsDecisionsItems[Random.Range(0, ruinsDecisionsItems.Length)], true);
    }

    public void WeakVillage()
    {
        Decision2Pack(villageDecisionsItems[Random.Range(0, villageDecisionsItems.Length)], true);
    }

    public void WeakCity()
    {
        Decision2Pack(cityDecisionsItems[Random.Range(0, cityDecisionsItems.Length)], true);
    }

    public bool Decision2Pack(Decision addedEvent) //возвращает true, если ивент с индексом index прошел проверку условий и добавлен в пак
    {
        if (stats.ConditionsVerifier(addedEvent.condition) == true)
        {
            decisionsPack.Add(addedEvent);
            DrawNotifications();
            return true;
        }
        else { return false; }
    }

    public bool Decision2Pack(Decision addedEvent, bool isInstantEvent)
    {
        if (isInstantEvent == false)
        {
            return Decision2Pack(addedEvent);
        }
        else
        {
            if (stats.ConditionsVerifier(addedEvent.condition) == true)
            {
                decisionsPack.Insert(0, addedEvent);
                OpenDecision(0);
                DrawNotifications();
                return true;
            }
            else { return false; }
        }
    }

    public bool Decision2Pack(string item, bool isInstantEvent)
    {
        for (int x = 0; x < allDecisions.Count; x++)
        {
            if (allDecisions[x].item == item)
            {
                if (Decision2Pack(allDecisions[x], isInstantEvent) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool Decision2Queue(Decision addedEvent) //добавление ивента в очередь на следующий ход
    {
        if (stats.ConditionsVerifier(addedEvent.condition) == true)
        {
            decisionQueue.Add(addedEvent);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Decision2Queue(string eventItem)
    {
        for (int x = 0; x < allDecisions.Count; x++)
        {
            if (allDecisions[x].item == eventItem)
            {
                if (Decision2Queue(allDecisions[x]) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void OpenDecision(int num)
    {
        currentDecision = decisionsPack[num];
        CreateAnswersList(decisionsPack[num].answerItems);
        decisionWindowControl.DrawDecision(currentDecision, answersPack);
    }

    public void CreateAnswersList(string answersItems) //создание List'а ответов
    {
        string[] items;
        answersPack = new List<Answer>();
        items = answersItems.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string item in items)
        {
            foreach (Answer answer in allAnswers)
            {
                if (item == answer.answer_item)
                {
                    if (stats.ConditionsVerifier(answer.answer_condition) == true)
                    {
                        answersPack.Add(answer);
                        break;
                    }
                }
            }
        }
    }

    public void ChooseAnswer(int index) //выбор ответа
    {
        Answer chosenAnswer = answersPack[index];
        Debug.Log("Выбран ответ " + chosenAnswer.answer_text);
        stats.ParametersChangerFromString(chosenAnswer.answer_mod);
        decisionsPack.Remove(currentDecision);
        if (chosenAnswer.next_event != "0")
        {
            if (chosenAnswer.instant_event == true)
            {
                if (Decision2Pack(chosenAnswer.next_event, true) == false)
                {
                    DrawNotifications();
                    decisionWindowControl.CloseWindow();
                }
            }
            else
            {
                Decision2Queue(chosenAnswer.next_event);
                DrawNotifications();
                decisionWindowControl.CloseWindow();
            }
        }
        else
        {
            DrawNotifications();
            decisionWindowControl.CloseWindow();
        }
    }

    public bool IsRemainingDecisions()
    {
        if (decisionsPack.Count < 1)
        {
            return false;
        } else { return true; }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Decision
{
    public string item; //уникальный идентификатор
    public string name; //отображаемое название
    public string text; //отображаемый текст
    public string condition; //условия для срабатывания
    public string answerItems; //идентификаторы ответов, записанные в строчку через запятую
}

public class Answer
{
    public string answer_item;
    public string answer_text;
    public string answer_condition;
    public string next_event;
    public bool instant_event;
    public string answer_mod;
    public string conditionHint;
}
