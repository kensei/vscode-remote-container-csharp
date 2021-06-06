using System;
using System.Collections;
using System.Collections.Generic;
using Todo.Views.Todo.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace Todo.Views.Todo.Views
{
    public class TodoItemView : MonoBehaviour
    {
        [SerializeField]
        private Text m_title;
        [SerializeField]
        private Toggle m_isComplete;

        private TodoItemViewModel m_viewModel;
        private Action<TodoItemViewModel> m_updateElementHandler;
        private Action<TodoItemViewModel> m_deleteElementHandler;

        public void ShowElement(TodoItemViewModel todoItem, Action<TodoItemViewModel> updateElementHandler, Action<TodoItemViewModel> deleteElementHandler)
        {
            m_viewModel = todoItem;
            m_updateElementHandler = updateElementHandler;
            m_deleteElementHandler = deleteElementHandler;

            m_title.text = todoItem.Title;
            m_isComplete.isOn = todoItem.IsComplete;
        }

        public void UpdateElement(TodoItemViewModel todoItem)
        {
            m_viewModel = todoItem;
            m_title.text = todoItem.Title;
            m_isComplete.isOn = todoItem.IsComplete;
        }

        public void DeleteElement()
        {
            m_updateElementHandler = null;
            m_deleteElementHandler = null;
            Destroy(this.gameObject);
        }

        public bool IsEqualId(long id)
        {
            return (m_viewModel.Id == id);
        }

        public void OnClickDelButton()
        {
            Debug.Log("OnClickDelButton");
            if (m_deleteElementHandler != null)
            {
                m_deleteElementHandler(m_viewModel);
            }
        }

        public void OnClickEditButton()
        {
            Debug.Log("OnClickEditButton");
            if (m_updateElementHandler != null)
            {
                m_updateElementHandler(m_viewModel);
            }
        }
    }
}
