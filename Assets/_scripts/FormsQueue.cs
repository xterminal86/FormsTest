using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormsQueue : MonoBehaviour 
{
  public Image ImageRef;
  public Text ElementsCounter;
  public RectTransform FillMeter;

  [System.NonSerialized]
  public int MaxElements = 10;

  Queue<Form> _queue = new Queue<Form>();
  public Queue<Form> QueueRef
  {
    get { return _queue; }
  }

  float _meterMaxHeight = 69.0f;
  float _heightDelta = 0.0f;
  void Start()
  {
    _heightDelta = _meterMaxHeight / (float)MaxElements;
    FillMeter.sizeDelta = new Vector2(38.0f, _heightDelta * _queue.Count);
  }

  public void MouseEnter()
  {
    if (_queue.Count == 0)
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

  public void MouseClick()
  {
    if (_queue.Count == 0)
    {
      Overseer.Instance.FlashText("No forms in queue!", 1.0f);
    }
    else
    {
      RestoreForm();
    }
  }

  public void AddForm(Form f)
  {    
    _queue.Enqueue(f);
    ElementsCounter.text = _queue.Count.ToString();
    FillMeter.sizeDelta = new Vector2(38.0f, _heightDelta * _queue.Count);
  }

  void RestoreForm()
  {
    Form f = _queue.Dequeue();

    if (_queue.Count == 0)
    {
      ImageRef.color = Color.red;
      ElementsCounter.color = Color.red;
    }
    else
    {
      ImageRef.color = Color.green;
      ElementsCounter.color = Color.green;
    }

    ElementsCounter.text = _queue.Count.ToString();
    FillMeter.sizeDelta = new Vector2(38.0f, _heightDelta * _queue.Count);

    StartCoroutine(RestoreFormRoutine(f));
  }

  float _movingTime = 0.2f;
  IEnumerator RestoreFormRoutine(Form f)
  {
    f.gameObject.SetActive(true);

    Vector3 pos = f.transform.position;
    Vector3 posToMoveAt = new Vector3(Screen.width / 2, Screen.height / 2, 0.0f);

    Vector3 scaleVector = Vector3.zero;

    float t = 0.0f;

    float dt = t / _movingTime;

    while (dt < 1.0f)
    {
      t += Time.smoothDeltaTime;

      dt = t / _movingTime;

      dt = Mathf.Clamp(dt, 0.0f, 1.0f);

      f.transform.position = Vector3.Lerp(pos, posToMoveAt, dt);

      scaleVector.Set(dt, dt, dt);

      f.transform.localScale = scaleVector;

      yield return null;
    }

    yield return null;
  }
}
