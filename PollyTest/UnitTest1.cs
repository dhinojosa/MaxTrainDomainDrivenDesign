using Polly;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace PollyTest;

public class Tests
{
    private readonly WireMockServer _server = WireMockServer.Start(new WireMockServerSettings
    {
        Urls = new[] { "http://localhost:8044" },
        StartAdminInterface = true
    });

    private readonly HttpClient _httpClient = new HttpClient();

    [SetUp]
    public void Setup()
    {
        _server
            .Given(
                Request.Create().WithPath("/hello").UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "text/plain")
                    .WithBody("Hello world!")
            );
    }

    [Test]
    public async Task UsePollyToCreateRunACircuitBreaker()
    {
        var httpRetryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2));

        // Circuit breaker policy: Break the circuit for 30 seconds if the action fails 5 times in a row
        var httpCircuitBreakerPolicy = Policy
            .Handle<HttpRequestException>()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

        // Combine the policies to first apply the retry, then the circuit breaker
        var policyWrap = Policy.WrapAsync(httpRetryPolicy, httpCircuitBreakerPolicy);

        // Using the policy to handle requests and potential failures
        var responseString = await policyWrap.ExecuteAsync(async () =>
        {
            var response = await _httpClient.GetAsync("http://localhost:8044/hello");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        });

        Assert.That(responseString, Is.Not.Null);
        Console.WriteLine($"Response: {responseString}");
    }
}
