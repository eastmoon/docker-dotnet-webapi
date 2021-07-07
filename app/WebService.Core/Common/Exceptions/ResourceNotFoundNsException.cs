using System;
using WebService.Core.Common.Enums;

namespace WebService.Core.Common.Exceptions
{
    public class ResourceNotFoundNsException : BaseException
    {
        public ResourceNotFoundNsException(uint id)
            : this(id, null)
        {
        }

        public ResourceNotFoundNsException(uint id, Exception innerException)
            : base($"Resource '{id}' not found.", innerException, HttpStatusCode.ResourceNotFound)
        {
        }
    }
}
