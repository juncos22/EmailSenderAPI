using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Responses
{
    public class MessageResponse
    {
        public int StatusCode { get; set; }
        public string ResponseMessage { get; set; }
        public Message Body { get; set; }

    }
}
