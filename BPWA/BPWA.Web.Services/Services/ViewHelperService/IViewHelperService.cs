namespace BPWA.Web.Services.Services
{
    public interface IViewHelperService
    {
        bool ShouldShowAuthSection();
        bool ShouldShowRolesSection();
        bool ShouldShowGeolocationsSection();
        bool ShouldShowCountriesSection();
        bool ShouldShowCitiesSection();
        bool ShouldShowCurrenciesSection();
        bool ShouldShowLanguagesSection();
        bool ShouldShowSystemSettingsSection();
    }
}
