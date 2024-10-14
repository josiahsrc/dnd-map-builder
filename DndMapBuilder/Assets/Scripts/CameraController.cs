using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  public static float SensitivityMult = 1.0f;

  public float orbitSpeed = 10f;
  public float moveSpeed = 10f;
  public float sensitivity = 10f;
  public float zoomSpeed = 5f; // Speed of zooming
  public Vector3 target = new Vector3(0, 0, 0);
  public Vector2 currPos = new Vector2(0, 0);
  public float minZoomDistance = 2f; // Minimum distance to zoom in
  public float maxZoomDistance = 50f; // Maximum distance to zoom out

  private Vector3 lastMousePosition;

  void Start()
  {
    lastMousePosition = Input.mousePosition;
    currPos = new Vector2(transform.position.x, transform.position.z);
  }

  void Update()
  {
    OrbitAroundTarget();
    Zoom();
    currPos = new Vector2(transform.position.x, transform.position.z);

    MoveAlongPlane();
    transform.position = new Vector3(currPos.x, transform.position.y, currPos.y);
  }

  void OrbitAroundTarget()
  {
    var isLeftMouseAndMeta = Input.GetMouseButton(0) && ControlManager.Instance.IsMeta();
    var isRightMouse = Input.GetMouseButton(1);
    if (isLeftMouseAndMeta || isRightMouse)
    {
      Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
      float angleX = mouseDelta.x * orbitSpeed * Time.deltaTime * sensitivity * SensitivityMult;
      float angleY = mouseDelta.y * orbitSpeed * Time.deltaTime * sensitivity * SensitivityMult;

      transform.RotateAround(target, Vector3.up, angleX);
      transform.RotateAround(target, transform.right, -angleY);
    }

    // Update last mouse position for next frame
    lastMousePosition = Input.mousePosition;
  }

  void MoveAlongPlane()
  {
    Vector3 move = new Vector3();

    if (Input.GetKey(KeyCode.W))
      move += transform.forward;
    if (Input.GetKey(KeyCode.S))
      move -= transform.forward;
    if (Input.GetKey(KeyCode.A))
      move -= transform.right;
    if (Input.GetKey(KeyCode.D))
      move += transform.right;

    currPos += new Vector2(move.x, move.z) * moveSpeed * Time.deltaTime;
    target += new Vector3(move.x, 0, move.z) * moveSpeed * Time.deltaTime;
  }

  void Zoom()
  {
    float scroll = Input.GetAxis("Mouse ScrollWheel");
    if (scroll != 0.0f)
    {
      // Get the direction from the camera to the target
      Vector3 direction = transform.position - target;
      float distance = direction.magnitude;

      // Adjust the distance based on scroll input
      distance -= scroll * zoomSpeed;
      distance = Mathf.Clamp(distance, minZoomDistance, maxZoomDistance);

      // Update the camera position based on the new distance
      transform.position = target + direction.normalized * distance;
    }
  }
}
