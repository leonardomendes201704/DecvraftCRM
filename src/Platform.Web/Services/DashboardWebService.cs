using Platform.Web.ViewModels.Dashboard;
using Platform.Web.ViewModels.Components;

namespace Platform.Web.Services;

public sealed class DashboardWebService
{
    public DashboardViewModel GetInitialDashboard()
    {
        var workQueue = new DataTableViewModel(
            ["Fila", "Modulo", "Status"],
            [
                ["Atividades comerciais", "CRM", "Aguardando integracao"],
                ["Recebiveis em aberto", "Financeiro", "Aguardando integracao"],
                ["Cadastros organizacionais", "Organizacao", "Aguardando integracao"]
            ],
            "Nenhum item operacional encontrado.");

        return new DashboardViewModel(
            [
                new DashboardKpiViewModel("Atividades vencidas", "0", "CRM"),
                new DashboardKpiViewModel("Proximas atividades", "0", "7 dias"),
                new DashboardKpiViewModel("Oportunidades abertas", "0", "Pipeline"),
                new DashboardKpiViewModel("Valor em aberto", "R$ 0,00", "Estimado")
            ],
            workQueue,
            [
                "Conectar autenticacao web na API.",
                "Trocar os dados locais por consultas nos API clients.",
                "Aplicar permissoes por usuario e tenant no menu."
            ]);
    }
}
