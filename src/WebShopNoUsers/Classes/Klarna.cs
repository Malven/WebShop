using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace WebShopNoUsers.Classes
{
    public class Klarna {
        private string sharedSecret = "hbfbUrAsSZ1Ezsk";


        public string CreateOrder(string jsonData ) {


            HttpClient _client = new HttpClient();
            HttpRequestMessage message = new HttpRequestMessage();
            message.RequestUri = new Uri( "https://checkout.testdrive.klarna.com/checkout/orders" );
            message.Method = HttpMethod.Post;
            message.Headers.Accept.Add( new MediaTypeWithQualityHeaderValue( "application/vnd.klarna.checkout.aggregated-order-v2+json" ) );
            message.Headers.Authorization = new AuthenticationHeaderValue( "Klarna", CreateAuthorization(string.Concat( jsonData, sharedSecret) ) );
            message.Content = new StringContent( jsonData, Encoding.UTF8, "application/vnd.klarna.checkout.aggregated-order-v2+json" );
            var response = _client.SendAsync( message ).Result;
            if(response.StatusCode == HttpStatusCode.Created ) {
                var location = response.Headers.Location.AbsoluteUri;

                //hämta orders
                HttpRequestMessage getMessage = new HttpRequestMessage();
                getMessage.RequestUri = new Uri( location );
                getMessage.Method = HttpMethod.Get;
                getMessage.Headers.Accept.Add( new MediaTypeWithQualityHeaderValue( "application/vnd.klarna.checkout.aggregated-order-v2+json" ) );
                getMessage.Headers.Authorization = new AuthenticationHeaderValue( "Klarna", CreateAuthorization( sharedSecret ) );

                var getResponse = _client.SendAsync( getMessage ).Result;
                var getResponseBody = getResponse.Content.ReadAsStringAsync().Result;

                return getResponseBody;
            }
            return response.Content.ReadAsStringAsync().Result;
        }

        public string GetOrder(string id ) {
            //hämta orders
            HttpClient _client = new HttpClient();
            HttpRequestMessage getMessage = new HttpRequestMessage();
            getMessage.RequestUri = new Uri( "https://checkout.testdrive.klarna.com/checkout/orders/"+id );
            getMessage.Method = HttpMethod.Get;
            getMessage.Headers.Accept.Add( new MediaTypeWithQualityHeaderValue( "application/vnd.klarna.checkout.aggregated-order-v2+json" ) );
            getMessage.Headers.Authorization = new AuthenticationHeaderValue( "Klarna", CreateAuthorization( sharedSecret ) );

            var getResponse = _client.SendAsync( getMessage ).Result;
            var getResponseBody = getResponse.Content.ReadAsStringAsync().Result;
            return getResponseBody;
        }

        private string CreateAuthorization( string data ) {
            //base64(hex(sha256 (request_payload + shared_secret)))

            using( var algorithm = SHA256.Create() ) {
                var bytes = Encoding.GetEncoding( "UTF-8" ).GetBytes( data );
                var hash = algorithm.ComputeHash( bytes );
                return Convert.ToBase64String( hash );
            }            
        }
    }
}
