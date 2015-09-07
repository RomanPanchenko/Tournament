using System;
using System.Collections.Generic;

namespace TournamentWebApi.Core.Validation
{
    public class ValidationResult
    {
        private List<Error> _errors;
        public List<Error> Errors
        {
            get { return _errors ?? (_errors = new List<Error>()); }
        }

        public bool IsValid
        {
            get { return Errors.Count == 0; }
        }

        public void Clear()
        {
            Errors.Clear();
        }

        public void AddError(Exception exception)
        {
            AddError(exception, null);
        }

        public void AddError(Exception exception, string userMessage)
        {
            Errors.Add(new Error
            {
                Exception = exception,
                UserMessage = userMessage
            });
        }
    }
}
