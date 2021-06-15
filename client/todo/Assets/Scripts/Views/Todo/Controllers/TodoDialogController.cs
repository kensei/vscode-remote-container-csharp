using System;
using System.Collections;
using System.Collections.Generic;
using Todo.Views.Todo.ViewModels;
using Todo.Views.Todo.Views;
using UnityEngine;

namespace Todo.Views.Todo.Controllers
{
    public class TodoDialogController : MonoBehaviour
    {
        private TodoDialogView m_view;

        public IEnumerator Show(TodoDialogViewModel todoDialogViewModel, Func<TodoDialogViewModel, IEnumerator> dialogOkHandler, Action dialogCancelHandler)
        {
            m_view = this.GetComponent<TodoDialogView>();
            yield return m_view.Show(todoDialogViewModel, dialogOkHandler, dialogCancelHandler);
        }
    }
}
