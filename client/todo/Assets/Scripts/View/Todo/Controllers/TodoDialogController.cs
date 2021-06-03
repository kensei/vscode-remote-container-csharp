using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Todo.Screen.Controllers
{
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
}
