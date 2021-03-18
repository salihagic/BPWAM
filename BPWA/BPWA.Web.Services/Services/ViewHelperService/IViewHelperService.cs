namespace BPWA.Web.Services.Services
{
    public interface IViewHelperService
    {
        bool ShouldShowCompaniesItem();

        bool ShouldShowSystemSettingsSection();

        bool ShouldShowAuthSection();
        bool ShouldShowRolesItem();

        bool ShouldShowGeolocationsSection();
        bool ShouldShowCountriesItem();
        bool ShouldShowCitiesItem();
        bool ShouldShowCurrenciesItem();
        bool ShouldShowLanguagesItem();

        bool ShouldShowTicketsItem();
    }
}
