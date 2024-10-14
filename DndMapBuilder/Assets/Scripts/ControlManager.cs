using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlManager : MonoBehaviour
{
  public static ControlManager Instance { get; private set; }

  public bool IsOverUI { get; private set; }
  public bool IsFocusedUI { get; private set; }
  public LayerMask UILayer;

  public bool IsMeta() => Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftControl);

  void Awake()
  {
    Instance = this;
  }

  void Update()
  {
    IsFocusedUI = EventSystem.current.currentSelectedGameObject != null;
    IsOverUI = EventSystem.current.IsPointerOverGameObject();
  }
}
