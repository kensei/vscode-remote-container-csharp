using System.Collections;
using System.Collections.Generic;
using Todo.Repositories;
using Todo.Screen.Views;
using UnityEngine;
using UnityEngine.UI;

namespace Todo.Screen.Controllers
{
    public class TodoController : MonoBehaviour
    {
        TodoView m_view;
        TodoPresenter m_presenter;

        private void Awake()
        {
            m_view = this.GetComponent<TodoView>();
            var repository = new TodoRepository();
            m_presenter = new TodoPresenter(m_view, repository);
        }

        void Start()
        {
            m_presenter.Start();
        }
    }
}