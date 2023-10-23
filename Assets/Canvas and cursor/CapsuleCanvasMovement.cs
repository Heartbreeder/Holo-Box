using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleCanvasMovement : MonoBehaviour
{
    public GameObject CapsuleCenter;
    public GameObject GraphicsCanvas;

    public float GraphicsDistance = 1;
    public float GraphicsAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        ResetGraphicsPosition();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = CapsuleCenter.transform.position;
    }

    public void SetGraphicsAngle(float angle)
    {
        GraphicsAngle = angle;
        ResetGraphicsPosition();
    }

    public void SetGraphicsDistance(float distanceoffset)
    {
        GraphicsDistance += distanceoffset;
        ResetGraphicsPosition();
    }

    public void ResetGraphicsPosition()
    {
        Vector3 prevPos = GraphicsCanvas.transform.localPosition;
        GraphicsCanvas.transform.localPosition = new Vector3(prevPos.x, prevPos.y, GraphicsDistance);
        this.transform.localRotation = Quaternion.Euler(0, GraphicsAngle, 0);
    }
}
