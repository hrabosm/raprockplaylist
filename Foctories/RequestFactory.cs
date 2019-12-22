using RestSharp;
using System.Collections.Generic;

namespace RaprockPlaylist.Factories
{
    public class RequestFactory
    {
        public RestRequest RequestConstructor(Dictionary<string,string> requestData, Dictionary<string,string> headersData, Method methodType)
        {
            var restRequest = new RestRequest(methodType);
            foreach (var item in requestData)
            {
                restRequest.AddParameter(item.Key,item.Value);
            }
            foreach (var item in headersData)
            {
                restRequest.AddHeader(item.Key,item.Value);
            }
            return restRequest;
        }
        public RestRequest RequestConstructor(Dictionary<string,string> requestData, Method methodType)
        {
            var restRequest = new RestRequest(methodType);
            foreach (var item in requestData)
            {
                restRequest.AddParameter(item.Key,item.Value);
            }
            return restRequest;
        }
        public IRestResponse RequestSender(string url, RestRequest request)
        {
            var clientRequest = new RestClient(url);
            var response = clientRequest.Execute(request);
            return response;
        }
    }
}