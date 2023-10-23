using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class ControllerHolobox : MonoBehaviour
{
    public GameObject CameraObject;
    public GlanceableUILogic GlanceableLogic;
    public Vector3 HoloBoxSpawnOffset;
    public Vector3 TabletopSpawnOfsset;

    public bool SpawnTabletop;
    public bool SpawnHolobox;


    private MLInput.Controller controller;
    private GameObject MainCamera;
    private CapsuleCanvasMovement capsuleMove;
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = GameMaster.instance.MainCamera;
        capsuleMove = GameMaster.instance.CapsuleMovementScript;
        MLInput.Start();
        controller = MLInput.GetController(MLInput.Hand.Left);
        MLInput.OnControllerButtonDown += OnButtonDown;

        SpawnTabletop = false;
        SpawnHolobox = false;

    }

    // Update is called once per frame
    void Update()
    {
        CheckTrigger();
    }

    void OnButtonDown(byte controllerId, MLInput.Controller.Button button)
    {
        //Debug.Log("Button pressed");
        if(button == MLInput.Controller.Button.Bumper)
        {

            if (SpawnHolobox)
            {
                GameObject box = Instantiate(GlanceableLogic.HoloBoxPrefab, (transform.position + transform.forward * HoloBoxSpawnOffset.z + transform.right * HoloBoxSpawnOffset.x + transform.up * HoloBoxSpawnOffset.y), transform.rotation);
                //box.transform.LookAt(CameraObject.transform);
                //box.transform.rotation = Quaternion.Euler(0, box.transform.rotation.y, 0);
                GlanceableLogic.SetHoloBox(box);
                box.transform.GetChild(0).gameObject.SetActive(false);
            }
            else if (SpawnTabletop)
            {
                GameObject box = Instantiate(GlanceableLogic.TabletopPrefab, (transform.position + transform.forward * TabletopSpawnOfsset.z + transform.right * TabletopSpawnOfsset.x + transform.up * TabletopSpawnOfsset.y), transform.rotation);
                GlanceableLogic.SetTabletop(box);
            }

        }
    }

    void CheckTrigger()
    {
        if (controller.TriggerValue > 0.5f)
        {
            //Debug.Log(MainCamera.transform.rotation.y + " " + MainCamera.transform.localRotation.y);
            capsuleMove.SetGraphicsAngle(MainCamera.transform.eulerAngles.y);
        }

    }


    private void OnDestroy()
    {
        MLInput.Stop();
    }
}
