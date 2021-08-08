namespace BoxingStore.Data
{
    public class DataConstants
    {
        public class User
        {
            public const int FullNameMinLength = 5;
            public const int FullNameMaxLength = 40;
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 40;
        }
         
        public const int ProductQuantityMin = 0;
        public const double ProductPriceMin = 0.01;
        public const string ProductQuantityErrMsg = "Quantity should not be negative";
        public const string ProductPriceErrMsg = "Price should be more than 0$";
        public const string AllSizesAreZeroErrMsg = "Quantity cannot be 0 for all sizes";
    }
}
