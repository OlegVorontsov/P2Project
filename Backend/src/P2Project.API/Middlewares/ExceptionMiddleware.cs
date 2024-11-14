﻿using Microsoft.AspNetCore.Http;
using P2Project.API.Response;
using System.Globalization;

namespace P2Project.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                var responseError = new ResponseError(
                    "server.internal",
                    ex.Message,
                    null);
                var envelope = Envelope.Error([responseError]);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(envelope);
            }
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}