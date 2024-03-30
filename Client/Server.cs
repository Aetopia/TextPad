using System.Collections;
using System.Collections.Generic;
using System.Net.Http;


class Server
{
    readonly static HttpClient httpClient = new();
    public static HttpResponseMessage Post(Dictionary<string, string> nameValueCollection)
    {
        return httpClient.PostAsync("http://localhost", new FormUrlEncodedContent(nameValueCollection)).Result;
    }
}