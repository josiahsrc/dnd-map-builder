using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Loadable : MonoBehaviour
{
  public TextMeshProUGUI title;
  private string id;
  public Image image;

  public void Init(String id)
  {
    title.text = SaveLoadManager.instance.GetName(id);
    this.id = id;
  }

  public void OnLoad()
  {
    SaveLoadManager.instance.Load(id);
  }

  public void OnDelete()
  {
    SaveLoadManager.instance.Delete(id);
  }

  void Update()
  {
    image.color = id == SaveLoadManager.instance.CurrentId ? Color.green : Color.white;
  }
}
