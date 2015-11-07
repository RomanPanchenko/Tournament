using System.Collections.Generic;

namespace TournamentWebApi.Core.Entities
{
    public abstract class ListEntity<T>
    {
        public List<T> Items { get; set; }
        public int PageCount { get; set; }
        public int CurrentPageNumber { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
