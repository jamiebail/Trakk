using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Helpers
{
    public class EntityResponse
    {
        public EntityResponse(bool successIn, string messageIn)
        {
            Success = successIn;
            Message = messageIn;
        }

        public EntityResponse(bool successIn, string messageIn, int id)
        {
            Success = successIn;
            Message = messageIn;
            IdReturn = id;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public int  IdReturn { get; set; }
    }
}