using System;

namespace BPWA.Common.Exceptions
{
    public class MappingException : Exception
    {
        public MappingException() : base("Failed to map an item") { }
        public MappingException(string message) : base(message) { }
        public MappingException(Exception innerException) : base("Failed to map an item", innerException) { }
        public MappingException(string message, Exception innerException) : base(message, innerException) { }
    }
}
