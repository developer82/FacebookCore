using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rest.Net;
using Rest.Net.Interfaces;

namespace FacebookCore
{
    public abstract class ApiBaseClass
    {
        protected readonly FacebookClient FacebookClient;
        protected readonly IRestClient RestClient;

        protected ApiBaseClass(FacebookClient client)
        {
            FacebookClient = client;
            RestClient = client.RestClient;
        }
    }
}
