using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesAPI.Exceptions
{
    /*
     * Custom Exception thrown when Company with existing CompanyCode is being added
    */

    public class CompanyAlreadyExistsException : ApplicationException
    {
        public CompanyAlreadyExistsException() { }
        public CompanyAlreadyExistsException(string message) : base(message) { }
    }
}
