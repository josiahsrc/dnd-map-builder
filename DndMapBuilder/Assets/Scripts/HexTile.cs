using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
  public GameObject slot;
  public GameObject tile = null;
  public Material hoverMaterial;
  public Material defaultMaterial;
  private Material overrideMaterial;

  public bool IsOccupied => tile != null;

  private void Start()
  {
    SetIsHovered(false);
  }

  public void Clear()
  {
    overrideMaterial = null;
    if (tile != null)
    {
      Destroy(tile);
    }
  }

  public void Edit(GameObject prefab, Material material)
  {
    Clear();
    tile = Instantiate(prefab, transform.position, Quaternion.identity, transform);
    SetIsHovered(false);
    overrideMaterial = material;
    foreach (var meshRenderer in GetMeshRenderers())
    {
      meshRenderer.material = material;
    }
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
      mat = overrideMaterial != null ? overrideMaterial : defaultMaterial;

    foreach (var meshRenderer in GetMeshRenderers())
    {
      meshRenderer.material = mat;
    }
  }
}
