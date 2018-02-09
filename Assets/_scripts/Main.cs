using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour 
{
  public GameObject FormPrefab;
  public Canvas CanvasTransform;

  void Start()
  {
    Overseer.Instance.Initialize();
  }

  public void AddFormHandler()
  {
    var go = Instantiate(FormPrefab, Vector3.zero, Quaternion.identity, CanvasTransform.transform);
  }
}
