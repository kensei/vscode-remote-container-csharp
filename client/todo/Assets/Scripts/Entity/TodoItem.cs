using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Todo.Entity
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; }
    }
}
