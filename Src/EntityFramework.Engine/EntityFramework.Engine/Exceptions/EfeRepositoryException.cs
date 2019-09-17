using System;
using System.Data.Entity.Validation;
using System.Runtime.Serialization;

namespace EntityFramework.Engine.Exceptions
{
    [Serializable]
    public class EfeRepositoryException : Exception
    {
        [NonSerialized]
        private DbEntityValidationException _exception;
        public EfeRepositoryException() { }
        public EfeRepositoryException(string message) : base(message) { }
        public EfeRepositoryException(string messageFormat, params object[] args)
            : this(string.Format(messageFormat, args)) { }
        public EfeRepositoryException(DbEntityValidationException exception)
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

                var msg = string.Empty;
                foreach (var validationErrors in _exception.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                return msg;
            }
        }
    }
}
