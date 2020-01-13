using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public TileControllerSO so;
    public Dictionary<string, float> parameters; //словарь с игровыми параметрами

    private readonly char[] separator = new char[] { ',' }; //разделитель, используемый в форматировании (запятая)
    private readonly char[] separator2 = new char[] { ' ' }; //разделитель, используемый в форматировании (пробел)

    // Start is called before the first frame update
    public void Initialize()
    {
        parameters = new Dictionary<string, float>
        {
            {"souls", 5 },
            {"mana", 15 },
            {"manaIncome", 0 },
            {"force", 5 },
            {"danger", 5 },
            {"turn", 1 },
            {"rebirth", 1 },
            {"tech0", 1 },
            {"tech1", 0 }, 
            {"tech2", 0 },
            {"tech3", 0 },
            {"tech4", 0 },
            {"tech5", 0 },
            {"tech6", 0 },
            {"tech7", 0 },
            {"tech8", 0 },
            {"tech9", 0 },
            {"tech10", 0 },
            {"tech11", 0 },
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ConditionsVerifier(string conditions) //проверка, соблюдаются ли переданные в виде строки условия
    {
        string[] conds;
        conds = conditions.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
        bool value = true;
        foreach (string condition in conds)
        {
            if (ConditionCheck(condition) == false)
            {
                value = false;
            }
        }
        return value;
    }

    public bool ConditionCheck(string condition) //проверка конкретного условия
    {
        if (condition == "0")
        {
            return true;
        } else
        {
            string[] parts;
            parts = condition.Split(separator2, System.StringSplitOptions.RemoveEmptyEntries);
            switch (parts[1])
            {
                case ">":
                    if (parameters.ContainsKey(parts[0]))
                    {
                        if (parameters[parts[0]] >= int.Parse(parts[2]))
                        {
                            return true;
                        } else { return false; }
                    } else { return false; }
                case "<":
                    if (parameters.ContainsKey(parts[0]))
                    {
                        if (parameters[parts[0]] < int.Parse(parts[2]))
                        {
                            return true;
                        }
                        else { return false; }
                    }
                    else { return false; }
                case "=":
                    if (parameters.ContainsKey(parts[0]))
                    {
                        if (parameters[parts[0]] == int.Parse(parts[2]))
                        {
                            return true;
                        }
                        else { return false; }
                    }
                    else { return false; }
                default:
                    return false;
            }
        }

    }

    public void ParametersChangerFromString(string effect) //обработка строки с эффектами через запятую
    {
        string[] parameters;
        parameters = effect.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string parameter in parameters)
        {
            ParametersChanger(parameter);
        }
    }

    public void ParametersChanger(string parameter)
    {
        if (parameter == "0")
        {
            return;
        }
        string[] parts;
        parts = parameter.Split(separator2, System.StringSplitOptions.RemoveEmptyEntries);
        if (parts[0] == "population")
        {
            so.controller.Population -= int.Parse(parts[2]);
        }
        else if (parts[0] == "defence")
        {
            so.controller.Defence -= int.Parse(parts[2]);
        }
        else
        {
            switch (parts[1])
            {
                case "+":
                    if (parameters.ContainsKey(parts[0]))
                    {
                        parameters[parts[0]] += float.Parse(parts[2]);
                    }
                    break;
                case "-":
                    if (parameters.ContainsKey(parts[0]))
                    {
                        parameters[parts[0]] -= float.Parse(parts[2]);
                    }
                    break;
                case "=":
                    if (parameters.ContainsKey(parts[0]))
                    {
                        parameters[parts[0]] = float.Parse(parts[2]);
                    }
                    break;
                default:
                    break;
            }
        }

        // Запуск смерти после решения события
        if(parts[0] == "army")
        {
            if (parameters[parts[0]] <= 0)
                so.death.Raise();
        }
    }

    public void ParametersManualChanger(string parameter, string action, float value)
    {
        if (parameters.ContainsKey(parameter))
        {
            switch (action)
            {
                case "+":
                    parameters[parameter] += value;
                    break;
                case "-":
                    parameters[parameter] -= value;
                    break;
                case "=":
                    parameters[parameter] = value;
                    break;
                default:
                    break;
            }
        } else { Debug.Log("Ключа " + parameter + " нет в словаре"); }   
    }

    public float GetParameter(string param)
    {
        if (parameters.ContainsKey(param) == true)
        {
            return parameters[param];
        } else
        {
            return 0;
        }
    }
}
