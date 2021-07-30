namespace BoxingStore.Services.CustomAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class ProductSizeAttribute : ValidationAttribute
    {
        public ProductSizeAttribute()
        {
            this.MinLength = ProductQuantityMin;
        }

        public int MinLength { get; private set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (Convert.ToInt32(value) < this.MinLength)
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }
    }
}