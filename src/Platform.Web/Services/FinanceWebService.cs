using Platform.Web.Clients;
using Platform.Web.ViewModels.Finance;

namespace Platform.Web.Services;

public sealed class FinanceWebService
{
    public FinanceWebService(FinanceApiClient financeApiClient)
    {
        FinanceApiClient = financeApiClient;
    }

    private FinanceApiClient FinanceApiClient { get; }

    public FinanceHomeViewModel GetHome()
    {
        return new FinanceHomeViewModel(
            0,
            0,
            0,
            [
                "Conciliacao de contas",
                "Recebiveis por cliente",
                "Documentos pendentes por vencimento"
            ]);
    }
}
