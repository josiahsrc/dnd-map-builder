using System.Collections.Generic;
using UnityEngine;

public class HexMapController : MonoBehaviour
{
  public static GameObject currentPrefab;
  public static Material currentMaterial;
  public static Sprite currentIcon;

  public HexMap hexMap;
  private Camera mainCamera;
  private HexTile lastHoveredTile = null;
  public GameObject initialPrefab;
  public Material initialMaterial;
  public Sprite initialIcon;

  void Start()
  {
    currentPrefab = initialPrefab;
    currentMaterial = initialMaterial;
    currentIcon = initialIcon;

    // Get the main camera
    mainCamera = Camera.main;

    // Ensure we have a reference to the HexMap
    if (hexMap == null)
    {
      hexMap = FindObjectOfType<HexMap>();
      if (hexMap == null)
      {
        Debug.LogError("No HexMap found in the scene. Please assign it in the inspector or add a HexMap to the scene.");
      }
    }
  }

  void Update()
  {
    HexTile tile = null;
    if (!ControlManager.Instance.IsOverUI)
    {
      tile = GetHexTile();
    }

    if (Input.GetMouseButton(0) && !ControlManager.Instance.IsMeta() && tile != null)
    {
      tile.Edit(currentPrefab, currentMaterial);
    }
    if (Input.GetKeyDown(KeyCode.I) && tile != null)
    {
      tile.EditIcon(currentIcon);
    }
    if (Input.GetKey(KeyCode.X) && tile != null)
    {
      tile.Clear();
    }

    if (tile != null)
    {
      if (Input.GetKey(KeyCode.Alpha1))
        tile.SetCount(1);
      if (Input.GetKey(KeyCode.Alpha2))
        tile.SetCount(2);
      if (Input.GetKey(KeyCode.Alpha3))
        tile.SetCount(3);
      if (Input.GetKey(KeyCode.Alpha4))
        tile.SetCount(4);
      if (Input.GetKey(KeyCode.Alpha5))
        tile.SetCount(5);
    }

    if (tile != lastHoveredTile)
    {
      if (lastHoveredTile != null)
        lastHoveredTile.SetIsHovered(false);
      if (tile != null)
        tile.SetIsHovered(true);
    }

    lastHoveredTile = tile;
  }

  private HexTile GetHexTile()
  {
    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;

    if (!Physics.Raycast(ray, out hit))
      return null;

    var root = hit.collider.transform.root;
    var rootTile = root.GetComponent<HexTile>();
    if (rootTile != null)
      return rootTile;

    // Convert world position to hex coordinate
    Vector3Int hexCoord = hexMap.WorldToHexPosition(hit.point);

    // Check if a tile already exists at this coordinate
    HexTile existingTile = hexMap.GetHexTileAt(hexCoord);
    return existingTile;
  }
}