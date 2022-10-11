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
