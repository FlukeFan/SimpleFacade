using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SimpleFacade.Exceptions;

namespace SimpleFacade.Validation
{
    public static class ExecutableValidator
    {
        public static void Validate(object dto)
        {
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(dto, context, results, true);

            if (valid)
                return;

            throw new FacadeException(results);
        }
    }
}
