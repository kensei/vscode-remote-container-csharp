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

        private List<TodoItemView> m_itemViewList;
        private IEnumerator m_addElementHandler;
        private Func<TodoItemViewModel, IEnumerator> m_updateElementHandler;
        private Func<TodoItemViewModel, IEnumerator> m_deleteElementHandler;

        private void Awake()
        {
            m_itemViewList = new List<TodoItemView>();
        }

        public void Show(List<TodoItemViewModel> todoItems, IEnumerator addElementHandler, Func<TodoItemViewModel, IEnumerator> updateElementHandler, Func<TodoItemViewModel, IEnumerator> deleteElementHandler)
        {
            m_addElementHandler = addElementHandler;

            foreach(var todoItem in todoItems)
            {
                var listElement = Instantiate(m_scrollElement, m_scrollContent.transform);
                var elementView = listElement.GetComponent<TodoItemView>();
                m_itemViewList.Add(elementView);
                elementView.ShowElement(todoItem, updateElementHandler, deleteElementHandler);
            }
        }

        public void OnClickAddButton()
        {
            Debug.Log("OnClickAddButton");
            if (m_addElementHandler != null)
            {
                StartCoroutine(m_addElementHandler);
            }
        }

        public void AddElement(TodoItemViewModel addTodoItem)
        {
            var listElement = Instantiate(m_scrollElement, m_scrollContent.transform);
            var elementView = listElement.GetComponent<TodoItemView>();
            m_itemViewList.Add(elementView);
            elementView.ShowElement(addTodoItem, m_updateElementHandler, m_deleteElementHandler);
        }

        public void UpdateElement(TodoItemViewModel updateTodoItem)
        {
            var updateTarget = m_itemViewList.Find(x => x.IsEqualId(updateTodoItem.Id));
            if (updateTarget != null)
            {
                updateTarget.UpdateElement(updateTodoItem);
            }
            else
            {
                Debug.LogError("update item not found : " + updateTodoItem.Id);
            }
        }

        public void DeleteElement(TodoItemViewModel deleteTodoItem)
        {
            Debug.Log("DeleteElement:" + deleteTodoItem.Id);
            var deleteTarget = m_itemViewList.Find(x => x.IsEqualId(deleteTodoItem.Id));
            if (deleteTarget != null)
            {
                deleteTarget.DeleteElement();
            }
            else
            {
                Debug.LogError("update item not found : " + deleteTodoItem.Id);
            }
        }
    }
}
