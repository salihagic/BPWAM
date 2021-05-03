using System;

namespace BPWA.Common.Exceptions
{
    public class TranslationException : Exception
    {
        public TranslationException() : base("Failed to translate an item") { }
        public TranslationException(string message) : base(message) { }
        public TranslationException(Exception innerException) : base("Failed to translate an item", innerException) { }
        public TranslationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
