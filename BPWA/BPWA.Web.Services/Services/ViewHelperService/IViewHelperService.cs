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

        bool ShowToggleCurrentCompanyItem();
        bool ShowToggleCurrentBusinessUnitItem();
        bool ShowCompaniesItem();
        bool ShowRolesItem();
        bool ShowUsersItem();
        bool ShowCitiesItem();
        bool ShowCountriesItem();
        bool ShowCurrenciesItem();
        bool ShowLanguagesItem();
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
