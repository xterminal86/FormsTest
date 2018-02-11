using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormsStack : FormsContainer 
{ 
  void Start()
  {
    Container = new Stack<Form>();

    Init();
  }

  public void MouseClick()
  {
    if (Container.Count == 0)
    {
      Overseer.Instance.FlashText("No forms in stack!", 1.0f);
    }
    else
    {
      GetForm();
    }
  }

  public void AddForm(Form f)
  {    
    (Container as Stack<Form>).Push(f);

    RefreshFillMeter();
  }

  void GetForm()
  {
    Form f = (Container as Stack<Form>).Pop();

    RestoreForm(f);
  }
}
