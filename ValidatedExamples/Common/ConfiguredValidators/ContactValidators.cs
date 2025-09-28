using Validated.Core.Common.Constants;
using Validated.Core.Extensions;
using Validated.Core.Types;
using Validated.Core.Validators;
using ValidatedExamples.Common.Models;

namespace ValidatedExamples.Common.ConfiguredValidators;


public static class ContactValidators
{
    public static MemberValidator<string> TitleValidator          { get; }
    public static MemberValidator<string> GivenNameValidator      { get; }
    public static MemberValidator<string> FamilyNameValidator     { get; }
    public static MemberValidator<int> AgeValidator               { get; }
    public static MemberValidator<ContactDto> CompareDOBValidator { get; }
    public static MemberValidator<DateOnly> DOBValidator          { get; }
    public static MemberValidator<string> UKMobileValidator       { get; }
    public static MemberValidator<string> EntryValidator          { get; }

    public static MemberValidator<List<string>> EntryCountValidator { get; }




    /*
        * All of these validator are good for multiple things. Validating individual values, used in the Validated.Core's ValidationBuilder
        * or as in this demo the BlazorValidationBuilder
    */
    static ContactValidators()
    {
        TitleValidator      = MemberValidators.CreateStringRegexValidator("^(Mr|Mrs|Ms|Dr|Prof)$", "Title", "Title", "Must be one of Mr, Mrs, Ms, Dr, Prof");

        GivenNameValidator  = MemberValidators.CreateStringRegexValidator(@"^(?=.{2,50}$)[A-Z]+['\- ]?[A-Za-z]*['\- ]?[A-Za-z]+$", "GivenName", "First name", "Must start with a capital letter and be between 2 and 50 characters in length");

        FamilyNameValidator = MemberValidators.CreateStringRegexValidator(@"^[A-Z]+['\- ]?[A-Za-z]*['\- ]?[A-Za-z]*$", "FamilyName", "Surname", "Must start with a capital letter")
                                .AndThen(MemberValidators.CreateStringLengthValidator(2, 50, "FamilyName", "Surname", "Must be between 2 and 50 characters in length"));

        AgeValidator        = MemberValidators.CreateRangeValidator(25, 50, "Age", "Age", "Must be between 25 and 50");

        CompareDOBValidator = MemberValidators.CreateMemberComparisonValidator<ContactDto, DateOnly>(c => c.CompareDOB, c => c.DOB, CompareType.LessThan, "Compare DOB", "Must be less than Date of birth");

        DOBValidator        = MemberValidators.CreateCompareToValidator<DateOnly>(DateOnly.Parse("2022-01-01"), CompareType.EqualTo, "DOB", "Date of birth", "Must be equal to 2022-01-01");

        UKMobileValidator   = MemberValidators.CreateStringRegexValidator(@"^(?:\+[1-9]\d{1,3}[ -]?7\d{9}|07\d{9})$", "Mobile", "Mobile", "Must be a valid UK mobile number format");

        EntryValidator      =  MemberValidators.CreateNotNullOrEmptyValidator<string>("Entry", "Entry", "Required, cannot be missing, null or empty");

        EntryCountValidator = MemberValidators.CreateCollectionLengthValidator<List<string>>(1, 3, "Entries", "Entries", $"Must contain between 1 and 3 items but the collection contained {FailureMessageTokens.ACTUAL_LENGTH} items");
    }
}

