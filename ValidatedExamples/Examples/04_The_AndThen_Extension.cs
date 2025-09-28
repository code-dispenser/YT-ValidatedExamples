using Validated.Core.Extensions;
using Validated.Core.Validators;

namespace ValidatedExamples.Examples;

public static class The_AndThen_Extension
{
    /*
        * The AndThen extension makes it possible to chain validators together for the same member form a single validator.
        * This is the preferred way to have multiple validations occur on the same property/member
        * 
        * AndThen works on 
    */ 
    public static async Task Run()
    {
        var nameRegexValidator  = MemberValidators.CreateStringRegexValidator(@"^[A-Z]+['\- ]?[A-Za-z]*['\- ]?[A-Za-z]*$", "FamilyName", "Surname", "Must start with a capital letter");
        var nameLengthValidator = MemberValidators.CreateStringLengthValidator(2, 50, "FamilyName", "Surname", "Must be between 2 and 50 characters in length");

        var familyNameValidator = nameRegexValidator.AndThen(nameLengthValidator);

        // or just simply familyNameValidator =  MemberValidators.CreateStringRegexValidator(@"^[A-Z]+['\- ]?[A-Za-z]*['\- ]?[A-Za-z]*$", "FamilyName", "Surname", "Must start with a capital letter")
        //                                             .AndThen(MemberValidators.CreateStringLengthValidator(2, 50, "FamilyName", "Surname", "Must be between 2 and 50 characters in length"));

        //lets fail both checks.

        var validatedFamilyName = await familyNameValidator("b");

        await Console.Out.WriteLineAsync($"Is surname valid: {validatedFamilyName.IsValid} - Failures:\r\n{String.Join("\r\n", validatedFamilyName.Failures.Select(f => f.FailureMessage))}\r\n");
    }
}
