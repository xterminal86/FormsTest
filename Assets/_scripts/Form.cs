﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Form : MonoBehaviour 
{
  public Canvas FormCanvas;
  public Text FormHead;
  public InputField FormText;

  public RectTransform DragZone;

  bool _isGrabbed = false;

  void Start()
  { 
    StartCoroutine(FormAppearRoutine());
  }

  public void SetFormHead()
  {
    FormHead.text = string.Format("Form {0}", Overseer.Instance.GlobalIdentifier);
    Overseer.Instance.GlobalIdentifier++;
  }

  float _formScaleSpeed = 10.0f;
  IEnumerator FormAppearRoutine()
  {
    Vector3 scale = transform.localScale;

    float scaleCounter = 0.0f;
    scale.Set(scaleCounter, scaleCounter, scaleCounter);
    transform.localScale = scale;

    while (scaleCounter < 1.0f)
    {
      scaleCounter += Time.smoothDeltaTime * _formScaleSpeed;

      scale.Set(scaleCounter, scaleCounter, scaleCounter);

      scale.x = Mathf.Clamp(scale.x, 0.0f, 1.0f);
      scale.y = Mathf.Clamp(scale.y, 0.0f, 1.0f);
      scale.z = Mathf.Clamp(scale.z, 0.0f, 1.0f);

      transform.localScale = scale;

      yield return null;
    }

    yield return null;
  }

  Vector3 _originalPosition = Vector3.zero;
  public void DragZoneSelected()
  {
    _isGrabbed = true;
    _originalPosition.Set(transform.position.x, transform.position.y, transform.position.z);
    _oldMousePos = Input.mousePosition;

    if (FormCanvas.sortingOrder != Overseer.Instance.SortingOrderMax)
    {
      FormCanvas.sortingOrder = Overseer.Instance.SortingOrderMax + 1;
      Overseer.Instance.SortingOrderMax++;
    }

    Overseer.Instance.LastDraggedForm = this;
  }

  public void DragZoneDeselected()
  {
    _isGrabbed = false;
  }

  float _dx = 0.0f, _dy = 0.0f;
  Vector3 _oldMousePos = Vector3.zero;
  public void DragZoneMoving()
  {    
    _dx = Input.mousePosition.x - _oldMousePos.x;
    _dy = Input.mousePosition.y - _oldMousePos.y;

    _oldMousePos = Input.mousePosition;

    transform.position = new Vector3(_originalPosition.x + _dx, _originalPosition.y + _dy, 0.0f);
    _originalPosition = transform.position;
  }

  bool _isClosing = false;
  public void CloseForm()
  {
    if (!_isClosing)
    {
      _isClosing = true;
      StartCoroutine(FormCloseRoutine());
    }
  }

  IEnumerator FormCloseRoutine()
  {
    Vector3 scale = transform.localScale;

    float scaleCounter = 1.0f;

    while (scaleCounter > 0.0f)
    {
      scaleCounter -= Time.smoothDeltaTime * _formScaleSpeed;

      scale.Set(scaleCounter, scaleCounter, scaleCounter);

      scale.x = Mathf.Clamp(scale.x, 0.0f, 1.0f);
      scale.y = Mathf.Clamp(scale.y, 0.0f, 1.0f);
      scale.z = Mathf.Clamp(scale.z, 0.0f, 1.0f);

      transform.localScale = scale;

      yield return null;
    }

    Destroy(gameObject);

    if (FormCanvas.sortingOrder == Overseer.Instance.SortingOrderMax)
    {
      Overseer.Instance.SortingOrderMax--;
    }

    yield return null;
  }

  public void ToQueueHandler()
  {
    if (Overseer.Instance.FormsQueueRef.Container.Count >= Overseer.Instance.FormsQueueRef.MaxElements)
    {
      Overseer.Instance.FormsQueueRef.FlashContainer();
      Overseer.Instance.FlashText("Queue is full!", 1.0f);
      return;
    }

    var go = GameObject.Find("queue").GetComponent<RectTransform>();

    // We should add form to container before animation to avoid adding when previous animation is still playing

    Overseer.Instance.FormsQueueRef.AddForm(this);

    StartCoroutine(MoveAndShrinkFormRoutine(go.position, () =>
    {
      _data.State = FormState.IN_QUEUE;
    }));
  }

  public void ToStackHandler()
  {
    if (Overseer.Instance.FormsStackRef.Container.Count >= Overseer.Instance.FormsStackRef.MaxElements)
    {
      Overseer.Instance.FormsStackRef.FlashContainer();
      Overseer.Instance.FlashText("Stack is full!", 1.0f);
      return;
    }

    var go = GameObject.Find("stack").GetComponent<RectTransform>();

    // We should add form to container before animation to avoid adding when previous animation is still playing

    Overseer.Instance.FormsStackRef.AddForm(this);

    StartCoroutine(MoveAndShrinkFormRoutine(go.position, () =>
    {
      _data.State = FormState.IN_STACK;
    }));
  }

  float _movingTime = 0.2f;
  IEnumerator MoveAndShrinkFormRoutine(Vector3 posToMoveAt, Callback cb = null)
  {    
    Vector3 pos = transform.position;

    Vector3 scaleVector = Vector3.one;

    float t = 0.0f;

    float dt = t / _movingTime;

    while (dt < 1.0f)
    {
      dt = t / _movingTime;

      t += Time.smoothDeltaTime;

      dt = Mathf.Clamp(dt, 0.0f, 1.0f);

      transform.position = Vector3.Lerp(pos, posToMoveAt, dt);

      float scale = 1.0f - dt;

      scaleVector.Set(scale, scale, scale);

      transform.localScale = scaleVector;

      yield return null;
    }

    gameObject.SetActive(false);

    if (cb != null)
    {
      cb();
    }

    yield return null;
  }

  FormData _data = new FormData();
  public FormData Data
  {
    get { return _data; }
  }

  public FormData GetFormData()
  {    
    _data.FormHead = FormHead.text;
    _data.FormText = FormText.text;
    _data.PosX = transform.position.x;
    _data.PosY = transform.position.y;
    _data.SortingOrder = FormCanvas.sortingOrder;

    return _data;
  }
}
