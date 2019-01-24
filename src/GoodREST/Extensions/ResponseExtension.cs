using GoodREST.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoodREST.Extensions
{
    public static class ResponseExtension
    {
        public static T ConvertExceptionAsError<T>(this T response, Exception ex) where T : IResponse
        {
            return ConvertExceptionAsError<T>(response, ex, null);
        }
        public static T ConvertExceptionAsError<T>(this T response, Exception ex, int? errorCode) where T : IResponse
        {
            return ConvertExceptionAsError<T>(response, ex, errorCode, null, null);
        }
        public static T ConvertExceptionAsError<T>(this T response, Exception ex, int? errorCode, ICollection<string> errors) where T : IResponse
        {
            return ConvertExceptionAsError<T>(response, ex, errorCode, errors, null);
        }

        public static T ConvertExceptionAsError<T>(this T response, int? errorCode, ICollection<string> errors, ICollection<string> warnings) where T : IResponse
        {
            return ConvertExceptionAsError<T>(response, null, errorCode, errors, warnings);
        }

        public static T ConvertExceptionAsError<T>(this T response, Exception ex, int? errorCode, ICollection<string> errors, ICollection<string> warnings) where T : IResponse
        {
            var errorCollection = new List<string>();
            var warningCollection = new List<string>();

            if (errors != null && errors.Any())
            {
                errorCollection.AddRange(errors);
            }

            if (warnings != null && warnings.Any())
            {
                warningCollection.AddRange(warnings);
            }
            if (ex != null)
            {
                errorCollection.Add(ex.GetBaseException().Message);
            }

            if (errorCode.HasValue)
            {
                response.HttpStatusCode = errorCode.Value;
            }
            else if (response.HttpStatusCode == 0 && !errorCode.HasValue && errors.Any())
            {
                response.HttpStatusCode = 500;
            }

            response.Warnings = warningCollection;
            response.Errors = errorCollection;
            return response;
        }


        public static T Ok<T>(this T response) where T : IResponse
        {
            response.HttpStatus = "Ok";
            response.HttpStatusCode = 200;

            return response;
        }

        public static T Unauthorized<T>(this T response) where T : IResponse
        {
            return Unauthorized(response, null);
        }

        public static T Unauthorized<T>(this T response, ICollection<string> errors) where T : IResponse
        {
            var errorCollection = new List<string>();

            if (errors != null && errors.Any())
            {
                errorCollection.AddRange(errors);
            }
            response.Errors = errorCollection;
            response.HttpStatus = "Unauthorized";
            response.HttpStatusCode = 401;

            return response;
        }

        public static T Forbidden<T>(this T response) where T : IResponse
        {
            return Forbidden(response, null);
        }
        public static T Forbidden<T>(this T response, ICollection<string> errors) where T : IResponse
        {
            var errorCollection = new List<string>();

            if (errors != null && errors.Any())
            {
                errorCollection.AddRange(errors);
            }
            response.Errors = errorCollection;
            response.HttpStatus = "Forbidden";
            response.HttpStatusCode = 403;

            return response;
        }

        public static T NotFound<T>(this T response) where T : IResponse
        {
            return NotFound(response, null);
        }
        public static T NotFound<T>(this T response, ICollection<string> errors) where T : IResponse
        {
            var errorCollection = new List<string>();

            if (errors != null && errors.Any())
            {
                errorCollection.AddRange(errors);
            }
            response.Errors = errorCollection;
            response.HttpStatus = "Not Found";
            response.HttpStatusCode = 404;

            return response;
        }
        public static T MethodNotAllowed<T>(this T response) where T : IResponse
        {
            return MethodNotAllowed(response, null);
        }
        public static T MethodNotAllowed<T>(this T response, ICollection<string> errors) where T : IResponse
        {
            var errorCollection = new List<string>();

            if (errors != null && errors.Any())
            {
                errorCollection.AddRange(errors);
            }
            response.Errors = errorCollection;
            response.HttpStatus = "Method Not Allowed";
            response.HttpStatusCode = 405;

            return response;
        }
        public static T Created<T>(this T response) where T : IResponse
        {
            return Created(response, null);
        }
        public static T Created<T>(this T response, ICollection<string> warnings) where T : IResponse
        {
            var warningCollection = new List<string>();

            if (warnings != null && warnings.Any())
            {
                warningCollection.AddRange(warnings);
            }
            response.Warnings = warningCollection;
            response.HttpStatus = "Created";
            response.HttpStatusCode = 201;

            return response;
        }
        public static T Accepted<T>(this T response) where T : IResponse
        {
            return Accepted(response, null);
        }
        public static T Accepted<T>(this T response, ICollection<string> warnings) where T : IResponse
        {
            var warningCollection = new List<string>();

            if (warnings != null && warnings.Any())
            {
                warningCollection.AddRange(warnings);
            }
            response.Warnings = warningCollection;
            response.HttpStatus = "Accepted";
            response.HttpStatusCode = 202;

            return response;
        }
    }
}
