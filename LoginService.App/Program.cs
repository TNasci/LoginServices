using LoginService.App.Data.Extensions;
using LoginService.App.Data.Seed;

var builder = WebApplication.CreateBuilder(args);

// Adicionar configurações de serviços e dependências
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agendamento API v1"));
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

// Chamar o metodo SeeedData
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Inicialize(services);
}

app.MapControllers();

app.Run();
