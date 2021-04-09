using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GetBestSellerList
{
    public class Function
    {
        static readonly HttpClient bestsellers = new HttpClient();
       

        public async Task<ExpandoObject> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            Dictionary<String, String> cr = (Dictionary<String, String>)input.QueryStringParameters;
            String list = String.Empty;
            cr.TryGetValue("list", out list);
            HttpResponseMessage res = await bestsellers.GetAsync("https://api.nytimes.com/svc/books/v3/lists/current/" + list + ".json?api-key=0MQAi7udASIEd5oRXMpu2Ax895e09Nt7");
            String resmes = await res.Content.ReadAsStringAsync();
            ExpandoObject eo = JsonConvert.DeserializeObject<ExpandoObject>(resmes);
            return eo;
        }
    }
}
