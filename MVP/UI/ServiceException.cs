using System;

namespace MVP.UI
{
    public class ServiceException : Exception
    {
        public ServiceException(string message) : base(message)
        { }
    }
}
