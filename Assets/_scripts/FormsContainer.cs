using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormsContainer : MonoBehaviour 
{
  public Image ImageRef;
  public Text ElementsCounter;
  public RectTransform FillMeter;

  [System.NonSerialized]
  public int MaxElements = 10;

  public ICollection Container;

  protected float _meterMaxHeight = 69.0f;
  protected float _heightDelta = 0.0f;

  protected virtual void Init()
  {
    _heightDelta = _meterMaxHeight / (float)MaxElements;

    RefreshFillMeter();
  }

  public void RefreshFillMeter()
  {
    ElementsCounter.text = Container.Count.ToString();
    FillMeter.sizeDelta = new Vector2(38.0f, _heightDelta * Container.Count);
  }

  public void MouseEnter()
  {
    if (Container.Count == 0)
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

  protected void RestoreForm(Form f)
  {
    f.Data.State = FormState.NORMAL;

    if (Container.Count == 0)
    {
      ImageRef.color = Color.red;
      ElementsCounter.color = Color.red;
    }
    else
    {
      ImageRef.color = Color.green;
      ElementsCounter.color = Color.green;
    }

    RefreshFillMeter();

    StartCoroutine(RestoreFormRoutine(f));
  }

  float _movingTime = 0.2f;
  protected IEnumerator RestoreFormRoutine(Form f)
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

  public void FlashContainer()
  {
    StartCoroutine(FlashContainerRoutine());
  }

  float _flashPeriod = 0.1f;
  IEnumerator FlashContainerRoutine()
  {
    Color[] colors = new Color[2] { Color.red, Color.white };

    int index = 0;

    for (int i = 0; i < 4; i++)
    {
      float t = 0.0f;
      float dt = t / _flashPeriod;

      ImageRef.color = colors[index];
      ElementsCounter.color = colors[index];

      while (dt < 1.0f)
      {
        t += Time.smoothDeltaTime;

        dt = t / _flashPeriod;

        dt = Mathf.Clamp(dt, 0.0f, 1.0f);

        yield return null;
      }

      index++;

      if (index > 1)
      {
        index = 0;
      }
    }

    yield return null;
  }
}
