using System;
using System.Collections;
using System.Collections.Generic;
using Todo.Entity;
using Todo.Views.Todo.ViewModels;
using UnityEngine;

namespace Todo.Views.Todo.Views
{
    public class TodoView : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_scrollContent;
        [SerializeField]
        private RectTransform m_scrollElement;

        private Action m_addElementHandler;

        public void Show(List<TodoItemViewModel> todoItems, Action addElementHandler, Action<TodoItemViewModel> updateElementHandler, Action<TodoItemViewModel> deleteElementHandler)
        {
            m_addElementHandler = addElementHandler;

            foreach(var todoItem in todoItems)
            {
                var listElement = Instantiate(m_scrollElement, m_scrollContent.transform);
                var elementView = listElement.GetComponent<TodoItemView>();
                elementView.ShowElement(todoItem, updateElementHandler, deleteElementHandler);
            }
        }

        public void OnClickAddButton()
        {
            Debug.Log("OnClickAddButton");
            if (m_addElementHandler != null)
            {
                m_addElementHandler();
            }
        }

        public void AddElement(TodoItemViewModel todoItem, Action<TodoItemViewModel> updateElementHandler, Action<TodoItemViewModel> deleteElementHandler)
        {
            var listElement = Instantiate(m_scrollElement, m_scrollContent.transform);
            var elementView = listElement.GetComponent<TodoItemView>();
            elementView.ShowElement(todoItem, updateElementHandler, deleteElementHandler);
        }
    }
}
