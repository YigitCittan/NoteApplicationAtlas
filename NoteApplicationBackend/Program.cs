using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NoteApplicationBackend.Configurations;
using NoteApplicationBackend.Data;
using NoteApplicationBackend.Models;
using NoteApplicationBackend.Repositories;
using NoteApplicationBackend.Services;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();

builder.Services.AddScoped<PhotoService>();
builder.Services.AddScoped<JwtService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();








builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection(key:"JwtConfiguration"));
// Database Connection
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Authentication and Authorization Services
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => 
        options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<ApplicationDbContext>();



builder.Services.AddAuthentication(options => 
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(jwt =>
    {   
        string? secretValue= builder.Configuration.GetSection("JwtConfiguration:Secret").Value;
        if(secretValue != null)
        {
            var key = Encoding.ASCII.GetBytes(secretValue);

            jwt.SaveToken = true;
            jwt.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                
                ValidateAudience = false,
            
                RequireExpirationTime = true,  
                ValidateLifetime = true
            };
        }
        else
        {
            System.Console.WriteLine("Secret key null");
        }
        
    });
        
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options.AddPolicy("FrontEnd",policy =>
{
    policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    var adminRoleExists = await roleManager.RoleExistsAsync("Admin");
    

    if (!adminRoleExists)
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
    var userRoleExists = await roleManager.RoleExistsAsync("User");
    if (!adminRoleExists)
    {
        await roleManager.CreateAsync(new IdentityRole("User"));
    }

    var adminUser = await userManager.FindByEmailAsync("admin@example.com");

    if (adminUser == null)
    {
        var newAdminUser = new IdentityUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com"
        };

        var result = await userManager.CreateAsync(newAdminUser, "StrongPassword123!");

        if (result.Succeeded)
        {
            Console.WriteLine("Admin kullanıcısı başarıyla oluşturuldu.");

            await userManager.AddToRoleAsync(newAdminUser, "Admin");
            Console.WriteLine("Admin kullanıcısına Admin rolü atandı.");
        }
        else
        {
            Console.WriteLine("Admin kullanıcısı oluşturulamadı. Hatalar:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error.Description);
            }
        }
    }
    else
    {
        Console.WriteLine("Admin kullanıcısı zaten mevcut.");
    }
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("FrontEnd");


app.Run();
