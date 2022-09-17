using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickOnGroundPiece : MonoBehaviour
{
    void OnMouseDown()
    {
        if (this.tag == "Empty")
        {
            this.tag = "Selected";
        }
    }
}
