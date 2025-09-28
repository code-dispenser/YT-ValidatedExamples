using Validated.Core.Builders;
using Validated.Core.Types;
using Validated.Core.Validators;
using ValidatedExamples.Common.Models;

namespace ValidatedExamples.Common.ConfiguredValidators;

public static class ContactMethodsValidators
{
    public static MemberValidator<string> ContactMethodValueValidator { get; }
    public static MemberValidator<string> ContactMethodTypeValidator  { get; }

    static ContactMethodsValidators()
    {
        ContactMethodValueValidator = MemberValidators.CreateNotNullOrEmptyValidator<string>("MethodValue", "Method value", "Required, cannot be missing, null or empty");

        ContactMethodTypeValidator  = MemberValidators.CreateNotNullOrEmptyValidator<string>("MethodType", "Method type", "Required, cannot be missing, null or empty");
    }

    public static EntityValidator<ContactMethodDto> ContactMethodsValidator()

        => ValidationBuilder<ContactMethodDto>.Create()
                    .ForMember(c => c.MethodType, ContactMethodTypeValidator)
                        .ForMember(c => c.MethodValue, ContactMethodValueValidator)
                            .Build();
                            
}
