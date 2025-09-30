using Validated.Core.Common.Constants;
using Validated.Core.Types;

namespace ValidatedExamples.Examples;

public static class The_Validated_Type
{
    /*
        * The Validated<T> is the type that gets returned from all functions/delegates. It is either valid containing the valid value
        * or invalid containing a readonly list of InvalidEntries.
    */ 
    public static async Task Run()
    {
        var validResult   = Validated<string>.Valid("john.doe@mail.com");

        /*
            * To get the value out use either the Match method supplying two functions, one for the sad path and one for the happy path as it could be either  
        */ 

        var theValidValue = validResult.Match(onInvalid: failures => string.Join(", ", failures.Select(f => f.FailureMessage)), onValid: value => value);
        
        /*
            *  Or, you can supply a default value to use if the value is invalid via the GetValueOr method which will then return either the valid value or the supplied default value. 
        */ 
        
        var theValueToUse = theValidValue = validResult.GetValueOr("john.doe@secondary-email.com");

        await Console.Out.WriteLineAsync($"Valid result match outputs: {theValidValue} - The value to use outputs: {theValueToUse}\r\n");

        /*
            * Lets do the same for the invalid result 
        */

        var invalidResult = Validated<string>.Invalid(new InvalidEntry(FailureMessage: "Email format is invalid", Path: "Email", PropertyName: "Email",
                                                                       DisplayName: "Email Address", Cause: CauseType.Validation));

        var invalidValue = invalidResult.Match(onInvalid: failures => string.Join(", ", failures.Select(f => f.FailureMessage)), onValid: value => value);

        var defaultValue = invalidResult.GetValueOr("john.doe@secondary-email.com");

        await Console.Out.WriteLineAsync($"Invalid result match outputs: {invalidValue} - The value to use outputs: {defaultValue}\r\n");

        /*
            * You can also check the IsValid or IsInvalid properties to see the validity of the contents. 
        */

        var isValid   = validResult.IsValid;
        var isInvalid = invalidResult.IsInvalid;
    }
}
