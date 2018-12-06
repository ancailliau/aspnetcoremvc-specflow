using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using TechTalk.SpecFlow;
using WebApp;
using AngleSharp.Parser.Html;

namespace DocIntel.WebApp.IntegrationTests.Driver
{
    [Binding]
    public class Steps 
    {
        private WebApplicationFactory<Startup> factory;
        private HttpClient client;
        private IWebHost host;
        private IServiceScope scope;
        private HttpResponseMessage response;

        public Steps()
        {
            Init();
        }

        private void Init()
        {
            System.Console.WriteLine("init");
            factory = new WebApplicationFactory<Startup>();
            client = factory.CreateClient();
            host = factory.Server?.Host;
            scope = host.Services.CreateScope();
        }

        ~Steps() {
            System.Console.WriteLine("disposing");
            if (scope != null)
                scope.Dispose();
            if (client != null)
                client.Dispose();
            if (factory != null)
                factory.Dispose();
        }

        [When(@"I go to page '([^\']*)'")]
        public async Task WhenIGoToPage(string uri)
        {
            response =  await client.GetAsync(uri);
        }

        [Then(@"the http result should be OK")]
        public void TheHTTPResultShouldBeOK()
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Then(@"the body contains '([^\']*)'")]
        public async void TheBodyContains(string str)
        {
            var parser = new HtmlParser();
            var content = await response.Content.ReadAsStringAsync();
            var document = parser.Parse(content);
            Assert.NotNull(document);
            Assert.True(document.Body.TextContent.Contains(str));
        }
    }
}