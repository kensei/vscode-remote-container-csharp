using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Todo.Views.Todo.ViewModels
{
    public class TodoItemViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; }
    }
}
