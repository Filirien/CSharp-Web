﻿namespace WebServer.Server.Exceptions
{
    using System;
    using System.ComponentModel.DataAnnotations;


    public class BadRequestException : Exception
    {
        private const string InvalidRequestMessage = "Request is not valid.";

        public static object ThrowFromInvalidRequest() =>
            throw new BadRequestException(InvalidRequestMessage);

        public BadRequestException(string message)
            : base(message)
        {

        }
    }
}
