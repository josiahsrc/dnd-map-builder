using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{
  public GameObject cellPrefab; // The hex cell prefab
  private Dictionary<Vector3Int, HexTile> cells = new Dictionary<Vector3Int, HexTile>();
  public int radius = 10; // Radius of the hexagonal grid

  // Hex grid layout constants
  private const float hexWidth = 1.732f; // 2 * sin(60°)
  private const float hexHeight = 2f;
  private const float horizontalSpacing = hexWidth;
  private const float verticalSpacing = hexHeight * 0.75f;

  void Start()
  {
    GenerateMap();
  }

  // Method to generate the hex map
  private void GenerateMap()
  {
    for (int q = -radius; q <= radius; q++)
    {
      int r1 = Mathf.Max(-radius, -q - radius);
      int r2 = Mathf.Min(radius, -q + radius);
      for (int r = r1; r <= r2; r++)
      {
        Vector3Int hexCoord = new Vector3Int(q, -q - r, r);
        PlaceHexTile(hexCoord);
      }
    }
  }

  // Method to place a hex tile at a given hex coordinate
  private void PlaceHexTile(Vector3Int hexCoord)
  {
    Vector3 worldPos = HexToWorldPosition(hexCoord);
    GameObject newCell = Instantiate(cellPrefab, worldPos, Quaternion.identity, transform);
    cells[hexCoord] = newCell.GetComponent<HexTile>();
  }

  // Convert hex coordinates to world position
  private Vector3 HexToWorldPosition(Vector3Int hexCoord)
  {
    float x = (hexCoord.x + hexCoord.z * 0.5f) * horizontalSpacing;
    float z = hexCoord.z * verticalSpacing;

    return new Vector3(x, 0, z);
  }

  // Convert world position to nearest hex coordinate
  public Vector3Int WorldToHexPosition(Vector3 worldPosition)
  {
    float q = (worldPosition.x / horizontalSpacing - worldPosition.z / verticalSpacing / 2f);
    float r = worldPosition.z / verticalSpacing;

    return CubeRound(new Vector3(q, -q - r, r));
  }

  // Helper method for rounding cube coordinates
  private Vector3Int CubeRound(Vector3 cube)
  {
    float rx = Mathf.Round(cube.x);
    float ry = Mathf.Round(cube.y);
    float rz = Mathf.Round(cube.z);

    float xDiff = Mathf.Abs(rx - cube.x);
    float yDiff = Mathf.Abs(ry - cube.y);
    float zDiff = Mathf.Abs(rz - cube.z);

    if (xDiff > yDiff && xDiff > zDiff)
      rx = -ry - rz;
    else if (yDiff > zDiff)
      ry = -rx - rz;
    else
      rz = -rx - ry;

    return new Vector3Int(Mathf.RoundToInt(rx), Mathf.RoundToInt(ry), Mathf.RoundToInt(rz));
  }

  // Method to get the hex tile at a given coordinate (if it exists)
  public HexTile GetHexTileAt(Vector3Int hexCoord)
  {
    if (cells.TryGetValue(hexCoord, out HexTile cell))
    {
      return cell;
    }
    return null;
  }
}