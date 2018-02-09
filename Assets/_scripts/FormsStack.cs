using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormsStack : MonoBehaviour 
{  
  public Image ImageRef;
  public Text ElementsCounter;
  public RectTransform FillMeter;

  public int MaxElements = 20;

  Stack<Form> _stack = new Stack<Form>();

  float _meterMaxHeight = 69.0f;
  float _heightDelta = 0.0f;
  void Start()
  {
    _heightDelta = _meterMaxHeight / (float)MaxElements;
    FillMeter.sizeDelta = new Vector2(38.0f, _heightDelta * _stack.Count);
  }

  public void MouseEnter()
  {
    if (_stack.Count == 0)
    {
      ImageRef.color = Color.red;
      ElementsCounter.color = Color.red;
    }
    else
    {
      ImageRef.color = Color.green;
      ElementsCounter.color = Color.green;
    }
  }

  public void MouseExit()
  {
    ImageRef.color = Color.white;
    ElementsCounter.color = Color.white;
  }
}
