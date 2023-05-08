using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RottenTomatoes.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RottenTomatoesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RottenTomatoesContext") ?? throw new InvalidOperationException("Connection string 'RottenTomatoesContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
//Probando..
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

//Cada que a�adamos un nuevo controlador y view debemos llamar al metodo MapControllerRoute y
//buscarlo tal que sea controlller = *nombre carpeta* y action = *archivo.cshtml* TODO guardado en la carpeta View
app.MapControllerRoute(
    name: "login",
    pattern: "account/login",
    defaults: new { controller = "Account", action = "Login" });




app.Run();

