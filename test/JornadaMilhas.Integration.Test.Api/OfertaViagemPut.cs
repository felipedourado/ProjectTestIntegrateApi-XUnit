using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Dominio.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;

namespace JornadaMilhas.Integration.Test.Api;

public class OfertaViagemPut(JornadaMilhasWebApplicationFactory app) : IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private readonly JornadaMilhasWebApplicationFactory app = app;

    [Fact]
    public async Task AtualizarOfertaViagemPorId()
    {
        //arrange
        app.Context.Database.ExecuteSqlRaw("Delete from OfertasViagem");
        
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

        ofertaExistente.Rota.Origem = "Origem Atualizada";
        ofertaExistente.Rota.Destino = "Destino Atualizada";

        //Act
        var response = await client.PutAsJsonAsync($"/ofertas-viagem/", ofertaExistente);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    
}
