using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Todo.Views.Todo.ViewModels
{
    public class TodoDialogViewModel
    {
        public enum DialogViewMode
        {
            Show,
            Add,
            Delete,
        }

        public DialogViewMode TodoDialogViewMode { get; set; }
        public long Id { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; }
    }
}