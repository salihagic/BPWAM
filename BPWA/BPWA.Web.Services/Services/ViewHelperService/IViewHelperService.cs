namespace BPWA.Web.Services.Services
{
    public interface IViewHelperService
    {
        #region Administration

        #region Sections

        bool ShowAdministrationSection();
        bool ShowAuthSection();
        bool ShowGeolocationsSection();
        bool ShowNotificationsSection();
        
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
        bool ShowLogsItem();
        bool ShowNotificationsItem();
        bool ShowGroupsItem();
        bool ShowTicketsItem();
        bool ShowTranslationsItem();

        #endregion Items

        #endregion Administration

        #region Company

        #region Sections

        bool ShowCompanySection();
        bool ShowCompanyAuthSection();
        bool ShowCompanyNotificationsSection();

        #endregion 

        #region Items

        bool ShowBusinessUnitsItem();
        bool ShowCompanyRolesItem();
        bool ShowCompanyUsersItem();
        bool ShowCompanyNotificationsItem();
        bool ShowCompanyGroupsItem();

        #endregion

        #endregion

        #region Business unit

        #region Sections

        bool ShowBusinessUnitSection();
        bool ShowBusinessUnitAuthSection();
        bool ShowBusinessUnitNotificationsSection();

        #endregion 

        #region Items

        bool ShowBusinessUnitRolesItem();
        bool ShowBusinessUnitUsersItem();
        bool ShowBusinessUnitNotificationsItem();
        bool ShowBusinessUnitGroupsItem();

        #endregion

        #endregion
    }
}
