using System;

namespace HousingAssociation.ExceptionHandling.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException() : base()
        {

        }

        public BadRequestException(string msg) : base(msg)
        {
            
        }
        
    }
}