namespace BPWA.Web.Services.Services
{
    public interface IViewHelperService
    {
        #region Administration

        #region Sections
        bool ShowAdministrationSection();
        bool ShowAuthSection();
        bool ShowGeolocationsSection();
        
        #endregion Sections

        #region Items

        bool ShowToggleCompanyItem();
        bool ShowCitiesItem();
        bool ShowCompaniesItem();
        bool ShowCountriesItem();
        bool ShowCurrenciesItem();
        bool ShowLanguagesItem();
        bool ShowRolesItem();
        bool ShowTicketsItem();

        #endregion Items

        #endregion Administration

        #region Company

        #region Sections

        bool ShowCompanySection();

        #endregion Sections

        #region Items

        bool ShowBusinessUnitsItem();

        #endregion Items

        #endregion Company
    }
}
