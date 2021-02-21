using System.Collections.Generic;

namespace WebService.Core.Models
{
    public class Result<T>
    {
        public IEnumerable<T> Page { get; set; }

        public long Total { get; set; }
    }
}
