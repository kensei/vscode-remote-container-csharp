using System.Collections;
using System.Collections.Generic;
using Todo.Screen.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace Todo.Screen.Views
{
    public class TodoItemView : MonoBehaviour
    {
        [SerializeField]
        private Text m_title;
        [SerializeField]
        private Toggle m_isComplete;

        public void ShowElement(TodoItemViewModel todoItem)
        {
            m_title.text = todoItem.Title;
            m_isComplete.isOn = todoItem.IsComplete;
        }

        public void OnClickDelButton()
        {
            Debug.Log("OnClickDelButton");
        }

        public void OnClickEditButton()
        {
            Debug.Log("OnClickEditButton");
        }
    }
}
