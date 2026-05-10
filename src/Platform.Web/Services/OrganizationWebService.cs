using Platform.Web.Clients;
using Platform.Web.ViewModels.Organization;

namespace Platform.Web.Services;

public sealed class OrganizationWebService
{
    public OrganizationWebService(OrganizationApiClient organizationApiClient)
    {
        OrganizationApiClient = organizationApiClient;
    }

    private OrganizationApiClient OrganizationApiClient { get; }

    public OrganizationHomeViewModel GetHome()
    {
        return new OrganizationHomeViewModel(
            0,
            0,
            0,
            [
                "Departamentos e cargos",
                "Funcionarios vinculados a usuarios",
                "Hierarquia operacional"
            ]);
    }
}
