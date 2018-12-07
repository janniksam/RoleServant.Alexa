using System.ComponentModel.DataAnnotations;

namespace RoleShuffle.Application.Abstractions.Validation
{
    public class NullOrTwoAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (int.TryParse(value.ToString(), out var number))
            {
                if (number == 0)
                {
                    return true;
                }

                if (number == 2)
                {
                    return true;
                }
            }
            return false;

        }
    }
}