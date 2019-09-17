using System;
using System.Runtime.Serialization;

namespace Microsoft.EntityFrameworkCore.Engine.Exceptions
{
    [Serializable]
    public class EfeRepositoryException : Exception
    {
        [NonSerialized]
        private DbUpdateException _exception;
        public EfeRepositoryException() { }
        public EfeRepositoryException(string message) : base(message) { }
        public EfeRepositoryException(string messageFormat, params object[] args)
            : this(string.Format(messageFormat, args)) { }
        public EfeRepositoryException(DbUpdateException exception)
        {
            _exception = exception;
        }
        protected EfeRepositoryException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
        public EfeRepositoryException(string message, Exception innerException)
            : base(message, innerException) { }

        public override string Message
        {
            get
            {
                if (_exception == null)
                    return base.Message;

                return _exception.ToString();
            }
        }
    }
}
