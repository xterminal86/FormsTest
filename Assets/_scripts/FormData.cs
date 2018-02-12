using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FormData
{
  public string FormHead = string.Empty;
  public string FormText = string.Empty;

  public float PosX = 0.0f;
  public float PosY = 0.0f;

  public int SortingOrder = 0;

  public FormState State = FormState.NORMAL;
}

[Serializable]
public class DataToSave
{
  public List<FormData> ActiveForms = new List<FormData>();
  public List<FormData> FormsInQueue = new List<FormData>();
  public List<FormData> FormsInStack = new List<FormData>();

  public int GlobalIdentifier = 0;
  public int SortingOrderMax = 0;

  public void Clear()
  {
    ActiveForms.Clear();
    FormsInQueue.Clear();
    FormsInStack.Clear();
  }
}

public enum FormState
{
  NORMAL = 0,
  IN_QUEUE,
  IN_STACK
}