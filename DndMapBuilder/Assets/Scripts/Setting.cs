using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Setting : MonoBehaviour
{
  public Material material;
  public GameObject prefab;
  public GameObject outline;
  public Image image;

  void Update()
  {
    if (material != null)
    {
      image.color = material.color;
      outline.SetActive(material == HexMapController.currentMaterial);
    }

    if (prefab != null)
    {
      outline.SetActive(prefab == HexMapController.currentPrefab);
    }
  }

  public void Publish()
  {
    if (material != null)
      HexMapController.currentMaterial = material;
    if (prefab != null)
      HexMapController.currentPrefab = prefab;
  }
}
