using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebhookProxy.Utility
{

    /// <summary>
    /// 
    /// </summary>
    public enum ServiceAction
    {
        /// <summary>
        /// 
        /// </summary>
        GET,
        /// <summary>
        /// 
        /// </summary>
        POST
    }

    /// <summary>
    /// 
    /// </summary>
    public class ServicesUtility
    {
        private IConfiguration _configuration;

        public ServicesUtility(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public dynamic RelayMessage(string payload)
        {
            var target = _configuration.GetSection("Target")["Location"];
            var securityHeaderValue = _configuration.GetSection("Target")["SecurityHeader"];

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("securityheader", securityHeaderValue);

            var res = CallService(ServiceAction.POST, headers, target, payload, true);
            return res;
        }

        public dynamic CallService(ServiceAction action, Dictionary<string, string> headerValues, string url, string jsonContent, bool handleError)
        {
            var res = CallService(action, headerValues, url, jsonContent);
            dynamic json = JsonConvert.DeserializeObject(res);

            //If we have a custom object returned from the called service that wants to pass us its internal errors, 
            //we want to throw that exception detail
            if (handleError && json != null && json.error != null && !String.IsNullOrEmpty((string)json.error))
            {
                throw new Exception((string)json.error);
            }

            return json;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="apiKey"></param>
        /// <param name="url"></param>
        /// <param name="jsonContent"></param>
        /// <returns></returns>
        private dynamic CallService(ServiceAction action, Dictionary<string, string> headerValues, string url, string jsonContent)
        {
            string res = null;

            using (var client = new WebClient { Encoding = System.Text.Encoding.UTF8 })
            {
                //Set the content type header
                client.Headers[HttpRequestHeader.ContentType] = "application/json";

                foreach (var header in headerValues)
                {
                    client.Headers.Add(header.Key, header.Value);
                }

                if (action == ServiceAction.GET)
                {
                    res = client.DownloadString(url);
                }
                else if (action == ServiceAction.POST)
                {
                    if (jsonContent == null)
                        jsonContent = "{}";

                    res = client.UploadString(url, jsonContent);
                }
            }

            return res;
        }
    }
}
