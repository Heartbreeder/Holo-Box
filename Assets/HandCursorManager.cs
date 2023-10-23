using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class HandCursorManager : MonoBehaviour
{

    public GameObject LeftIndexCursor, RightIndexCursor;

    private Vector3 LeftIndexPos;
    private Vector3 RightIndexPos;

    private MLHandTracking.HandKeyPose[] _gestures;

    // Start is called before the first frame update
    void Start()
    {
        MLHandTracking.Start();
        _gestures = new MLHandTracking.HandKeyPose[5];
        _gestures[0] = MLHandTracking.HandKeyPose.Ok;
        _gestures[1] = MLHandTracking.HandKeyPose.Finger;
        _gestures[2] = MLHandTracking.HandKeyPose.OpenHand;
        _gestures[3] = MLHandTracking.HandKeyPose.Fist;
        _gestures[4] = MLHandTracking.HandKeyPose.Thumb;
        MLHandTracking.KeyPoseManager.EnableKeyPoses(_gestures, true, false);
    }

    private void OnDestroy()
    {
        MLHandTracking.Stop();
    }

    private void ShowPointsLeft()
    {
        LeftIndexPos = MLHandTracking.Left.Index.KeyPoints[2].Position;
        LeftIndexCursor.transform.position = LeftIndexPos;
    }

    private void ShowPointsRight()
    {
        RightIndexPos = MLHandTracking.Right.Index.KeyPoints[2].Position;
        RightIndexCursor.transform.position = RightIndexPos;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if ( GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Finger))
        {
            LeftIndexCursor.SetActive(true);
            ShowPointsLeft();
        }
        else
        {
            LeftIndexCursor.SetActive(false);
        }

        if (GetGesture(MLHandTracking.Right, MLHandTracking.HandKeyPose.Finger))
        {
            RightIndexCursor.SetActive(true);
            ShowPointsRight();
        }
        else
        {
            RightIndexCursor.SetActive(false);
        }*/
        if (MLHandTracking.Left.IsVisible)
        {
            LeftIndexCursor.SetActive(true);
            ShowPointsLeft();
        }
        else
        {
            LeftIndexCursor.SetActive(false);
        }

        if (MLHandTracking.Right.IsVisible)
        {
            RightIndexCursor.SetActive(true);
            ShowPointsRight();
        }
        else
        {
            RightIndexCursor.SetActive(false);
        }

    }

    private bool GetGesture(MLHandTracking.Hand hand, MLHandTracking.HandKeyPose type)
    {
        if (hand != null)
        {
            if (hand.KeyPose == type)
            {
                if (hand.HandKeyPoseConfidence > 0.9f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
