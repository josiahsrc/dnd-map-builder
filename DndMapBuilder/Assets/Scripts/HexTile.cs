using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
  public GameObject slot;
  public Material hoverMaterial;
  public Material defaultMaterial;

  public int Count => count;
  public GameObject Prefab => prefab;
  public Material Material => material;

  private int count = 0;
  private GameObject prefab;
  private Material material;
  private List<GameObject> tiles = new List<GameObject>();

  public bool IsOccupied => prefab != null && material != null;

  private void Start()
  {
    SetIsHovered(false);
  }

  private void DeleteObjects()
  {
    foreach (var tile in tiles)
      Destroy(tile);
    tiles.Clear();
  }

  private void CreateObjects()
  {
    var bounds = prefab.GetComponent<MeshRenderer>().bounds;
    var height = bounds.size.y;
    for (var i = 0; i < count; i++)
    {
      var tile = Instantiate(prefab, transform.position + new Vector3(0, height * i, 0), Quaternion.identity, transform);
      tiles.Add(tile);
    }

    foreach (var meshRenderer in GetMeshRenderers())
    {
      meshRenderer.sharedMaterial = material;
    }
  }

  public void Clear()
  {
    count = 0;
    prefab = null;
    material = null;
    DeleteObjects();
  }

  public void Edit(GameObject prefab, Material material)
  {
    Clear();
    count = 1;
    this.prefab = prefab;
    this.material = material;
    CreateObjects();
    SetIsHovered(false);
  }

  public void SetCount(int count)
  {
    if (!IsOccupied)
      return;

    this.count = count;
    DeleteObjects();
    CreateObjects();
  }

  private IEnumerable<MeshRenderer> GetMeshRenderers()
  {
    return GetComponentsInChildren<MeshRenderer>();
  }

  public void SetIsHovered(bool isHovered)
  {
    Material mat;
    if (isHovered)
      mat = hoverMaterial;
    else
      mat = material ? material : defaultMaterial;

    foreach (var meshRenderer in GetMeshRenderers())
    {
      meshRenderer.material = mat;
    }
  }
}
