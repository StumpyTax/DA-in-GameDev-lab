using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColiderBeh : MonoBehaviour
{
  	public Material[] materials;
  private void OnTriggerEnter(Collider other) {
    var perceptron=GameObject.Find("Perceptron").GetComponent<Perceptron>();
    Dictionary<string,int> blackWhite=new Dictionary<string, int>(){{"Black (Instance)",0},{"White (Instance)",1}};
    var firstValue=blackWhite[this.GetComponent<Renderer>().material.name];
    var secondValue=blackWhite[other.GetComponent<Renderer>().material.name];
    var res=perceptron.CalcOutput(firstValue,secondValue);
    other.gameObject.SetActive(false);
    this.GetComponent<Renderer>().material=materials[(int)res];
  }
}
