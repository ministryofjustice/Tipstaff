using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tipstaff
{
    public class NotUploaded : Exception
    {
        public NotUploaded()
        {
        }

        public NotUploaded(string message)
            : base(message)
        {
        }


    }
}