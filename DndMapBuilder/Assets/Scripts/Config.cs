using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Config")]
public class Config : ScriptableObject
{
  public List<Tile> tiles;
  public List<Color> colors;
  public List<Sprite> icons;

  private Dictionary<string, Tile> tileDict = null;
  private Dictionary<string, Color> colorDict = null;
  private Dictionary<string, Sprite> iconDict = null;

  public Tile GetTile(string id)
  {
    if (tileDict == null)
    {
      tileDict = new Dictionary<string, Tile>();
      foreach (var tile in tiles)
        tileDict[tile.id] = tile;
    }

    return tileDict[id];
  }

  public Color GetColor(string id)
  {
    if (colorDict == null)
    {
      colorDict = new Dictionary<string, Color>();
      foreach (var color in colors)
        colorDict[color.id] = color;
    }

    return colorDict[id];
  }

  public Sprite GetIcon(string id)
  {
    if (iconDict == null)
    {
      iconDict = new Dictionary<string, Sprite>();
      foreach (var icon in icons)
        iconDict[GetIconId(icon)] = icon;
    }

    return iconDict.GetValueOrDefault(id, null);
  }

  public static string GetTileId(GameObject prefab)
  {
    return prefab.name;
  }

  public static string GetColorId(Material material)
  {
    return material.name;
  }

  public static string GetIconId(Sprite sprite)
  {
    return sprite.name;
  }

  [System.Serializable]
  public class Tile
  {
    public GameObject prefab;
    public Sprite image;

    public string id => GetTileId(prefab);
  }

  [System.Serializable]
  public class Color
  {
    public Material material;

    public string id => GetColorId(material);
  }
}
