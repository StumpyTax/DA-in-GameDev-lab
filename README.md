# АНАЛИЗ ДАННЫХ И ИСКУССТВЕННЫЙ ИНТЕЛЛЕКТ [in GameDev]
Отчет по лабораторной работе #1 выполнил(а):
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
Познакомиться с программными средствами для создания системы машинного обучения и ее интеграции в Unity.

## Задание 1
### Реализовать систему машинного обучения в связке Python -Google-Sheets – Unity.

```С#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class RollerAgent : Agent
{
    Rigidbody rBody;
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform Target;
    public override void OnEpisodeBegin()
    {
        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        Target.localPosition = new Vector3(Random.value * 8-4, 0.5f, Random.value * 8-4);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }
    public float forceMultiplier = 10;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        if(distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        else if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }
}
```

## Задание 2
### Подробно опишите каждую строку файла конфигурации нейронной сети, доступного в папке с файлами проекта по ссылке. Самостоятельно найдите информацию о компонентах Decision Requester, Behavior Parameters, добавленных на сфере.

```yaml
#Указывает какое поведение отслеживается
behaviors:
  #Имя модели поведения
  RollerBall:
    #Тип обучения, по умолчанию ppo
    trainer_type: ppo
    #Параметры содержащие данные которые управляют обучением
    hyperparameters:
    #Количество опытов в каждой итерации градиентного спуска.
      batch_size: 10
      #При trainer_type:
      # PPO: количество опытов, которые необходимо собрать перед обновлением модели поведения. Соответствует тому, сколько опыта должно быть собрано, прежде чем мы будем изучать или обновлять модель. Это значение должно быть в несколько раз больше, чем batch_size. Обычно больший размер буфера соответствует более стабильным обновлениям обучения.
      # SAC: максимальный размер буфера опыта — примерно в тысячи раз больше, чем ваши эпизоды, чтобы SAC мог учиться как на старом, так и на новом опыте.
      buffer_size: 100
      # Начальная скорость обучения для градиентного спуска
      learning_rate: 3.0e-4
      #Сила энтропийной регуляризации, которая делает поведение «более случайным».
      beta: 5.0e-4
      #Влияет на то, насколько быстро поведение может развиваться во время обучения
      epsilon: 0.2
      # Параметр регуляризации (лямбда), используемый при расчете обобщенной оценки преимущества. Его можно рассматривать как то, насколько агент полагается на свою текущую оценку стоимости при расчете обновленной оценки стоимости.
      lambd: 0.99
      # Количество проходов через буфер опыта при выполнении оптимизации градиентного спуска.
      num_epoch: 3
      #Определяет, как скорость обучения изменяется с течением времени.
      learning_rate_schedule: linear
    #Конфигурация для нейронной сети
    network_settings:
      # Применяется ли нормализация к входным данным векторного наблюдения.
      normalize: false
      #Количество юнитов в скрытых слоях нейронной сети
      hidden_units: 128
      #Количество скрытых слоев в нейронной сети
      num_layers: 2
    #Позволяет задавать настройки как для внешних (т. е. основанных на среде), так и для внутренних сигналов вознаграждения (например, любопытство и GAIL).
    reward_signals:
      #Настройки для сигналов основанных на среде
      extrinsic:
        #Коэффициент дисконтирования для будущих вознаграждений, поступающих из окружающей среды.
        gamma: 0.99
        #Фактор, на который умножается вознаграждение, данное средой. Типичные диапазоны будут варьироваться в зависимости от сигнала вознаграждения.
        strength: 1.0
    #Максимальное кол-во шагов
    max_steps: 500000
    #Сколько шагов опыта нужно собрать для каждого агента, прежде чем добавить его в буфер опыта.
    time_horizon: 64
    #Количество опытов, которое необходимо собрать перед созданием и отображением статистики обучения.
    summary_freq: 10000
```
DecisionRequest это компонент который вызывает RequestDecision.
Behavior Parameters это параметры поведения.

## Задание 3
### Доработайте сцену и обучите ML-Agent таким образом, чтобы шар перемещался между двумя кубами разного цвета. Кубы должны, как и в первом задании, случайно изменять координаты на плоскости.

```С#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class RollerAgent : Agent
{
  Rigidbody rBody;
  // Start is called before the first frame update
  void Start()
  {
    rBody=GetComponent<Rigidbody>();
  }
  public Transform Target;
  public Transform Target2;
  public bool firstOrSecond;
  public float targetsDistance=8;
  public override void OnEpisodeBegin()
  {
    if (this.transform.localPosition.y<0){
      this.rBody.angularVelocity=Vector3.zero;
      this.rBody.velocity=Vector3.zero;
      this.transform.localPosition=new Vector3(0,0.5f,0);
    }

    Target.localPosition=new Vector3(Random.value*8-4,0.5f,Random.value*8-4);
    Target2.localPosition=new Vector3(Random.value*8-4,0.5f,Random.value*8-4);
    targetsDistance=Vector3.Distance(Target.localPosition,Target2.localPosition);
    if (!(targetsDistance>3 && targetsDistance<7))
      while(!(targetsDistance>3 && targetsDistance<7)){
       Target.localPosition=new Vector3(Random.value*8-4,0.5f,Random.value*8-4);
        Target2.localPosition=new Vector3(Random.value*8-4,0.5f,Random.value*8-4);
        targetsDistance=Vector3.Distance(Target.localPosition,Target2.localPosition);
      }
  }

  public override void CollectObservations(VectorSensor sensor)
  {
    sensor.AddObservation(Target.localPosition);
    sensor.AddObservation(Target2.localPosition);
    sensor.AddObservation(this.transform.localPosition);
    sensor.AddObservation(rBody.velocity.x);
    sensor.AddObservation(rBody.velocity.z);
  }
  public float forceMultiplier=10;
  public override void OnActionReceived(ActionBuffers actionBuffers)
  {
    Vector3 controlSignal =Vector3.zero;
    controlSignal.x=actionBuffers.ContinuousActions[0];
    controlSignal.z=actionBuffers.ContinuousActions[1];
    rBody.AddForce(controlSignal*forceMultiplier);

    float distanceToTarget=Vector3.Distance(this.transform.localPosition,Target.localPosition);
    float distanceToTarget2=Vector3.Distance(this.transform.localPosition,Target2.localPosition);
    if(distanceToTarget+distanceToTarget2==targetsDistance){
      SetReward(1.0f);
      EndEpisode();
    }
    else if (this.transform.localPosition.y<0){
      EndEpisode();
    }

  }
}

```

## Выводы
Игровой баланс — это ответвление игрового дизайна, которое описывается как математико-алгоритмическая модель игровых чисел, игровой механики и взаимосвязей между ними. Игровой баланс состоит из настройки значений для создания определенного пользовательского опыта. Улучшение восприятия и опыта игроков является целью балансировки игры.

Системы машинного обучения могут быть использованы для составления статистики. Благодаря машинному обучению можно симулировать на миллионы игр больше, чем способны сыграть живые игроки за тот же промежуток времени. И исходя из этой статистики менять баланс.

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
