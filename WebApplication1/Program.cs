using DataAccess;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("connect"));
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/" ,() => "Hello minimal api");


app.MapGet("/users", (DataContext context) =>
{
    return context.UsersDb.ToList();
});



app.MapGet("/users/{id}", (DataContext context , int id) =>
{
    
    return context.UsersDb.FindAsync(id).Result is UserModel user ? 
           Results.Ok(user) : Results.NotFound("user not found");

});


app.MapDelete("/users/deleteUser/{id}", (DataContext context, int id) =>
{
    UserModel user = context.UsersDb.FindAsync(id).Result;

    if(user != null)
    {
        context.UsersDb.Remove(user);

        context.SaveChanges();
    }
    else
    {
        return Results.NotFound("User not found");
    }

    return Results.Ok("User: " + user.Name + " deleted");


});

app.MapPost("/users/addUser/", async (DataContext context, UserModel user) =>
{
    try
    {
        var checkUser = context.UsersDb.FirstOrDefaultAsync(u => u.Username == user.Username);
        if(checkUser == null)
        {
            return Results.BadRequest("Username: " + user.Username + " already exist");
        }
        context.UsersDb.Add(user);
        await context.SaveChangesAsync();

        return Results.Ok("User created");

    }catch(Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
   
});

app.MapPut("/users/EditUser/", async (DataContext context , UserModel user ) =>
{
    UserModel dbUser = context.UsersDb.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == user.UserId).Result;

    if(dbUser == null) return Results.NotFound("user not found");

        context.Entry(user).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return Results.Ok(user);

});

        
app.Run();

