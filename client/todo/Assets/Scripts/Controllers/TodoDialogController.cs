using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TodoDialogController : MonoBehaviour
{
    public void OnClickOkButton()
    {
        Debug.Log("OnClickOkButton");
        Destroy(this.gameObject);
    }

    public void OnClickCancelButton()
    {
        Debug.Log("OnClickCancelButton");
        Destroy(this.gameObject);
    }
}
