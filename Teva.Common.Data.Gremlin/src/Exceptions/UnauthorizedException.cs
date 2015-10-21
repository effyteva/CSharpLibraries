using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Teva.Common.Data.Gremlin.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string Message)
            : base(Message)
        {
        }
    }
}
