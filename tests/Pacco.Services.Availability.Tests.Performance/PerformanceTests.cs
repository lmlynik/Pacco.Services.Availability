using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NBomber.CSharp;
using NBomber.Http.CSharp;
using Newtonsoft.Json;
using Pacco.Services.Availability.Application.Commands;
using Xunit;

namespace Pacco.Services.Availability.Tests.Performance
{
    public class PerformanceTests
    {
        [Fact]
        public void get_resources()
        {
            const string url = "http://localhost:5001";
            var endpoint = $"{url}/resources";
            const int duration = 3;
            const int expectedRps = 100;
            const string stepName = "test";

            var step = HttpStep.Create(stepName, ctx =>
                Task.FromResult(Http.CreateRequest("GET", endpoint)
                    .WithCheck(response => Task.FromResult(response.IsSuccessStatusCode))));

            var assertions = new[]
            {
                Assertion.ForStep(stepName, s => s.RPS >= expectedRps),
                Assertion.ForStep(stepName, s => s.OkCount >= expectedRps * duration)
            };

            var scenario = ScenarioBuilder.CreateScenario("GET resources", step)
                .WithConcurrentCopies(1)
                .WithWarmUpDuration(TimeSpan.FromSeconds(3))
                .WithDuration(TimeSpan.FromSeconds(duration))
                .WithAssertions(assertions);
            
            NBomberRunner.RegisterScenarios(scenario)
                .RunTest();
        }
        
        [Fact]
        public void post_resources()
        {
            const string url = "http://localhost:5001";
            const string stepName = "init";
            const int duration = 3;
            const int expectedRps = 50;
            var endpoint = $"{url}/resources";

            var step = HttpStep.Create(stepName, ctx =>
                Task.FromResult(Http.CreateRequest("POST", endpoint)
                    .WithBody(GetContent(new AddResource(Guid.NewGuid(), new []{"test"})))
                    .WithCheck(response => Task.FromResult(!string.IsNullOrWhiteSpace(response.Headers.Location.AbsolutePath)))
                    .WithCheck(response => Task.FromResult(response.IsSuccessStatusCode))));

            var assertions = new[]
            {
                Assertion.ForStep(stepName, s => s.RPS >= expectedRps),
                Assertion.ForStep(stepName, s => s.OkCount >= expectedRps * duration)
            };

            var scenario = ScenarioBuilder.CreateScenario("POST resources", step)
                .WithConcurrentCopies(1)
                .WithOutWarmUp()
                .WithDuration(TimeSpan.FromSeconds(duration))
                .WithAssertions(assertions);

            NBomberRunner.RegisterScenarios(scenario)
                .RunTest();
        }
        
        private static StringContent GetContent(object @object)
            => new StringContent(JsonConvert.SerializeObject(@object), Encoding.UTF8, "application/json");
    }
}