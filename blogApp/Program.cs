using blogApp.Data;
using blogApp.Datacontext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DataContext>(options =>
{

	var config = builder.Configuration;
	var connectionString = config.GetConnectionString("DefaultConnection");
	if (connectionString != null)
		options.UseMySQL(connectionString);
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
	options.LoginPath = "/user/login";
});



var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

await SeedData.TestDataAsync(app);


app.MapControllerRoute(
	name: "posts_by_tag",
	pattern: "post/tag/with/{url}",
	defaults: new { controller = "Post", action = "Index" }
);

app.MapControllerRoute

(
	name: "post_details",
	pattern: "post/details/{url}",
	defaults: new { controller = "Post", action = "Details" }
);

app.MapControllerRoute

(
	name: "post_details",
	pattern: "event-list/{url}",
	defaults: new { controller = "Event", action = "List" }
);

app.MapControllerRoute(
	name: "post_edit",
	pattern: "edit/{url}",

	defaults: new { controller = "Post", action = "Edit" }
);

app.MapControllerRoute(
	name: "event_edit",
	pattern: "event-edit/{id}",
	defaults: new { controller = "Event", action = "Edit" }
);

app.MapControllerRoute(
	name: "profil_detail",
	pattern: "detail/{url}",
	defaults: new { controller = "User", action = "Profile" }
);



app.MapControllerRoute(
	name: "post_delete",
	pattern: "delete/{url}",
	defaults: new { controller = "Post", action = "Delete" }
);

app.MapControllerRoute(
	name: "event_delete",
	pattern: "delete/event/{id}",
	defaults: new { controller = "Event", action = "Delete" }
);

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Post}/{action=Index}/{id?}");

app.Run();
