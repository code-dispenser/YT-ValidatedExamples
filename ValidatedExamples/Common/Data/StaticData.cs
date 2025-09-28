using ValidatedExamples.Common.Models;

namespace ValidatedExamples.Common.Data;

public static class StaticData
{
    public static ContactDto CreateContactObjectGraph()
    {
        var dob = new DateOnly(2000, 1, 1);
        var olderDob = new DateOnly(2000, 1, 2);

        var nullableAge = DateTime.Now.Year - dob.Year - (DateTime.Now.DayOfYear < dob.DayOfYear ? 1 : 0);
        var age = DateTime.Now.Year - dob.Year - (DateTime.Now.DayOfYear < dob.DayOfYear ? 1 : 0);

        AddressDto address = new() { AddressLine = "Some AddressLine", County = "Some County", NullablePostcode="Some PostCode", TownCity="Some Town" };
        AddressDto nullableAddress = address with { NullablePostcode="SW1A 1AA", County="1" };

        List<ContactMethodDto> contactMethods = [new() { MethodType = "MethodTypeOne", MethodValue = "MethodValueOne" }, new() { MethodType = "MethodTypeTwo", MethodValue = "MethodValueTwo" }];


        return new()
        {
            Address = address,
            NullableAddress = nullableAddress,
            NullableAge = nullableAge,
            Age = age,
            ContactMethods = contactMethods,
            DOB = dob,
            CompareDOB = olderDob,
            Email = "john.doe@email.com",
            FamilyName="Doe",
            GivenName = "John",
            NullableMobile="123456789",
            Title="Mr",
            Entries = []
        };

    }
}
