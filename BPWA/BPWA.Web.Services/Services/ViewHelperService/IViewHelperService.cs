namespace BPWA.Web.Services.Services
{
    public interface IViewHelperService
    {
        #region Administration

        #region Sections

        bool ShowSystemSettingsSection();
        bool ShowAuthSection();
        bool ShowGeolocationsSection();
        
        #endregion Sections

        #region Items

        bool ShowCitiesItem();
        bool ShowCompaniesItem();
        bool ShowCountriesItem();
        bool ShowCurrenciesItem();
        bool ShowLanguagesItem();
        bool ShowRolesItem();
        bool ShowTicketsItem();

        #endregion Items

        #endregion Administration
    }
}
