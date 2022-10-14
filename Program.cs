using Microsoft.AspNetCore.Mvc;
using minitodo.Data;
using minitodo.ViewModel;
using Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("", () =>
{

    return Results.Redirect("/swagger/index.html");
});

app.MapGet("v1/todos", (AppDbContext context) =>
{
    var todos = context.Todos.ToList();
    return Results.Ok(todos);
}).Produces<Todo>();

app.MapPost("v1/todos", (
    AppDbContext context,
    CreateTodoViewModel model) =>
{

    var todo = model.MapTo();
    if (!model.IsValid)
        return Results.BadRequest(model.Notifications);

    context.Todos.Add(todo);
    context.SaveChanges();

    return Results.Created($"/v1/todos/{todo.Id}", todo);
}).Produces<Todo>();


app.MapGet("v1/todos/{id:Guid}", ([FromRoute] Guid id, AppDbContext context) =>
{
    var todo = context.Todos.FirstOrDefault(x => x.Id == id);
    if (todo == null)
        return Results.NotFound();
    return Results.Ok(todo);
}).Produces<Todo>();




app.Run();
