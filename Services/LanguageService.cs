
using System.Reflection;
using Microsoft.Extensions.Localization;

namespace AirlineSeatReservationSystem.Services
{
    public class SharedResource
    {

    }
    public class LanguageService 
    {
        private readonly IStringLocalizer _localizer;

        public LanguageService(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyName= new AssemblyName(type.GetTypeInfo().Assembly.FullName);

            _localizer = factory.Create(nameof(SharedResource), assemblyName.Name);

        }

        public LocalizedString Getkey (string key)
        {
            return _localizer[key];
        }
    }
}