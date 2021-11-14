using Microsoft.AspNetCore.Http;
using HousingAssociation.ExceptionHandling.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HousingAssociation.ExceptionHandling
{
    /// <summary>
    /// Class <c>ExceptionHandlingMiddleware</c> is custom exception excteption handling middleware.
    /// Used to catch custom exceptions and set related http responses.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// Method <c>HandleExceptionAsync</c> catches exceptions and if exception is known, the related http response is set.
        /// Unknown, unhalted exceptions are resulting in setting 500 Internal Error response.
        /// </summary>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            //TODO: Clear this code.
            //var exceptionType = exception.GetType();

            switch (exception)
            {
                    case NotFoundException:
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case BadRequestException:
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            context.Response.StatusCode,
                            exception.Message,
                        });
                        break;
                    default:
                        //Log.Error(exception, $"{ exception.Message } at { exception.Source }.");
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            //context.Response.StatusCode,
                            exception.Message,
                            exception.Source
                        });
                        break;
            }

            // if(exceptionType == typeof(NotFoundException))
            // {
            //    
            // }
            // else if (exceptionType == typeof(BadRequestException))
            // {
            //     context.Response.ContentType = "application/json";
            //     context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //     await context.Response.WriteAsJsonAsync(new
            //     {
            //         context.Response.StatusCode,
            //         exception.Message,
            //     });
            // }
            // else
            // {
            //     //Log.Error(exception, $"{ exception.Message } at { exception.Source }.");
            //     context.Response.ContentType = "application/json";
            //     context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //     await context.Response.WriteAsJsonAsync(new
            //     {
            //         //context.Response.StatusCode,
            //         exception.Message,
            //         exception.Source
            //     });
            // }
        }
    }
}
