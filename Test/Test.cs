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
        private readonly HttpContent _loginContent = new StringContent(JsonConvert.SerializeObject(new
        {
            Username = "zeeko",
            Password = "pwd"
        }), Encoding.UTF8, "application/json");

        /// <summary>
        /// 先申请 token，然后使用这个 token 获取数据
        /// </summary>
        /// <returns></returns>
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
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token.token) }
            };
            var testResult = await client.SendAsync(testReq);
            Assert.True(testResult.StatusCode == HttpStatusCode.OK);
        }

        /// <summary>
        /// 申请一个 audience 不匹配的 token，然后尝试获取数据失败
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task WrongAudienceTest()
        {
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(_issuerUrl + "token/Test", _loginContent);
            var json = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeAnonymousType(json, new { token = "" });
            Assert.True(string.IsNullOrEmpty(token.token) == false);

            HttpRequestMessage testReq = new HttpRequestMessage(HttpMethod.Get, _audienceUrl + "values")
            {
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token.token) }
            };
            var testResult = await client.SendAsync(testReq);
            Assert.True(testResult.StatusCode == HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// 先申请一个 token，然后使 token 失效
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DisableTokenTest()
        {
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(_issuerUrl + "token/TestAudience", _loginContent);
            var json = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeAnonymousType(json, new { token = "" });
            Assert.True(string.IsNullOrEmpty(token.token) == false);

            HttpRequestMessage testReq = new HttpRequestMessage(HttpMethod.Delete, _audienceUrl + "token")
            {
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token.token) }
            };
            var testResult = await client.SendAsync(testReq);
            Assert.Equal(HttpStatusCode.OK, testResult.StatusCode);

            testReq = new HttpRequestMessage(HttpMethod.Get, _audienceUrl + "values")
            {
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token.token) }
            };
            testResult = await client.SendAsync(testReq);
            Assert.True(testResult.StatusCode == HttpStatusCode.Forbidden);
        }

        /// <summary>
        /// 先申请 token 然后尝试使同一个 token 失败两次
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DisableTaskTwiceTest()
        {
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(_issuerUrl + "token/TestAudience", _loginContent);
            var json = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeAnonymousType(json, new { token = "" });
            Assert.True(string.IsNullOrEmpty(token.token) == false);

            HttpRequestMessage testReq = new HttpRequestMessage(HttpMethod.Delete, _audienceUrl + "token")
            {
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token.token) }
            };
            var testResult = await client.SendAsync(testReq);
            Assert.Equal(HttpStatusCode.OK, testResult.StatusCode);

            testReq = new HttpRequestMessage(HttpMethod.Delete, _audienceUrl + "token")
            {
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token.token) }
            };
            testResult = await client.SendAsync(testReq);
            Assert.Equal(HttpStatusCode.Forbidden, testResult.StatusCode);
        }
    }
}
