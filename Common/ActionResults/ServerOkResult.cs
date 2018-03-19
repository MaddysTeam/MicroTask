using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Common.ActionResults
{

    public class ServerOkResult :ObjectResult
    {

        public ServerOkResult(object error):base(error)
        {
            StatusCode = (int)HttpStatusCode.OK;
        }

    }

}
