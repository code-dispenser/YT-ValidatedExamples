using Validated.Core.Builders;
using Validated.Core.Types;
using Validated.Core.Validators;
using ValidatedExamples.Common.Models;

namespace ValidatedExamples.Common.ConfiguredValidators;

public static class AddressValidators
{
    public static MemberValidator<string> AddressLineValidator { get; }
    public static MemberValidator<string> TownCityValidator    { get; }
    public static MemberValidator<string> CountyValidator      { get; }
    public static MemberValidator<string> UKPostcodeValidator  { get; }

    static AddressValidators()
    {
        AddressLineValidator = MemberValidators.CreateStringRegexValidator(@"^(?=.{5,250}$)(?!.* {2})(?!.*[,\-']{2})[A-Za-z0-9][A-Za-z0-9 ,\-\n']+[A-Za-z0-9]$", "AddressLine",
                                                                     "Address Line", "Must start with a letter or number and be 5 to 250 characters in length.");

        TownCityValidator   = MemberValidators.CreateStringRegexValidator(@"^(?=.{3,100}$)[A-Z](?!.* {2})(?!.*'{2})(?!.*-{2})[\-A-Za-z ']+[a-z]+$", "TownCity",
                                                                        "Town / City", "Must start with a capital letter and be between 3 to 100 characters in length.");

        CountyValidator     = MemberValidators.CreateStringRegexValidator(@"^(?=.{3,100}$)[A-Z](?!.* {2})(?!.*'{2})(?!.*-{2})[\-A-Za-z ']+[a-z]+$", "County",
                                                                      "County", "Must start with a capital letter and be between 3 to 100 characters in length.");

        UKPostcodeValidator = MemberValidators.CreateStringRegexValidator(@"^(GIR 0AA)|((([ABCDEFGHIJKLMNOPRSTUWYZ][0-9][0-9]?)|(([ABCDEFGHIJKLMNOPRSTUWYZ][ABCDEFGHKLMNOPQRSTUVWXY][0-9][0-9]?)|(([ABCDEFGHIJKLMNOPRSTUWYZ][0-9][ABCDEFGHJKSTUW])|([ABCDEFGHIJKLMNOPRSTUWYZ][ABCDEFGHKLMNOPQRSTUVWXY][0-9][ABEHMNPRVWXY])))) [0-9][ABDEFGHJLNPQRSTUWXYZ]{2})$",
                                                                          "Postcode", "Postcode", "Must be a valid UK formatted postcode.");
    }

    public static EntityValidator<AddressDto> AddressValidator()

        => ValidationBuilder<AddressDto>.Create()
                    .ForMember(a => a.AddressLine, AddressLineValidator)
                        .ForMember(a => a.TownCity, TownCityValidator)
                            .ForMember(a => a.County, CountyValidator)
                                .ForNullableStringMember(a => a.NullablePostcode, UKPostcodeValidator)
                                   .Build();

}
