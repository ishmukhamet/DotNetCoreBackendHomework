namespace WebApi.BusinessLogic.Contracts.UpdateTodoItem
{
    public class UpdateTodoItemRequest
    {
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; }
    }
}