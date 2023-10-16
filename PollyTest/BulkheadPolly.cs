using Polly;
using Polly.Bulkhead;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace PollyTest;

public class BulkheadPolly
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
                    .WithDelay(3000)
            );
    }

    [Test]
    public async Task UsePollyToCreateRunABulkhead()
    {
        var bulkheadPolicy1 = Policy.BulkheadAsync(5, 20);
        // Using the policy to handle requests and potential failures
        var responseString = await bulkheadPolicy1.ExecuteAsync(async () =>
        {
            var response = await _httpClient.GetAsync("http://localhost:8044/hello");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        });

        Assert.That(responseString, Is.Not.Null);
        Console.WriteLine($"Response: {responseString}");
    }

    [Test]
    public async Task UsePollyToCreateRunABulkheadAndExceedTheQueue()
    {
        // Using the policy to handle requests and potential failures
        var bulkheadPolicy = Policy.BulkheadAsync(5, 20);

        var tasks = new List<Task<string>>();

        for (var i = 0; i < 100; i++)
        {
            var responseString = MakeCallToService(bulkheadPolicy);
            Assert.That(responseString, Is.Not.Null);
            Console.WriteLine($"Response: {responseString}");
            tasks.Add(responseString);
        }

        for (var j = 0; j < 1000; j++)
        {
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine($"BulkheadCount: {bulkheadPolicy.QueueAvailableCount}");
            });
        }

        await Task.WhenAll(tasks);
    }


    private async Task<string> MakeCallToService(IAsyncPolicy bulkheadPolicy)
    {
        return await bulkheadPolicy.ExecuteAsync(async () =>
        {
            var response = await _httpClient.GetAsync("http://localhost:8044/hello");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        });
    }
}
