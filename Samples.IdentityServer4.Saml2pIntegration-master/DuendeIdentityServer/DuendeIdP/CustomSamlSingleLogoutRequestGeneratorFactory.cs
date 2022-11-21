using Rsk.AspNetCore.Authentication.Saml2p.Factories;
using Rsk.Saml.Configuration;
using Rsk.Saml.Generators;
using Rsk.Saml.Services;
using Rsk.Saml.Stores;

namespace DuendeIdP
{
    public class CustomSamlSingleLogoutRequestGeneratorFactory : ISamlFactory<ISamlSingleLogoutRequestGenerator>
    {
        private readonly ISamlBindingService bindingService;
        private readonly IDateTimeService dateTimeService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ISamlArtifactService artifactService;
        private readonly ISamlArtifactStore artifactStore;
        private readonly ILogger<ISamlSingleLogoutRequestGenerator> logger;

        public CustomSamlSingleLogoutRequestGeneratorFactory(
             ISamlBindingService bindingService,
             IDateTimeService dateTimeService,
             ISamlArtifactService artifactService,
             ISamlArtifactStore artifactStore,
             IHttpContextAccessor httpContextAccessor,
             ILogger<ISamlSingleLogoutRequestGenerator> logger)
        {

            this.bindingService = bindingService ?? throw new ArgumentNullException(nameof(bindingService));
            this.dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.artifactService = artifactService ?? throw new ArgumentNullException(nameof(artifactService));
            this.artifactStore = artifactStore ?? throw new ArgumentNullException(nameof(artifactStore));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public ISamlSingleLogoutRequestGenerator Create(SamlSpOptions options)
        {
            return new CustomSamlSingleLogoutRequestGenerator(
                bindingService,
                dateTimeService,
                new SamlNameIdService(httpContextAccessor),
                artifactService,
                new SamlSigningCertificateStore(new SamlOptionsKeyService(options)),
                artifactStore,
                httpContextAccessor,
                options,
                options?.SigningOptions,
                logger);
        }
    }
}
