namespace BPWA.Web.Services.Services
{
    public interface IViewHelperService
    {
        bool ShouldShowSystemSettingsSection();

        bool ShouldShowAuthSection();
        bool ShouldShowRolesSection();

        bool ShouldShowGeolocationsSection();
        bool ShouldShowCountriesSection();
        bool ShouldShowCitiesSection();
        bool ShouldShowCurrenciesSection();
        bool ShouldShowLanguagesSection();

        bool ShouldShowGeneralSystemSettingsSection();
        bool ShouldShowTicketsSection();
    }
}
