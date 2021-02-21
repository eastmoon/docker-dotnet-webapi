using System;
using WebService.Core.Common.Enums;

namespace WebService.Core.Common.Exceptions
{
    public class ResourceNotFoundNsException : BaseException
    {
        public ResourceNotFoundNsException(Guid uuid)
            : this(uuid, null)
        {
        }

        public ResourceNotFoundNsException(Guid uuid, Exception innerException)
            : base($"Resource '{uuid}' not found.", innerException, HttpStatusCode.ResourceNotFound)
        {
        }
    }
}
