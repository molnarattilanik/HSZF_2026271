namespace TodoManager
{
    internal class TodoManager
    {
        List<Todo> todos = new List<Todo>();

        private int nextId = 1;

        public void AddTodo(string name)
        {
            Todo todo = new Todo { Id = nextId++, Name = name };
            todos.Add(todo);
        }

        public List<Todo> GetTodos()
        {
            return todos;
        }

        public void RemoveTodo(int id)
        {
            todos.RemoveAll(t => t.Id == id);
        }
    }
}
