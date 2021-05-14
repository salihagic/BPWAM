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
        bool ShowConfigurationItem();

        #endregion Items

        #endregion Administration

        #region Company

        #region Sections

        bool ShowCompanySection();
        bool ShowCompanyAuthSection();
        bool ShowCompanyNotificationsSection();

        #endregion 

        #region Items

        bool ShowConvertFromGuestToRegularItem();
        bool ShowCompanyCompaniesItem();
        bool ShowCompanyRolesItem();
        bool ShowCompanyUsersItem();
        bool ShowCompanyNotificationsItem();
        bool ShowCompanyGroupsItem();

        #endregion

        #endregion
    }
}
