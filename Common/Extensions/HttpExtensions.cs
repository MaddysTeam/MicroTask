﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{

    public static class HttpResponseExtensions
    {

        public static async Task SendServerErrorResponse(this HttpResponse response,string contentType,string error)
        {
            response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            response.ContentType = contentType;
            await response.WriteAsync(error);
        }

        public static async Task SendOKResponse(this HttpResponse response, string contentType,string message)
        {
            response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            response.ContentType = contentType;
            await response.WriteAsync(message);
        }

    }

}
