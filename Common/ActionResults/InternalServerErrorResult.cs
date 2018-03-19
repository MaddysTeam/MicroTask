using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Common.ActionResults
{

    public class InternalServerErrorResult :ObjectResult
    {

        public InternalServerErrorResult(object error):base(error)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError;
        }

    }

}
