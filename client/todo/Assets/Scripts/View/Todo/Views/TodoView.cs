using System.Collections;
using System.Collections.Generic;
using Todo.Entity;
using Todo.Screen.ViewModels;
using UnityEngine;

namespace Todo.Screen.Views
{
    public class TodoView : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_scrollContent;
        [SerializeField]
        private RectTransform m_scrollElement;

        public void Show(List<TodoItemViewModel> todoItems)
        {
            foreach(var todoItem in todoItems)
            {
                var listElement = Instantiate(m_scrollElement, Vector3.zero, Quaternion.identity);
                listElement.SetParent(m_scrollContent.transform);
                var elementView = listElement.GetComponent<TodoItemView>();
                elementView.ShowElement(todoItem);
            }
        }

        public void OnClickAddButton()
        {
            Debug.Log("OnClickAddButton");
        }
    }
}
