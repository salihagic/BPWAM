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
        bool ShowTranslationsItem();
        bool ShowTicketsItem();

        #endregion Items

        #endregion Administration

        #region Company

        #region Sections

        bool ShowCompanySection();
        bool ShowCompanyAuthSection();

        #endregion 

        #region Items

        bool ShowBusinessUnitsItem();
        bool ShowCompanyRolesItem();
        bool ShowCompanyUsersItem();

        #endregion

        #endregion

        #region Business unit

        #region Sections

        bool ShowBusinessUnitSection();
        bool ShowBusinessUnitAuthSection();

        #endregion 

        #region Items

        bool ShowBusinessUnitRolesItem();
        bool ShowBusinessUnitUsersItem();

        #endregion

        #endregion

        #region Users

        bool ShowUsersRolesManagement();
        bool ShowUsersCompanyRolesManagement();
        bool ShowUsersBusinessUnitRolesManagement();
        bool ShowUsersCompaniesManagement();
        bool ShowUsersBusinessUnitsManagement();

        #endregion
    }
}
