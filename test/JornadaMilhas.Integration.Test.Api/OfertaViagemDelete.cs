using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Dominio.ValueObjects;
using System.Net;

namespace JornadaMilhas.Integration.Test.Api;

public class OfertaViagemDelete(JornadaMilhasWebApplicationFactory app) : IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private readonly JornadaMilhasWebApplicationFactory app = app;

    [Fact]
    public async Task DeletarOfertaViagemPorId()
    {
        //arrange
        var ofertaExistente = app.Context.OfertasViagem.FirstOrDefault();
        if (ofertaExistente is null)
        {
            ofertaExistente = new OfertaViagem()
            {
                Preco = 100,
                Rota = new Rota("Origem", "Destino"),
                Periodo = new Periodo(DateTime.Parse("2024-03-03"), DateTime.Parse("2024-03-06"))
            };
            app.Context.Add(ofertaExistente);
            app.Context.SaveChanges();
        }

        using var client = await app.GetClientWithAccessTokenAsync();

        //act
        var response = await client.DeleteAsync("/ofertas-viagem/" + ofertaExistente.Id);

        //assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
