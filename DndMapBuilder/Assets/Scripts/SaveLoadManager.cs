using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class SaveData
{
  public string name;
  public MapData mapData;
}

[Serializable]
public class MapData
{
  public List<string> ids = new List<string>();
  public List<TileData> tiles = new List<TileData>();

  public static string ToKey(int x, int z)
  {
    return $"{x}_{z}";
  }

  public static Vector2Int ToCoords(string key)
  {
    var parts = key.Split('_');
    return new Vector2Int(int.Parse(parts[0]), int.Parse(parts[1]));
  }
}

[Serializable]
public class TileData
{
  public string tileId;
  public string colorId;
  public int count;
}

public class SaveLoadManager : MonoBehaviour
{
  public static SaveLoadManager instance { get; private set; }

  public HexMap hexMap;
  public TextMeshProUGUI nameText;
  public GameObject loadablePrefab;
  public Transform loadablesParent;

  private string currentId = null;
  public string CurrentId => currentId;

  private Dictionary<string, string> idToName = new Dictionary<string, string>();
  private Dictionary<string, Loadable> idToLoadable = new Dictionary<string, Loadable>();

  public string GetName(string id)
  {
    if (idToName.ContainsKey(id))
    {
      return idToName[id];
    }

    var saveData = GetSaveData(id);
    idToName[id] = saveData.name;
    return saveData.name;
  }

  void Awake()
  {
    instance = this;
  }

  void Start()
  {
    InitializeLoadables();
  }

  void InitializeLoadables()
  {
    // clear existing loadables
    foreach (Transform child in loadablesParent)
    {
      Destroy(child.gameObject);
    }

    var ids = GetIds();
    foreach (var id in ids)
    {
      var loadable = Instantiate(loadablePrefab, loadablesParent).GetComponent<Loadable>();
      loadable.Init(id);
      idToLoadable[id] = loadable;
    }
  }

  List<string> GetIds()
  {
    var value = PlayerPrefs.GetString("ids", "");
    if (value == "")
      return new List<string>();
    var ids = new List<string>();
    value.Split(',').ToList().ForEach(id => ids.Add(id));
    return ids;
  }

  public void OnChangeName(string name)
  {
    if (currentId == null)
      return;
    idToName[currentId] = name;
  }

  void SetIds(List<string> ids)
  {
    PlayerPrefs.SetString("ids", string.Join(",", ids));
  }

  public void Delete(string id)
  {
    var ids = GetIds();
    if (!ids.Contains(id))
    {
      return;
    }

    ids.Remove(id);
    SetIds(ids);
    PlayerPrefs.DeleteKey(id);

    var loadable = idToLoadable[id];
    idToLoadable.Remove(id);
    Destroy(loadable.gameObject);

    if (currentId == id)
    {
      New();
    }

    InitializeLoadables();
  }

  public void New()
  {
    currentId = null;
    hexMap.LoadMap();
    nameText.text = "New Map";
  }

  public void Save()
  {
    if (currentId == null)
      currentId = Guid.NewGuid().ToString();

    var saveData = GetSaveData(currentId);
    saveData.mapData = hexMap.BuildMapData();
    saveData.name = nameText.text;
    PlayerPrefs.SetString(currentId, JsonUtility.ToJson(saveData));

    var ids = GetIds();
    if (!ids.Contains(currentId))
    {
      ids.Add(currentId);
      SetIds(ids);
    }

    InitializeLoadables();
  }

  public void Load(string id)
  {
    currentId = id;
    hexMap.LoadMap(GetSaveData(id).mapData);
    nameText.text = GetSaveData(currentId).name;
  }

  public static SaveData GetSaveData(string id)
  {
    var raw = PlayerPrefs.GetString(id, "");
    if (raw == "")
    {
      return new SaveData() { name = "New Map", mapData = new MapData() };
    }

    try
    {
      return JsonUtility.FromJson<SaveData>(raw);
    }
    catch
    {
      return new SaveData() { name = "New Map", mapData = new MapData() };
    }
  }
}
