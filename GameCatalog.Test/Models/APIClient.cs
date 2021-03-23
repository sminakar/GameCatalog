using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace GameCatalog.Test
{
    public class APIClient
    {
        ApiResultModel _result;
        private readonly HttpClient _client;
        public APIClient()
        {

        }
        public APIClient(string URI, string Data, string Format, HttpClient httpClient)
        {
            this.URI = URI;
            this.Data = Data;
            this.Format = Format;
            _client = httpClient;
        }
        public string URI { get; set; }
        public string Data { get; set; }
        public string Format { get; set; }
        public ApiResultModel Result => _result;
        public ApiResultModel CallApi()
        {
            string dta = this.Data.Replace("'", "\"");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(this.URI),
                Content = new StringContent(dta, Encoding.UTF8, "application/json")
            };

            if (this.Format.ToUpper() == "XML")

                request.Content = new StringContent(dta, Encoding.UTF8, "application/xml");

            var response = _client.SendAsync(request).ConfigureAwait(false);

            var Content = response.GetAwaiter().GetResult().Content.ReadAsStringAsync();
            var serContent = JsonConvert.SerializeObject(Content);

            ApiResultModel apiResult = JsonConvert.DeserializeObject<ApiResultModel>(serContent);
            _result = apiResult;
            return apiResult;
        }

        public class ApiResultModel
        {
            public int Id { get; set; }
            public Exception Exception { get; set; }
            public int Status { get; set; }
            public bool IsCanceled { get; set; }
            public bool IsCompleted { get; set; }
            public bool IsCompletedSuccessfully { get; set; }
            public int CreationOptions { get; set; }
            public string AsyncStatus { get; set; }
            public bool IsFaulted { get; set; }
            public string Result { get; set; }
        }
    }
}
