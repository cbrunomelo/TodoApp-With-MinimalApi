using Flunt.Notifications;
using Flunt.Validations;
using Models;

namespace minitodo.ViewModel;

public class CreateTodoViewModel : Notifiable<Notification> 
{
    public string Title { get; set; }

    public Todo MapTo()
    {   
        var contract = new Contract<Notification>()
        .Requires()
        .IsNotNull(Title, "Informe o Titulo da tarefa");


        AddNotifications(contract);
        return new Todo(Guid.NewGuid(), Title, false);
    }
}