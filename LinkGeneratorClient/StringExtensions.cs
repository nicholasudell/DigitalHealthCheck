using PhoneNumbers;

namespace LinkGeneratorClient
{
    public static class StringExtensions
    {
        /// <summary>
        /// Checks if a phone number is a valid UK number.
        /// </summary>
        /// <param name="phoneNumber">The phone number to check</param>
        /// <returns>True if the phone number is a valid UK number; otherwise false.</returns>
        public static bool IsValidPhoneNumber(this string phoneNumber)
        {
            try
            {
                var phone = PhoneNumberUtil.GetInstance();
                var number = phone.Parse(phoneNumber, "GB");
                return phone.IsPossibleNumber(number) && phone.IsValidNumber(number);
            }
            catch (NumberParseException)
            {
                return false;
            }
        }
    }
}
