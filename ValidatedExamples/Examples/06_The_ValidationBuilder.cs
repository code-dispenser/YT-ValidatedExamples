using Validated.Core.Builders;
using ValidatedExamples.Common.ConfiguredValidators;
using ValidatedExamples.Common.Data;
using ValidatedExamples.Common.Models;

namespace ValidatedExamples.Examples;

/*
    * The ValidationBuilder just simplifies creating a validator to validate an entire object graph to any depth.
    * 
    * For nested complex types, the simplest way to create a validator for the graph is just to use a separate builder
    * per complex type, which then gets added to its parent, as its N-Level deep.
*/

public static class The_ValidationBuilder
{
    /*
        * Our ContactDto has a collection of ContactMethods (complex type) and both nullable and non-nullable primitive properties and properties that are complex types.
        * 
        * We will use three builders on for each complex type, I already created the validators for the AddressDto and ContactMethodDto complex types using builders. 
        * Please see the AddressValidators and ContactMethodValidators files. 
        * 
        * I will create the ContactDto validator here which is the parent
        *
        *
        * Nullable ie. Property? means optional to the framework when used with the respective builder method. 
        * These method ensure that validation is skipped when the value is null and checked when not null. A non-nullable property when null
        * if it has a validator assigned to it, will fail validation with a required message.
    */ 
    public static async Task Run()
    {
        // I already created some member validators for the contactDto primitives

        var contactValidator = ValidationBuilder<ContactDto>.Create()
                                    .ForMember(c => c.Title, ContactValidators.TitleValidator)
                                    .ForMember(c => c.GivenName, ContactValidators.GivenNameValidator)
                                    .ForMember(c => c.FamilyName, ContactValidators.FamilyNameValidator)
                                    .ForMember(c => c.DOB, ContactValidators.DOBValidator)
                                    .ForComparisonWithMember(c => c.CompareDOB, ContactValidators.CompareDOBValidator)
                                    .ForNullableStringMember(c => c.NullableMobile, ContactValidators.UKMobileValidator)
                                    .ForNullableMember(c => c.NullableAge, ContactValidators.AgeValidator)
                                    .ForMember(c => c.Age, ContactValidators.AgeValidator)
                                    .ForNestedMember(c => c.Address, AddressValidators.AddressValidator())
                                    .ForNullableNestedMember(c => c.NullableAddress, AddressValidators.AddressValidator())
                                    .ForEachPrimitiveItem(c => c.Entries, ContactValidators.EntryValidator) //Checks each item.
                                    .ForCollection(c => c.Entries, ContactValidators.EntryCountValidator)   //Checks collection i.e the number of items
                                    .ForEachCollectionMember(c => c.ContactMethods, ContactMethodsValidators.ContactMethodsValidator())
                                    .Build();

        /*
            * Alter the data for any property that has a validator to see the effects 
        */

        var contactData = StaticData.CreateContactObjectGraph();

        contactData.Entries = ["EntryOne", "EntryTwo", "EntryThree", ""];

        var validatedContact = await contactValidator(contactData);

        await Console.Out.WriteLineAsync($"Is contact valid: {validatedContact.IsValid}\r\n" +
            $"Failures: {String.Join("\r\n", validatedContact.Failures.Select(f => String.Concat(f.Path, " - ", f.DisplayName, " - ", f.FailureMessage)))}");
    }
}
