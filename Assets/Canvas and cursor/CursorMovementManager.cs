using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using UnityEngine.UI;

public class CursorMovementManager : MonoBehaviour
{
    public float CursorSensitivity = 1;

    private bool isControllerTouched;
    private Vector3 ControllerTouchStart;
    private MLInput.Controller Controller;

    private EyeTracking2DManager eyeTrackScript;

    // Start is called before the first frame update
    void Start()
    {
        MLInput.Start();
        Controller = MLInput.GetController(MLInput.Hand.Left);
        isControllerTouched = false;
        eyeTrackScript = transform.GetComponent<EyeTracking2DManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Controller.Touch1Active)
        {
            if (!isControllerTouched)
            {
                isControllerTouched = true;
                ControllerTouchStart = Controller.Touch1PosAndForce;
                eyeTrackScript.enabled = false;
            }
            else
            {
                float hor = Controller.Touch1PosAndForce.x - ControllerTouchStart.x;
                float ver = Controller.Touch1PosAndForce.y - ControllerTouchStart.y;
                Vector3 displacement = new Vector3(hor, ver, 0);
                Vector3 newpos = new Vector3(Mathf.Clamp(transform.localPosition.x + (displacement.x * CursorSensitivity), -25, 25), Mathf.Clamp(transform.localPosition.y + (displacement.y * CursorSensitivity), -17.5f, 17.5f), 0);
                //Debug.Log(newpos);
                transform.localPosition = newpos;

            }
        }
        else
        {
            if (isControllerTouched)
            {
                eyeTrackScript.enabled = true;
                isControllerTouched = false;
            }

            isControllerTouched = false;

        }
    }

    private void OnApplicationQuit()
    {
        MLInput.Stop();
    }
}
