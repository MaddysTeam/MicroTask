using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Infrastructure
{

    public class ServerOkResult :ObjectResult
    {

        public ServerOkResult(object error):base(error)
        {
            StatusCode = (int)HttpStatusCode.OK;
        }

    }

}
