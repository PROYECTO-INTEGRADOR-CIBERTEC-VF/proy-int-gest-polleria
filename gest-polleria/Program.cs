var builder = WebApplication.CreateBuilder(args);

// 1. IMPORTANTE: Cambiamos AddControllers por AddControllersWithViews
builder.Services.AddControllersWithViews();

// Si quieres Swagger para pruebas, puedes dejarlo, pero no es necesario para la Web
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Configuración del pipeline
if (app.Environment.IsDevelopment())
{
    // Comenta estas dos líneas si quieres que NO se abra Swagger al inicio
    //app.UseSwagger();
    //app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Importante para CSS y JS

app.UseRouting();

app.UseAuthorization();

// 3. LA RUTA: Aquí le decimos cómo leer tus controladores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MeserosApi}/{action=Index}/{id?}");

app.Run();