using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Dominio.ValueObjects;
using System.Net;
using System.Net.Http.Json;

namespace JornadaMilhas.Integration.Test.Api;

//compartilhar uma instancia de um objeto dentro da mesma classe usar o IClassFixture
//compartilhar uma instancia de um objeto entre as classes utilizar o ICollectionFixture

public class OfertaViagemPost(JornadaMilhasWebApplicationFactory app) : IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private readonly JornadaMilhasWebApplicationFactory app = app;

    [Fact]
    public async Task Cadastra_OfertaViagem()
    {
        //Arrange
        using var client = await app.GetClientWithAccessTokenAsync();

        var ofertaViagem = new OfertaViagem()
        {
            Preco = 100,
            Rota = new Rota("Origem", "Destino"),
            Periodo = new Periodo(DateTime.Parse("2024-03-03"), DateTime.Parse("2024-03-06"))
        };
        //Act
        var response = await client.PostAsJsonAsync("/ofertas-viagem", ofertaViagem);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Cadastra_OfertaViagemSemAutorizacao()
    {
        //Arrange
        using var client = app.CreateClient();

        var ofertaViagem = new OfertaViagem()
        {
            Preco = 100,
            Rota = new Rota("Origem", "Destino"),
            Periodo = new Periodo(DateTime.Parse("2024-03-03"), DateTime.Parse("2024-03-06"))
        };
        //Act
        var response = await client.PostAsJsonAsync("/ofertas-viagem", ofertaViagem);

        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
