using System;

namespace TournamentWebApi.Core.Validation
{
    public class Error
    {
        public Exception Exception { get; set; }
        public string UserMessage { get; set; }
    }
}
