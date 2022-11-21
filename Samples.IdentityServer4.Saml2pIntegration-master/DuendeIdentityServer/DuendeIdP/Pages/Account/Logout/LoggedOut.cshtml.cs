using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rsk.Saml.DuendeIdentityServer.Services.Models;
using Rsk.Saml.Services;

namespace DuendeIdP.Pages.Logout;

[SecurityHeaders]
[AllowAnonymous]
public class LoggedOut : PageModel
{
    private readonly IIdentityServerInteractionService _interactionService;
    private readonly ISamlInteractionService _samlInteractionService;

    public LoggedOutViewModel View { get; set; }
    public string SamlSignOutIframeUrl { get; set; }

    public LoggedOut(IIdentityServerInteractionService interactionService, ISamlInteractionService samlInteractionService)
    {
        _interactionService = interactionService;
        _samlInteractionService = samlInteractionService;
    }

    public async Task OnGet(string logoutId)
    {
        // get context information (client name, post logout redirect URI and iframe for federated signout)
        var logout = await _interactionService.GetLogoutContextAsync(logoutId);

        View = new LoggedOutViewModel
        {
            AutomaticRedirectAfterSignOut = LogoutOptions.AutomaticRedirectAfterSignOut,
            PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
            ClientName = String.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
            SignOutIframeUrl = logout?.SignOutIFrameUrl
        };

        if (logout != null)
        {
            var samlLogout = await _samlInteractionService.GetSamlSignOutFrameUrl(logoutId, new SamlLogoutRequest(logout));
            SamlSignOutIframeUrl = samlLogout;
        }
    }
}