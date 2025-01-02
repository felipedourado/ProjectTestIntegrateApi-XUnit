using JornadaMilhas.API.DTO.Auth;
using System.Net;
using System.Net.Http.Json;

namespace JornadaMilhas.Integration.Test.Api;

//https://learn.microsoft.com/pt-br/aspnet/core/test/integration-tests?view=aspnetcore-8.0
//https://learn.microsoft.com/pt-br/aspnet/core/test/integration-tests?view=aspnetcore-8.0#aspnet-core-integration-tests
//Um projeto de teste é usado para conter e executar os testes e tem uma referência ao SUT.
//O projeto de teste cria um host Web de teste para o SUT e usa um cliente do servidor de teste para lidar com solicitações e respostas com o SUT.
//Um executor de teste é usado para implementar os testes e relatar os resultados.

public class JornadaMilhas_AuthTest
{
    [Fact]
    public async Task PostEfetuaLoginComSucesso()
    {
        //arrange
        var app = new JornadaMilhasWebApplicationFactory();
        var user = new UserDTO { Email = "tester@email.com", Password = "Senha123@" };
        using var client = app.CreateClient();


        //act
        var resultado = await client.PostAsJsonAsync("/auth-login", user);

        //assert
        Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);

    }
}