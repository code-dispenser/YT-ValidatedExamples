using System.Text.RegularExpressions;
using Validated.Core.Common.Constants;
using Validated.Core.Types;

namespace ValidatedExamples.Examples;

/*
    * Will create a custom regex MemberValidator as the built-in one does not have a parameter for RegexOptions which you may want.
    * Rather than use a generic T and use ToString on the passed in value will be just make this for strings.
 */ 
public static class Custom_MemberValidator_Factory
{
    public static MemberValidator<string> CustomRegexValidator(string regexPattern, RegexOptions regexOptions, string failureMessage, string propertyName, string displayName)

        => (valueToValidate, path, compareTo, cancellationToken) =>
        {
            if (String.IsNullOrWhiteSpace(valueToValidate)) return Task.FromResult(Validated<string>.Invalid(new InvalidEntry(failureMessage, path, propertyName, displayName, CauseType.Validation)));
              
            var result = Regex.IsMatch(valueToValidate ?? String.Empty, regexPattern, regexOptions)
                                ? Validated<string>.Valid(valueToValidate!)
                                    : Validated<string>.Invalid(new InvalidEntry(failureMessage, path, propertyName, displayName, CauseType.Validation));

            return Task.FromResult(result);
        };

    /*
        * The above factory can now be used to create regex validators for any property, field or just value that you want validating. 
        
        * Lets assume we have a property called Email on a few of our objects so we will create an EmailValidator from our Regex factory, but you can pass in params if need be.
    */

    public static MemberValidator<string> EmailValidator()

        => CustomRegexValidator(@"^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-']+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.[a-zA-Z]{2,63}$", RegexOptions.None, "Invalid email format", "Email", "Email");

    public static async Task Run()
    {
        var goodEmailFormat = "john.doe@email.com";
        var badEmailFormat  = "john.doe@home";

        var emailValidator = EmailValidator();//This will create the validator each time its called, we can make this better by making it a property of a static class and creating it in a static constructor

        var validatedGoodFormat = await emailValidator(goodEmailFormat);
        var validatedBadFormat  = await emailValidator(badEmailFormat);

        await Console.Out.WriteLineAsync($"Is email valid: {validatedGoodFormat.IsValid} - Failures: {String.Join(", ", validatedGoodFormat.Failures.Select(f => f.FailureMessage))}\r\n");

        await Console.Out.WriteLineAsync($"Is email valid: {validatedBadFormat.IsValid} - Failures: {String.Join(", ", validatedBadFormat.Failures.Select(f => f.FailureMessage))}\r\n");
    }
}
