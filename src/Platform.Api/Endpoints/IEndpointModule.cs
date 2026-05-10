namespace Platform.Api.Endpoints;

public interface IEndpointModule
{
    void MapEndpoints(IEndpointRouteBuilder app);
}
