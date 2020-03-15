using Convey.WebApi.Exceptions;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacco.Services.Availability.Infrastructure.Exceptions
{
    public class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        public ExceptionResponse Map(Exception exception)
         => exception switch
         {
             DomainException ex => new ExceptionResponse(new { code = ex.Code, reason = ex.Message }, System.Net.HttpStatusCode.BadRequest),
             AppException ex => new ExceptionResponse(new { code = ex.Code, reason = ex.Message }, System.Net.HttpStatusCode.BadRequest),
             Exception ex => new ExceptionResponse(new { code = "error", reason = ex.Message }, System.Net.HttpStatusCode.InternalServerError),
         };
    }
}
