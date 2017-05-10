using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Xunit.Sdk;

namespace Test
{
    public class Test
    {
        private readonly string _issuerUrl = "http://localhost:54385/api/";
        private readonly string _audienceUrl = "http://localhost:54539/api/";
        private HttpContent _loginContent = new StringContent(JsonConvert.SerializeObject(new
        {
            Username = "zeeko",
            Password = "pwd"
        }), Encoding.UTF8, "application/json");

        [Fact]
        public async Task SuccessTest()
        {
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(_issuerUrl + "token/TestAudience", _loginContent);
            var json = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeAnonymousType(json, new { token = "" });
            Assert.True(string.IsNullOrEmpty(token.token) == false);

            HttpRequestMessage testReq = new HttpRequestMessage(HttpMethod.Get, _audienceUrl + "values")
            {
                Headers = {Authorization = new AuthenticationHeaderValue("Bearer", token.token)}
            };
            var testResult = await client.SendAsync(testReq);
            Assert.True(testResult.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async Task WrongAudienceTest()
        {
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(_issuerUrl + "token/Test", _loginContent);
            var json = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeAnonymousType(json, new {token = ""});
            Assert.True(string.IsNullOrEmpty(token.token)==false);

            HttpRequestMessage testReq = new HttpRequestMessage(HttpMethod.Get, _audienceUrl + "values")
            {
                Headers = {Authorization = new AuthenticationHeaderValue("Bearer", token.token)}
            };
            var testResult = await client.SendAsync(testReq);
            Assert.True(testResult.StatusCode == HttpStatusCode.Unauthorized);
        }
    }
}
