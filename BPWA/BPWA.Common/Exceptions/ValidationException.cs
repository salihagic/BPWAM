using System;
using System.Collections.Generic;
using System.Linq;

namespace BPWA.Common.Exceptions
{
    public class ValidationException : Exception 
    {
        public List<string> Errors { get; set; }

        public ValidationException(params string[] errors)
        {
            Errors = errors.ToList();
        }
    }
}
