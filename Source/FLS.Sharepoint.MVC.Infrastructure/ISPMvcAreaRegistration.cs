namespace FLS.Sharepoint.MVC.Infrastructure
{
    public interface ISPMvcAreaRegistration
    {
        string AreaName { get; }

        void RegisterRoutes(SPMvcAreaRegistrationContext areaRegistrationContext);
    }
}
