using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using UnityEngine.UI;

public class EyeTracking2DManager : MonoBehaviour
{
    public GameObject MainCamera;

    public GameObject TDMarker;

    public GameObject ChaseCursorObject;

    public GameObject ExtraCursor;

    public Color CursorIdleColor;
    public Color CursorBlinkingColor;

    private Vector3 viewpoint;
    private Vector3 viewpointStart;
    private bool isBlinking;
    // Start is called before the first frame update
    void Start()
    {
        MLEyes.Start();
        isBlinking = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * Throw rays and reposition cursors
         * */


        /*
        if (!MLEyes.LeftEye.IsBlinking)
            viewpointStart = MLEyes.LeftEye.Center;
        else
            viewpointStart = MLEyes.RightEye.Center;
        */
        ExtraCursor.transform.position = Camera.main.transform.position;


        //Cursor reposition Raycast. Hits only Layer 5(UI)
        RaycastHit rayHit;
        int layer = 1 << 5;

        viewpoint = MLEyes.FixationPoint;

        ExtraCursor.transform.LookAt(viewpoint);
        viewpointStart = ExtraCursor.transform.position;


        if (Physics.Raycast(viewpointStart, ExtraCursor.transform.forward, out rayHit, 1000.0f, layer))
        {
            transform.position = rayHit.point;
        }

        ExtraCursor.transform.LookAt(transform.position);
        //Capsule Cursor. Goes through follow cursor. Hits only Layer 9 (World UI)
        int layer2 = 1 << 9;
        ExtraCursor.transform.LookAt(ChaseCursorObject.transform.position);

        if (Physics.Raycast(viewpointStart, ExtraCursor.transform.forward, out rayHit, 1000.0f, layer2))
        {
            TDMarker.transform.position = rayHit.point;
            Debug.DrawLine(viewpointStart, rayHit.point, Color.red);
        }
        Debug.DrawLine(viewpointStart, viewpoint, Color.white);


        /* 
         * Click button via blinking. Do not use if clicking usning hands
         * */
        GameMaster gm = GameMaster.instance;
        if (!gm.isHandTriggered)
        {
            if ((MLEyes.LeftEye.IsBlinking && !MLEyes.RightEye.IsBlinking)
                || (!MLEyes.LeftEye.IsBlinking && MLEyes.RightEye.IsBlinking))
            {
                //if blinking was not on and became on in this frame start clicking
                if (!isBlinking)
                {
                    ChaseCursorObject.GetComponent<Image>().color = CursorBlinkingColor;
                    if (gm.SelectedButton != null)
                        gm.SelectedButton.DelayedClick();
                }
                isBlinking = true;
            }
            else
            {
                //if blinking was on and became off in this frame stop clicking
                if (isBlinking)
                {
                    ChaseCursorObject.GetComponent<Image>().color = CursorIdleColor;
                    if (gm.SelectedButton != null)
                        gm.SelectedButton.DelayedClickStop();
                }
                isBlinking = false;
            }
        }



    }

    private void OnApplicationQuit()
    {
        MLEyes.Stop();
    }
}
