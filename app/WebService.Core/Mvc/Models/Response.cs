using System.Collections.Generic;
using WebService.Core.Common.Enums;

namespace WebService.Core.Mvc.Models
{
    public class Response<TContent>
    {
        public Response(TContent content)
        {
            Content = content;
        }

        public bool Success { get; set; }

        public HttpStatusCode StatsCode { get; set; }

        public string Message { get; set; }

        public TContent Content { get; set; }
    }

    public class Response : Response<object>
    {
        public Response()
            : base(null)
        {
        }
    }

    public class PagingResponse<TContent> : Response<IEnumerable<TContent>>
    {
        public PagingResponse(IEnumerable<TContent> pagingContent) : base(pagingContent)
        {
        }

        public long Total { get; set; }
    }
}
