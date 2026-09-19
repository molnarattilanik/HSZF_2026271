namespace TodoManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TodoManager todoManager = new TodoManager();

            todoManager.AddTodo("Buy groceries");

            List<Todo> todos = todoManager.GetTodos();

            todoManager.RemoveTodo(1);

            todos = todoManager.GetTodos();
        }
    }
}
