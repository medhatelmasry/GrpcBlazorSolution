using BlazorGrpcClient.Components;
using Microsoft.Extensions.DependencyInjection;
using Grpc.Net.Client;
using BlazorGrpcClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// add grpc client
builder.Services.AddGrpcClient<StudentRemote.StudentRemoteClient>(options =>
{
    options.Address = new Uri("http://localhost:5099");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
