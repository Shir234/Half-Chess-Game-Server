using SERVER.Data;
using System.ComponentModel.DataAnnotations;

namespace SERVER.Attributes
{
    public class UniqueIDAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !(value is int id))
                return ValidationResult.Success;

            // Access DbContext via DI
            var dbContext = validationContext.GetService<SERVERContext>();

            // Check if ID already exists
            bool idExists = dbContext.TblPlayers.Any(p => p.Id == id);

            return idExists
                ? new ValidationResult("The chosen ID is already taken. Please select another.")
                : ValidationResult.Success;
        }

    }
}
