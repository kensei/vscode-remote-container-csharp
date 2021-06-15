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
        private Func<TodoItemViewModel, IEnumerator> m_updateElementHandler;
        private Func<TodoItemViewModel, IEnumerator> m_deleteElementHandler;

        public IEnumerator ShowElement(TodoItemViewModel todoItem, Func<TodoItemViewModel, IEnumerator> updateElementHandler, Func<TodoItemViewModel, IEnumerator> deleteElementHandler)
        {
            m_viewModel = todoItem;
            m_updateElementHandler = updateElementHandler;
            m_deleteElementHandler = deleteElementHandler;

            m_title.text = todoItem.Title;
            m_isComplete.isOn = todoItem.IsComplete;

            yield return null;
        }

        public IEnumerator UpdateElement(TodoItemViewModel todoItem)
        {
            m_viewModel = todoItem;
            m_title.text = todoItem.Title;
            m_isComplete.isOn = todoItem.IsComplete;

            yield return null;
        }

        public IEnumerator DeleteElement()
        {
            m_updateElementHandler = null;
            m_deleteElementHandler = null;
            Destroy(this.gameObject);

            yield return null;
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
                StartCoroutine(m_deleteElementHandler(m_viewModel));
            }
        }

        public void OnClickEditButton()
        {
            Debug.Log("OnClickEditButton");
            if (m_updateElementHandler != null)
            {
                StartCoroutine(m_updateElementHandler(m_viewModel));
            }
        }
    }
}
