using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormsQueue : FormsContainer 
{  
  void Start()
  {
    Container = new Queue<Form>();

    Init();
  }

  public void MouseClick()
  {
    if (Container.Count == 0)
    {
      Overseer.Instance.FlashText("No forms in queue!", 1.0f);
    }
    else
    {
      GetForm();
    }
  }

  public void AddForm(Form f)
  {    
    (Container as Queue<Form>).Enqueue(f);

    RefreshFillMeter();
  }

  void GetForm()
  {
    Form f = (Container as Queue<Form>).Dequeue();

    RestoreForm(f);
  }
}
