using Platform.Web.Clients;
using Platform.Web.ViewModels.Crm;

namespace Platform.Web.Services;

public sealed class CrmWebService
{
    public CrmWebService(CrmApiClient crmApiClient)
    {
        CrmApiClient = crmApiClient;
    }

    private CrmApiClient CrmApiClient { get; }

    public CrmHomeViewModel GetHome()
    {
        return new CrmHomeViewModel(
            0,
            0,
            0,
            [
                "Carteira por vendedor",
                "Funil por equipe",
                "Agenda comercial por responsavel"
            ]);
    }
}
