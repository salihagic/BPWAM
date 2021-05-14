using System;
using System.Collections.Generic;
using System.Linq;

namespace BPWA.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public List<string> Messages { get; set; }

        public ValidationException(params string[] messages) : base(messages?.FirstOrDefault() ?? "Validation error occured")
        {
            Messages = messages.ToList();
        }

        public ValidationException() : base("Validation error occured") { }
        public ValidationException(string message) : base(message) { }
        public ValidationException(Exception innerException) : base("Validation error occured", innerException) { }
        public ValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
