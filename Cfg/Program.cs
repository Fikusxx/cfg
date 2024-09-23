using Cfg.Options.DictionaryOptions;
using Cfg.Options.NamedOptions;
using Cfg.Options.ScopedOptions;
using Cfg.Options.SingletonOptions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddScopedOptions();
builder.AddSingletonOptions();
builder.AddNamedOptions();
builder.AddDictionaryOptions();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapSingletonOptions();
app.MapScopedOptions();
app.MapNamedOptions();
app.MapDictionaryOptions();

app.Run();