using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CursorChaseLogic : MonoBehaviour
{
    public GameObject TargetObject;
    public float LerpFactor = 0.8f;
    public float SpeedFactor = 0.8f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //transform.localPosition = new Vector3(Mathf.Lerp(transform.localPosition.x, TargetObject.transform.localPosition.x, LerpFactor*Time.deltaTime), Mathf.Lerp(transform.localPosition.y, TargetObject.transform.localPosition.y, LerpFactor*Time.deltaTime), 0);

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, TargetObject.transform.localPosition, SpeedFactor * Time.deltaTime);


            
            //Mathf.Lerp(transform.position, TargetObject.transform.position, 0.6f);

    }
}
