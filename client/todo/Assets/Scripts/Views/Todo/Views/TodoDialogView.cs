using System;
using System.Collections;
using System.Collections.Generic;
using Todo.Views.Todo.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace Todo.Views.Todo.Views
{
    public class TodoDialogView : MonoBehaviour
    {
        [SerializeField]
        private InputField m_title;
        [SerializeField]
        private Toggle m_isComplete;

        private TodoDialogViewModel m_oridinalTodoItem;
        private Func<TodoDialogViewModel, IEnumerator> m_dialogOkHandler;
        private Action m_dialogCancelHandler;

        public IEnumerator Show(TodoDialogViewModel todoDialogViewModel, Func<TodoDialogViewModel, IEnumerator> dialogOkHandler, Action dialogCancelHandler)
        {
            Debug.Log("TodoDialogView.Show:" + todoDialogViewModel.Id);
            m_dialogOkHandler = dialogOkHandler;
            m_dialogCancelHandler = dialogCancelHandler;
            m_oridinalTodoItem = todoDialogViewModel;

            m_title.text = todoDialogViewModel.Title;
            m_isComplete.isOn = todoDialogViewModel.IsComplete;

            this.gameObject.SetActive(true);

            yield return null;
        }

        public IEnumerator Test(int i)
        {
            yield return null;
        }

        public void OnClickOkButton()
        {
            Debug.Log("OnClickOkButton");
            var result = new TodoDialogViewModel { Id = m_oridinalTodoItem.Id, Title = m_title.text, IsComplete = m_isComplete.isOn };

            StartCoroutine(OnClickOk(result));
        }

        public void OnClickCancelButton()
        {
            Debug.Log("OnClickCancelButton");
            m_dialogCancelHandler();
            this.gameObject.SetActive(false);
        }

        private IEnumerator OnClickOk(TodoDialogViewModel result)
        {
            yield return StartCoroutine(m_dialogOkHandler(result));

            this.gameObject.SetActive(false);
        }
    }
}
