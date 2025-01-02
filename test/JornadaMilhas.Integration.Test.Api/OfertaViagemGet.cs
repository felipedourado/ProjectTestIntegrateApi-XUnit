using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Dominio.ValueObjects;
using JornadaMilhas.Integration.Test.Api.Builders;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace JornadaMilhas.Integration.Test.Api;

public class OfertaViagemGet(JornadaMilhasWebApplicationFactory app) 
    : IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private readonly JornadaMilhasWebApplicationFactory app = app;

    [Fact]
    public async Task RetornaOfertaViagemPorId()
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
        var response = await client.GetFromJsonAsync<OfertaViagem>
            ("/ofertas-viagem/" + ofertaExistente.Id);

        //assert
        Assert.NotNull(response);
        Assert.Equal(ofertaExistente.Preco, response.Preco, 0.001);
        Assert.Equal(ofertaExistente.Rota.Origem, response.Rota.Origem);
        Assert.Equal(ofertaExistente.Rota.Destino, response.Rota.Destino);

    }

    [Fact]
    public async Task RetornaOfertaViagemConsultaPaginada()
    {
        //arrange
        var ofertaBuilder = new OfertaViagemDataBuilder();
        var listaOfertas = ofertaBuilder.Generate(80);
        app.Context.OfertasViagem.AddRange(listaOfertas);
        app.Context.SaveChanges();

        using var client = await app.GetClientWithAccessTokenAsync();
        
        int pagina = 1;
        int tamanhoPagina = 80;

        //act
        var response = await client.GetFromJsonAsync<ICollection<OfertaViagem>>
            ($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPagina}");

        //assert
        Assert.True(response != null);
        Assert.Equal(tamanhoPagina, response.Count());

    }

    [Fact]
    public async Task RetornaOfertaViagemConsultaUltimaPagina()
    {
        //arrange
        app.Context.Database.ExecuteSqlRaw("Delete from OfertasViagem");

        var ofertaBuilder = new OfertaViagemDataBuilder();
        var listaOfertas = ofertaBuilder.Generate(80);
        app.Context.OfertasViagem.AddRange(listaOfertas);
        app.Context.SaveChanges();

        using var client = await app.GetClientWithAccessTokenAsync();

        int pagina = 4;
        int tamanhoPagina = 25;

        //act
        var response = await client.GetFromJsonAsync<ICollection<OfertaViagem>>
            ($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPagina}");

        //assert
        Assert.True(response != null);
        Assert.Equal(5, response.Count());

    }

    [Fact]
    public async Task RetornaOfertaViagemConsultaPaginaInexistente()
    {
        //arrange
        app.Context.Database.ExecuteSqlRaw("Delete from OfertasViagem");

        var ofertaBuilder = new OfertaViagemDataBuilder();
        var listaOfertas = ofertaBuilder.Generate(80);
        app.Context.OfertasViagem.AddRange(listaOfertas);
        app.Context.SaveChanges();

        using var client = await app.GetClientWithAccessTokenAsync();

        int pagina = 5;
        int tamanhoPagina = 25;

        //act
        var response = await client.GetFromJsonAsync<ICollection<OfertaViagem>>
            ($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPagina}");

        //assert
        Assert.True(response != null);
        Assert.Equal(0, response.Count());

    }

    [Fact]
    public async Task RetornaOfertaViagemConsultaPaginaNegativa()
    {
        //arrange
        app.Context.Database.ExecuteSqlRaw("Delete from OfertasViagem");

        var ofertaBuilder = new OfertaViagemDataBuilder();
        var listaOfertas = ofertaBuilder.Generate(80);
        app.Context.OfertasViagem.AddRange(listaOfertas);
        app.Context.SaveChanges();

        using var client = await app.GetClientWithAccessTokenAsync();

        int pagina = -5;
        int tamanhoPagina = 25;

        //act + assert
        await Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            var response = await client.GetFromJsonAsync<ICollection<OfertaViagem>>
            ($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPagina}");
        });

    }
   
}
