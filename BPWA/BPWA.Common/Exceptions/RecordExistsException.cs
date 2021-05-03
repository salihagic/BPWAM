using System;

namespace BPWA.Common.Exceptions
{
    public class RecordExistsException : Exception
    {
        public RecordExistsException() : base("Record already exists") { }
        public RecordExistsException(string message) : base(message) { }
        public RecordExistsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
