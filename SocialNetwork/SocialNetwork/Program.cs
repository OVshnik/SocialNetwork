using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwork;
using SocialNetwork.Data;
using SocialNetwork.Data.Models;
using SocialNetwork.Data.Repository;
using SocialNetwork.ViewModels.Extensions;

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Configuration.AddJsonFile("appsettings.json").AddJsonFile("appsettings.Development.json");

		string connection = builder.Configuration.GetConnectionString("DefaultConnection");

		builder.Services.AddAutoMapper(typeof(MappingProfile));

		builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton)
			.AddUnitOfWork()
			.AddCustomRepository<Friend, FriendRepository>()
			.AddCustomRepository<Message,MessageRepository>();
		

        builder.Services.AddIdentity<User, IdentityRole>(opts =>
		{
			opts.Password.RequiredLength = 5;
			opts.Password.RequireNonAlphanumeric = false;
			opts.Password.RequireLowercase = false;
			opts.Password.RequireUppercase = false;
			opts.Password.RequireDigit = false;
		}).AddEntityFrameworkStores<ApplicationDbContext>();

		// Add services to the container.
		builder.Services.AddControllersWithViews();
		builder.Services.AddRazorPages();
		builder.Services.AddRouting();


		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Home/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}
		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		app.UseAuthentication();
		app.UseAuthorization();

		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Home}/{action=Index}/{id?}");

		app.Run();
	}
}