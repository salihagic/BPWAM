namespace BPWA.Web.Services.Services
{
    public interface IViewHelperService
    {
        bool ShouldShowSystemSettingsSection();

        bool ShouldShowAuthSection();
        bool ShouldShowCompaniesItem();
        bool ShouldShowRolesItem();

        bool ShouldShowGeolocationsSection();
        bool ShouldShowCountriesItem();
        bool ShouldShowCitiesItem();
        bool ShouldShowCurrenciesItem();
        bool ShouldShowLanguagesItem();

        bool ShouldShowGeneralSystemSettingsSection();
        bool ShouldShowTicketsItem();
    }
}
