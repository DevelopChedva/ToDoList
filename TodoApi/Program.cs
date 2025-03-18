using TodoApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToDoDbContext>(options =>
options.UseMySql(builder.Configuration.GetConnectionString("ToDoDb"), 
    new MySqlServerVersion(new Version(8, 0, 41)))); 

builder.Services.AddCors(option => option.AddPolicy("AllowAll",//נתינת שם להרשאה
    p => p.AllowAnyOrigin()//מאפשר כל מקור
    .AllowAnyMethod()//כל מתודה - פונקציה
    .AllowAnyHeader()));//וכל כותרת פונקציה

// builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
var app = builder.Build();
app.UseCors("AllowAll");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");
// שליפה
app.MapGet("/getAll", async (ToDoDbContext db) =>
{
    var tasks = await db.Items.ToListAsync();
    return Results.Ok(tasks);
});
// הוספה
app.MapPost("/addTask", async (ToDoDbContext db,string s) => 
{
    var task= new Item{
        Name=s,
        IsComplete=false
    };
    db.Items.Add(task);   
    await db.SaveChangesAsync(); 
    return "chedva";
});

// עדכון
app.MapPatch("/updateTask/{id}", async (int id, ToDoDbContext db) =>
{
    var task = await db.Items.FindAsync(id);
    if (task == null)
    {
        return Results.NotFound();
    }
    task.IsComplete = !task.IsComplete; 
    await db.SaveChangesAsync();
    return Results.Ok(task);
});

// מחי��ה
 app.MapDelete("/delete/{id}", async (int id, ToDoDbContext db) =>
 {
     var task = await db.Items.FindAsync(id);
     if (task == null)
     {
         return Results.NotFound();
     }
     db.Items.Remove(task);
     await db.SaveChangesAsync();
    return Results.Ok("Item deleted"); 
  });

app.Run();