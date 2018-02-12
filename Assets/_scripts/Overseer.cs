using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void Callback();

public class Overseer : MonoSingleton<Overseer>
{
  public Text InfoText;

  [HideInInspector]
  public int GlobalIdentifier = 0;

  [HideInInspector]
  public int SortingOrderMax = 0;

  [HideInInspector]
  public Form LastDraggedForm;

  FormsStack _formsStackRef;
  public FormsStack FormsStackRef
  {
    get { return _formsStackRef; }
  }

  FormsQueue _formsQueueRef;
  public FormsQueue FormsQueueRef
  {
    get { return _formsQueueRef; }
  }

  public override void Initialize()
  {
    _formsQueueRef = GameObject.Find("queue").GetComponent<FormsQueue>();
    _formsStackRef = GameObject.Find("stack").GetComponent<FormsStack>();
  }

  public void ClearContainers()
  {
    (_formsQueueRef.Container as Queue<Form>).Clear();
    (_formsStackRef.Container as Stack<Form>).Clear();

    _formsQueueRef.RefreshFillMeter();
    _formsStackRef.RefreshFillMeter();
  }

  bool _showText = false;
  float _timer = 0.0f;
  float _timeToShow = 0.0f;
  public void FlashText(string textToShow, float delaySeconds = 1.0f)
  {
    _timer = 0.0f;
    _timeToShow = delaySeconds;
    _showText = true;

    InfoText.gameObject.SetActive(true);
    InfoText.text = textToShow;
  }

  void Update()
  {
    if (!_showText)
    {
      return;
    }

    _timer += Time.smoothDeltaTime;

    InfoText.gameObject.SetActive(_timer < _timeToShow);

    if (_timer > _timeToShow)
    {
      _showText = false;
    }
  }
}
