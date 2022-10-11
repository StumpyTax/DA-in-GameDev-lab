# АНАЛИЗ ДАННЫХ И ИСКУССТВЕННЫЙ ИНТЕЛЛЕКТ [in GameDev]
Отчет по лабораторной работе 2 выполнил(а):
- Демидов Никита Александрович
- РИ-210913
Отметка о выполнении заданий (заполняется студентом):

| Задание | Выполнение | Баллы |
| ------ | ------ | ------ |
| Задание 1 | * | 60 |
| Задание 2 | * | 20 |
| Задание 3 | * | 20 |

знак "*" - задание выполнено; знак "#" - задание не выполнено;

Работу проверили:
- к.т.н., доцент Денисов Д.В.
- к.э.н., доцент Панов М.А.
- ст. преп., Фадеев В.О.

[![N|Solid](https://cldup.com/dTxpPi9lDf.thumb.png)](https://nodesource.com/products/nsolid)

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

Структура отчета

- Данные о работе: название работы, фио, группа, выполненные задания.
- Цель работы.
- Задание 1.
- Код реализации выполнения задания. Визуализация результатов выполнения (если применимо).
- Задание 2.
- Код реализации выполнения задания. Визуализация результатов выполнения (если применимо).
- Задание 3.
- Код реализации выполнения задания. Визуализация результатов выполнения (если применимо).
- Выводы.
- ✨Magic ✨

## Цель работы
Познакомиться с программными средствами для организции передачи данных между инструментами google, Python и Unity
## Задание 1
### Реализовать совместную работу и передачу данных в связке Python
- Google-Sheets – Unity. При выполнении задания используйте видео-материалы и исходные данные, предоставленные преподавателя курса.
- В облачном сервисе google console подключить API для работы с google sheets и google drive.
- Реализовать запись данных из скрипта на python в google-таблицу. Данные описывают изменение темпа инфляции на протяжении 11 отсчётных периодов, с учётом стоимости игрового объекта в каждый период.
- Создать новый проект на Unity, который будет получать данные из google-таблицы, в которую были записаны данные в предыдущем пункте.
- Написать функционал на Unity, в котором будет воспризводиться аудио-файл в зависимости от значения данных из таблицы.
```py
import gspread
import numpy as np
gc = gspread.service_account(filename='lab2-365206-3534427906c2.json')
sh = gc.open("UnitySheets")
price = np.random.randint(2000, 10000, 11)
mon = list(range(1,11))
i = 0
while i <= len(mon):
    i += 1
    if i == 0:
        continue
    else:
        tempInf = ((price[i-1]-price[i-2])/price[i-2])*100
        tempInf = str(tempInf)
        tempInf = tempInf.replace('.',',')
        sh.sheet1.update(('A' + str(i)), str(i))
        sh.sheet1.update(('B' + str(i)), str(price[i-1]))
        sh.sheet1.update(('C' + str(i)), str(tempInf))
        print(tempInf)
```

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class NewBehaviourScript : MonoBehaviour
{
    public AudioClip goodSpeak;
    public AudioClip normalSpeak;
    public AudioClip badSpeak;
    private AudioSource selectAudio;
    private Dictionary<string,float> dataSet = new Dictionary<string, float>();
    private bool statusStart = false;
    private int i = 1;
    // Start is called before the first frame update
    void Start()
    {
      StartCoroutine(GoogleSheets());
    }

    // Update is called once per frame
    void Update()
    {
      if (dataSet["Mon_" + i.ToString()] <= 10 & statusStart == false & i != dataSet.Count)
      {
        StartCoroutine(PlaySelectAudioGood());
        Debug.Log(dataSet["Mon_" + i.ToString()]);
      }
      if (dataSet["Mon_" + i.ToString()] > 10 & dataSet["Mon_" + i.ToString()] < 100 & statusStart == false & i != dataSet.Count)
      {
       StartCoroutine(PlaySelectAudioNormal());
        Debug.Log(dataSet["Mon_" + i.ToString()]);
      }
      if (dataSet["Mon_" + i.ToString()] >= 100 & statusStart == false & i != dataSet.Count)
      {
        StartCoroutine(PlaySelectAudioBad());
        Debug.Log(dataSet["Mon_" + i.ToString()]);
      }
        if (dataSet["Mon_" + i.ToString()] >= 100 & statusStart == false & i != dataSet.Count)
      {
        StartCoroutine(PlaySelectAudioBad());
        Debug.Log(dataSet["Mon_" + i.ToString()]);
      }
    }

    IEnumerator GoogleSheets()
    {
      UnityWebRequest curResp= UnityWebRequest.Get("https://sheets.googleapis.com/v4/spreadsheets/1I3t3_KU0g1LEF2WdRkdR3IKtayQwQZvQhjTE2qUjl0Q/values/Sheet1?key=AIzaSyC-pt1nUQ19P7nigWWN3EODlsjFmozZhK0");
      yield return curResp.SendWebRequest();
        string rawResp = curResp.downloadHandler.text;
        var rawJson = JSON.Parse(rawResp);
        foreach (var itemRawJson in rawJson["values"])
        {
            var parseJson = JSON.Parse(itemRawJson.ToString());
            var selectRow = parseJson[0].AsStringList;
            dataSet.Add(("Mon_" + selectRow[0]), float.Parse(selectRow[2]));
        }
    }
    IEnumerator PlaySelectAudioGood()
    {
      statusStart = true;
      selectAudio = GetComponent<AudioSource>();
      selectAudio.clip = goodSpeak;
      selectAudio.Play();
      yield return new WaitForSeconds(3);
      statusStart = false;
      i++;
    }
    IEnumerator PlaySelectAudioNormal()
    {
      statusStart = true;
      selectAudio = GetComponent<AudioSource>();
      selectAudio.clip = normalSpeak;
      selectAudio.Play();
      yield return new WaitForSeconds(3);
      statusStart = false;
      i++;
    }
    IEnumerator PlaySelectAudioBad()
    {
      statusStart = true;
      selectAudio = GetComponent<AudioSource>();
      selectAudio.clip = badSpeak;
      selectAudio.Play();
      yield return new WaitForSeconds(4);
      statusStart = false;
      i++;
    }
}

```


## Задание 2
###Реализовать запись в Google-таблицу набора данных, полученных с помощью линейной регрессии из лабораторной работы № 1
```py

import gspread
import numpy as np
import matplotlib.pyplot as plt

def model(a, b, x):
    return a*x + b

def loss_function(a, b, x, y):
    num = len(x)
    prediction=model(a, b, x)
    return (0.5/num) * (np.square(prediction-y)).sum()

def optimize(a, b, x, y):
    num = len(x)
    prediction = model(a, b, x)
    da = (1.0/num) * ((prediction -y)*x).sum()
    db = (1.0/num) * ((prediction -y).sum())
    a = a - Lr*da
    b = b - Lr*db
    return a, b

def iterate(a, b, x, y, times):
    for i in range(times):
        a,b = optimize(a,b,x,y)
    return a,b

a = np.random.rand(1)
b = np.random.rand(1)
Lr = 0.00001

gc = gspread.service_account(filename='lab2-365206-3534427906c2.json')

sh = gc.open("UnitySheets")

price = np.random.randint(2000, 10000, 11)
mon = list(range(1,12))

a,b = iterate(a,b,price,mon,100)
prediction = model(a,b,price)

i = 0
while i < len(mon):
    i += 1
    if i == 0:
        continue
    else:
        predInf=((prediction[i-1]-prediction[i-2])/prediction[i-2])*100
        tempInf = ((price[i-1]-price[i-2])/price[i-2])*100
        tempInf = str(tempInf)
        tempInf = tempInf.replace('.',',')
        predInf = str(predInf)
        predInf = predInf.replace('.',',')
        sh.sheet1.update(('A' + str(i)), str(i))
        sh.sheet1.update(('B' + str(i)), str(price[i-1]))
        sh.sheet1.update(('C' + str(i)), str(tempInf))
        sh.sheet1.update(('D' + str(i)), str(prediction[i-1]))
        sh.sheet1.update(('E' + str(i)), str(predInf))
        print(tempInf)
```

## Задание 3
### Самостоятельно разработать сценарий воспроизведения звукового сопровождения в Unity в зависимости от изменения считанных данных в задании 2

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class NewBehaviourScript : MonoBehaviour
{
    public AudioClip goodSpeak;
    public AudioClip normalSpeak;
    public AudioClip badSpeak;
    public AudioClip zolotoSpeak;
    private AudioSource selectAudio;
    private Dictionary<string,(float,float)> dataSet = new Dictionary<string, (float,float)>();
    private bool statusStart = false;
    private int i = 1;
    // Start is called before the first frame update
    void Start()
    {
      StartCoroutine(GoogleSheets());
    }

    // Update is called once per frame
    void Update()
    {
      if (dataSet["Mon_" + i.ToString()].Item1 <= 10 & statusStart == false & i != dataSet.Count)
      {
        StartCoroutine(PlaySelectAudioGood());
        Debug.Log(dataSet["Mon_" + i.ToString()]);
      }
      if (dataSet["Mon_" + i.ToString()].Item1 > 10 & dataSet["Mon_" + i.ToString()].Item1 < 100 & statusStart == false & i != dataSet.Count)
      {
        StartCoroutine(PlaySelectAudioNormal());
        Debug.Log(dataSet["Mon_" + i.ToString()]);
      }
      if (dataSet["Mon_" + i.ToString()].Item1 >= 100 & dataSet["Mon_" + i.ToString()].Item1<120 & statusStart == false & i != dataSet.Count)
      {
        StartCoroutine(PlaySelectAudioBad());
        Debug.Log(dataSet["Mon_" + i.ToString()]);
      }
        if (dataSet["Mon_" + i.ToString()].Item2 >= 120 & statusStart == false & i != dataSet.Count)
      {
        StartCoroutine(PlaySelectAudioZoloto());
        Debug.Log(dataSet["Mon_" + i.ToString()]);
      }
    }

    IEnumerator GoogleSheets()
    {
      UnityWebRequest curResp= UnityWebRequest.Get("https://sheets.googleapis.com/v4/spreadsheets/1I3t3_KU0g1LEF2WdRkdR3IKtayQwQZvQhjTE2qUjl0Q/values/Sheet1?key=AIzaSyC-pt1nUQ19P7nigWWN3EODlsjFmozZhK0");
      yield return curResp.SendWebRequest();
        string rawResp = curResp.downloadHandler.text;
        var rawJson = JSON.Parse(rawResp);
        foreach (var itemRawJson in rawJson["values"])
        {
            var parseJson = JSON.Parse(itemRawJson.ToString());
            var selectRow = parseJson[0].AsStringList;
            dataSet.Add(("Mon_" + selectRow[0]), (float.Parse(selectRow[2]),float.Parse(selectRow[4])));
        }
    }
    IEnumerator PlaySelectAudioGood()
    {
      statusStart = true;
      selectAudio = GetComponent<AudioSource>();
      selectAudio.clip = goodSpeak;
      selectAudio.Play();
      yield return new WaitForSeconds(3);
      statusStart = false;
      i++;
    }
    IEnumerator PlaySelectAudioNormal()
    {
      statusStart = true;
      selectAudio = GetComponent<AudioSource>();
      selectAudio.clip = normalSpeak;
      selectAudio.Play();
      yield return new WaitForSeconds(3);
      statusStart = false;
      i++;
    }
    IEnumerator PlaySelectAudioBad()
    {
      statusStart = true;
      selectAudio = GetComponent<AudioSource>();
      selectAudio.clip = badSpeak;
      selectAudio.Play();
      yield return new WaitForSeconds(4);
      statusStart = false;
      i++;
    }
      IEnumerator PlaySelectAudioZoloto()
    {
      statusStart = true;
      selectAudio = GetComponent<AudioSource>();
      selectAudio.clip = zolotoSpeak;
      selectAudio.Play();
      yield return new WaitForSeconds(3);
      statusStart = false;
      i++;
    }
}

```

## Выводы
Познакомился с программными средствами для организции передачи данных между инструментами google, Python и Unity

| Plugin | README |
| ------ | ------ |
| Dropbox | [plugins/dropbox/README.md][PlDb] |
| GitHub | [plugins/github/README.md][PlGh] |
| Google Drive | [plugins/googledrive/README.md][PlGd] |
| OneDrive | [plugins/onedrive/README.md][PlOd] |
| Medium | [plugins/medium/README.md][PlMe] |
| Google Analytics | [plugins/googleanalytics/README.md][PlGa] |

## Powered by

**BigDigital Team: Denisov | Fadeev | Panov**
