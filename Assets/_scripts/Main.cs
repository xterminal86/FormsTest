using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

  List<Form> _allForms = new List<Form>();
  public void AddFormHandler()
  {
    var go = Instantiate(FormPrefab, Vector3.zero, Quaternion.identity, CanvasTransform.transform);
    Form f = go.GetComponent<Form>();
    f.SetFormHead();
    _allForms.Add(f);
  }

  string _saveFile = "save.st";

  DataToSave _dataToSave = new DataToSave();
  public void SaveHandler()
  {
    _dataToSave.Clear();

    foreach (var item in _allForms)
    {
      // If we close form, value in this list becomes null
      if (item != null && item.gameObject.activeSelf)
      {
        var data = item.GetFormData();
        _dataToSave.ActiveForms.Add(data);
      }
    }

    Stack<Form> stackCopy = new Stack<Form>((Overseer.Instance.FormsStackRef.Container as Stack<Form>));
    Queue<Form> queueCopy = new Queue<Form>((Overseer.Instance.FormsQueueRef.Container as Queue<Form>));

    Form f;
    FormData formData;

    while (queueCopy.Count != 0)
    {
      f = queueCopy.Dequeue();
      formData = f.GetFormData();
      _dataToSave.FormsInQueue.Add(formData);
    }

    while (stackCopy.Count != 0)
    {
      f = stackCopy.Pop();
      formData = f.GetFormData();
      _dataToSave.FormsInStack.Add(formData);
    }

    var formatter = new BinaryFormatter();
    Stream s = new FileStream(_saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
    formatter.Serialize(s, _dataToSave);
    s.Close();

    Overseer.Instance.FlashText("State saved");
  }

  public void LoadHandler()
  {
    foreach (var item in _allForms)
    {
      // If we close form, value in this list becomes null
      if (item != null)
      {
        Destroy(item.gameObject);
      }
    }

    _allForms.Clear();

    Overseer.Instance.ClearContainers();

    var formatter = new BinaryFormatter();  
    Stream stream = new FileStream(_saveFile, FileMode.Open, FileAccess.Read, FileShare.Read);  
    _dataToSave = (DataToSave)formatter.Deserialize(stream);
    stream.Close();

    RestoreState();

    Overseer.Instance.FlashText("State loaded");
  }

  void RestoreState()
  {    
    GameObject go;
    Form f;

    foreach (var item in _dataToSave.ActiveForms)
    {
      go = Instantiate(FormPrefab, Vector3.zero, Quaternion.identity, CanvasTransform.transform);
      f = go.GetComponent<Form>();

      f.FormHead.text = item.FormHead;
      f.FormText.text = item.FormText;

      f.transform.position = new Vector3(item.PosX, item.PosY, 0.0f);
      _allForms.Add(f);
    }

    foreach (var item in _dataToSave.FormsInQueue)
    {
      go = Instantiate(FormPrefab, Vector3.zero, Quaternion.identity, CanvasTransform.transform);
      go.SetActive(false);

      f = go.GetComponent<Form>();

      f.FormHead.text = item.FormHead;
      f.FormText.text = item.FormText;

      f.transform.position = new Vector3(item.PosX, item.PosY, 0.0f);

      Overseer.Instance.FormsQueueRef.AddForm(f);

      _allForms.Add(f);
    }

    for (int i = _dataToSave.FormsInStack.Count - 1; i >= 0; i--)
    {
      go = Instantiate(FormPrefab, Vector3.zero, Quaternion.identity, CanvasTransform.transform);
      go.SetActive(false);

      f = go.GetComponent<Form>();

      f.FormHead.text = _dataToSave.FormsInStack[i].FormHead;
      f.FormText.text = _dataToSave.FormsInStack[i].FormText;

      f.transform.position = new Vector3(_dataToSave.FormsInStack[i].PosX, _dataToSave.FormsInStack[i].PosY, 0.0f);

      Overseer.Instance.FormsStackRef.AddForm(f);

      _allForms.Add(f);
    }
  }

  public void ExitAppHandler()
  {
    Application.Quit();
  }
}
