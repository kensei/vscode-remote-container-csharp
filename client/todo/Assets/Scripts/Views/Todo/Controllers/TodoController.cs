using System.Collections;
using System.Collections.Generic;
using Todo.Repositories;
using Todo.Views.Todo.Views;
using UnityEngine;
using UnityEngine.UI;

namespace Todo.Views.Todo.Controllers
{
    public class TodoController : MonoBehaviour
    {
        [SerializeField]
        private RectTransform m_todoDialog;
        [SerializeField]
        private Transform m_uiRoot;

        private TodoView m_view;
        private TodoDialogController m_dialogController;
        private TodoPresenter m_presenter;

        private void Awake()
        {
            m_view = this.GetComponent<TodoView>();
            m_dialogController = InitTodoDialog();
            var repository = new TodoRepository();
            m_presenter = new TodoPresenter(m_view, m_dialogController, repository);
        }

        void Start()
        {
            m_presenter.Start();
        }

        private TodoDialogController InitTodoDialog()
        {
            var todoDialog = Instantiate(m_todoDialog, m_uiRoot, false);
            return todoDialog.GetComponent<TodoDialogController>();
        }
    }
}