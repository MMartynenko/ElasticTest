using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Nest;
using ElasticLinq;
using ElasticLinq.Mapping;
using System.Security.Cryptography.X509Certificates;
using Elasticsearch.Net;
using Newtonsoft.Json;

namespace ElasticTest
{
    public class ElasticDataAccess
    {
        public ElasticClient _Client;
        private ElasticConnection _linqCon;
        private ElasticContext _ElasticContext;
        private ElasticMapping _Mapping = new ElasticMapping(pluralizeTypeNames: false, camelCaseTypeNames: false);
        private ConnectionSettings _ConnectionSettings { get; set; }


        public ElasticDataAccess()
        {
            Uri node = new Uri("https://127.0.0.1:9200");
            string defaultIndex = "result_set";
            var user = "tester";
            var pass = "tester";

            //X509Certificate x509Certificate = new X509Certificate(@"C:\Projects\ElasticTest\ElasticTest\cert.cer");

            _ConnectionSettings = new ConnectionSettings(node)
                .DefaultIndex(defaultIndex)
                .BasicAuthentication(user, pass)
                .DisableDirectStreaming()
                .ServerCertificateValidationCallback(CertificateValidations.AllowAll);

            _Client = new ElasticClient(_ConnectionSettings);
            _linqCon = new ElasticConnection(node, defaultIndex);
            _ElasticContext = new ElasticContext(_linqCon, _Mapping);
        }
        public bool CheckIfIndexExists(string indexName)
        {
            bool exists = _Client.Indices.Exists(indexName).Exists;
            return exists;
        }

        public async Task<IndexResponse> IndexDocumentAsync (Person person)
        {
            return await _Client.IndexDocumentAsync(person);
        }

        public async Task<IndexResponse> IndexDocumentAsync(ElasticSearchDashboardModelJson model)
        {            
            return await _Client.IndexDocumentAsync(model);
        }

        public string SearchDocs (string firstName)
        {
            var searchResponse = _Client.Search<Person>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.firstName)
                        .Query(firstName)
                        )
                    )
                );
            return searchResponse.Documents.Count.ToString();
        }

        public string SearchDocs(int code)
        {
            var searchResponse = _Client.Search<ElasticSearchDashboardModel>(s => s
                .Query(q => q
                    .Range(m => m
                        .Field(f => f.Code)
                        .GreaterThanOrEquals(code)
                        )
                    )
                );
            return searchResponse.Documents.Count.ToString();
        }

        public string SearchDocsJson(string json)
        {
            var searchResponse = _Client.Search<ElasticSearchDashboardModelJson>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.ResultSet)
                        .Query(json)
                        )
                    )
                );
            return searchResponse.Documents.Count.ToString();
        }

        //public void AddDataCache(ResultModel model, string indexName)
        //{
        //    var blkOperations = new List<BulkIndexOperation<ResultModel>>() { new BulkIndexOperation<ResultModel>(model) }.ToList().Cast<IBulkOperation>().ToList();
        //    var blkRequest = new BulkRequest(indexName)
        //    {
        //        Operations = blkOperations
        //    };
        //    var response = _Client.Bulk(blkRequest);
        //}

        /// <summary>
        /// Get the certificate using the certificate thumbprint
        /// </summary>
        /// <param name="certificateThumbprint">Thumbprint of certificate</param>
        /// <returns>Certificate object</returns>
        //public static X509Certificate2 GetCertificateByThumbprint(string certificateThumbprint)
        //{

        //    // Open the certificate store
        //    X509Store certificateStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        //    certificateStore.Open(OpenFlags.ReadOnly);

        //    // Get the certificates
        //    var matchingCertificates = certificateStore.Certificates.Find(X509FindType.FindByThumbprint, certificateThumbprint, false);
        //    if (matchingCertificates.Count == 0)
        //    {
        //        // No certificate found
        //        return null;
        //    }
        //    else
        //    {
        //        // Return first certificate
        //        return matchingCertificates[0];
        //    }
        //}
    }
    //public class HttpConnectionWithClientCertificate : HttpConnection
    //{
    //    protected override HttpWebRequest CreateHttpWebRequest(RequestData requestData)
    //    {
    //        var request = base.CreateHttpWebRequest(requestData);
    //        // add the certificate to the request
    //        request.ClientCertificates.Add(new X509Certificate("path_to_cert"));
    //        return request;
    //    }
    //}

    
}