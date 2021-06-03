using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Todo.Screen.Views
{
    public class TodoDialogView : MonoBehaviour
    {
        [SerializeField]
        private Text m_title;
        [SerializeField]
        private Toggle m_isComplete;

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
