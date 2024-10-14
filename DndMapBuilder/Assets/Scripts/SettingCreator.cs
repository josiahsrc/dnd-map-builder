using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingCreator : MonoBehaviour
{
  public Config config;
  public SettingType type;
  public GameObject settingPrefab;

  void Start()
  {
    if (type == SettingType.tile)
    {
      foreach (var tile in config.tiles)
      {
        var setting = Instantiate(settingPrefab, transform).GetComponent<Setting>();
        setting.prefab = tile.prefab;
        setting.prefabImage = tile.image;
      }
    }
    else
    {
      foreach (var color in config.colors)
      {
        var setting = Instantiate(settingPrefab, transform).GetComponent<Setting>();
        setting.material = color.material;
      }
    }
  }

  public enum SettingType
  {
    tile,
    color
  }
}
