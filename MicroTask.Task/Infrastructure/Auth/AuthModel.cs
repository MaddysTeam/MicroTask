﻿using Steeltoe.Common.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{

    public class AuthTokenRequest
    {

        public AuthTokenRequest(string authority, string client, string secret,string api,
            string userName,string password,
            DiscoveryHttpClientHandler httpClientHandler)
        {
            Authority = authority;
            Client = client;
            Secret = secret;
            Api = api;
            Password = password;
            UserName = userName;
           // Scope = scope;
            HttpClientHandler = httpClientHandler;
        }

        public string Authority { get; }
        public string Client { get; }
        public string Secret { get; }
        public string Api { get; }
        public string UserName { get; set; }
        public string Password { get; set; }
       // public string Scope { get; set; }

        /// <summary>
        /// Steeltoe discovery client handlers for spring cloud 
        /// </summary>
        public DiscoveryHttpClientHandler HttpClientHandler { get; set; }

    }

    public class AuthTokenResponse
    {
        public AuthTokenResponse(string accessToken, bool isSuccess, string error)
        {
            AccessToken = accessToken;
            IsSuccess = isSuccess;
            Error = error;
        }

        public string AccessToken { get; }
        public bool IsSuccess { get; }
        public string Error { get; }
    }

    public enum AuthType
    {
        byCredential,
        byResoucePassword
    }

}
