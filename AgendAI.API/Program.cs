using AgendAI.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddAgendAIConfiguration();

builder.Services.AddAgendAICors(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(CorsExtensions.AngularPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();
