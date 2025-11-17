using Unify.Identity;
using Unify.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddUnifyIdentity();
builder.Services.AddRazorPages();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("proxyPolicy", policy =>
        policy.RequireAuthenticatedUser());

var app = builder.Build();
app.UseUnifyWeb();
app.UseUnifyIdentity();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapReverseProxy();
Unify.Identity.ApplicationBuilderExtensions.FixMicrosoftIdentityOptionsMonitorRaceCondition(app.Services);
app.Run();