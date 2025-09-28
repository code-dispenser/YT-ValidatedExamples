using Validated.Core.Types;

namespace ValidatedExamples.Examples;

/*
    * The MemberValidator delegate is the main building block, if used on its own, you may not need to supply the optional params.
    * When used with the ValidationBuilder or the ValidatorExtension methods the path will be populated for you and used in the InvalidEntries to show the path from the root object ot the 
    * failing property including the index if its ina collection.
    *  
    * public delegate Task<Validated<T>> MemberValidator<T>(T memberValue, string path= "", T? compareTo = default, CancellationToken cancellationToken = default) where T : notnull;
    * 
    * The framework uses the compareTo value when comparing values prior to object creation such as with a ValueObject that should not be created in an invalid state.
*/
public static class The_MemberValidator_Delegate
{
    /*
        * We will create a simple inline custom validator for the example, but generally you will create standard methods with params for what you want to set in your custom validators
        * as seen in later examples
    */ 
    public static async Task Run()
    {
        /*
            * All we are doing is creating a function that returns a function that accepts a value, optional path, optional compareTo Value and on optional cancellation token as of the delegate signature,
            * but we are just discarding any optional parameters.
         */

        MemberValidator<string> emailValidator = (emailValue, _, _, _) =>
        {
            var result = emailValue.Contains('@') 
                            ? Validated<string>.Valid(emailValue)
                                : Validated<String>.Invalid(new InvalidEntry("Invalid email format"));//Only the failure message is required, we will omit the other properties
            
            return Task.FromResult(result);
        };

        var emailAddress = "badAddress.com";
        var theResult    = await emailValidator(emailAddress);// we are not supplying the optional values so the defaults would be used

        await Console.Out.WriteLineAsync($"Is email valid: {theResult.IsValid} - Failures: {String.Join(", ", theResult.Failures.Select(f => f.FailureMessage))}\r\n");

    }
}
