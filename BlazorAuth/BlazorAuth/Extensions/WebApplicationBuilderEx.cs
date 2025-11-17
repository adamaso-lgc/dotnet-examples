using Auth0.AspNetCore.Authentication;
using BlazorAuth.Components;

namespace BlazorAuth.Extensions;

public static class WebApplicationBuilderEx
{
    public static void ConfigureApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        builder.Services.AddAuth0WebAppAuthentication(options =>
        {
            options.Domain = builder.Configuration["Auth0:Domain"] ?? string.Empty;
            options.ClientId = builder.Configuration["Auth0:ClientId"] ?? string.Empty;
            options.Scope = "openid profile email";
        });
        
        builder.Services.AddHttpContextAccessor();
    }

    public static void ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        
        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        app.UseHttpsRedirection();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
       // app.MapAdditionalIdentityEndpoints();
    }
}