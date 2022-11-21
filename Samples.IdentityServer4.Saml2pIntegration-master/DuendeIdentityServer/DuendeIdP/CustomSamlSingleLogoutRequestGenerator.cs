using Rsk.IdentityModel.Metadata;
using Rsk.Saml.Configuration;
using Rsk.Saml.Generators;
using Rsk.Saml.Models;
using Rsk.Saml.Services;
using Rsk.Saml.Stores;

namespace DuendeIdP
{
    public class CustomSamlSingleLogoutRequestGenerator : SamlSingleLogoutRequestGenerator
    {
        private readonly SamlSpOptions samlOptions;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CustomSamlSingleLogoutRequestGenerator(ISamlBindingService bindingService, IDateTimeService dateTimeService, ISamlNameIdService nameIdService, ISamlArtifactService artifactService, ISamlSigningCertificateStore signingCertificateStore, ISamlArtifactStore artifactStore, IHttpContextAccessor httpContextAccessor, SamlSpOptions samlOptions, SigningOptions signingOptions, ILogger<ISamlSingleLogoutRequestGenerator> logger) : base(bindingService, dateTimeService, nameIdService, artifactService, signingCertificateStore, artifactStore, httpContextAccessor, samlOptions, signingOptions, logger)
        {
            this.samlOptions = samlOptions;
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }


        protected override async Task<SamlRequest> CreateRequest(EntityId issuer, Uri destination)
        {
            var response = await base.CreateRequest(issuer, destination);

            // expand on NameId information
            response.NameId.Format = new Uri("urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified");

            return response;
        }
    }
}
