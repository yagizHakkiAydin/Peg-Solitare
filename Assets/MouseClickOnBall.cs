using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickOnBall : MonoBehaviour
{

    void OnMouseDown()
    {
        this.tag = "Selected";
    }
}
