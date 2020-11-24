using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplainUi : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            Destroy(gameObject, 0);
        }
    }
}
