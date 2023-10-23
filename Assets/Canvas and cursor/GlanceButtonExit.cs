using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlanceButtonExit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Entered on trigger");
        if (other.tag == "Player")
        {
            transform.parent.SendMessage("DeselectButton");

        }
    }
}
