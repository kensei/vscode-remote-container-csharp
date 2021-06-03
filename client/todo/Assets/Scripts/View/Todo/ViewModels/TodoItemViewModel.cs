using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Todo.Screen.ViewModels
{
    public class TodoItemViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; }
    }
}
