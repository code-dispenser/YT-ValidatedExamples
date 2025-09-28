using Validated.Core.Types;
using ValidatedExamples.Common.ConfiguredValidators;
using ValidatedExamples.Common.Data;
using ValidatedExamples.Common.Models;

namespace ValidatedExamples.Examples;

/*
     * The EntityValidator delegate is for object graphs and although you could use it as shown below with the inline function
     * you generally would just use the ValidationBuilder which under the covers uses extensions that take a MemberValidator returning an 
     * EntityValidator for working with object graphs. These extension method also providing the path information, unlike below where we are adding
     * it ourselves.
     * 
     * The built-in MemberValidators offers comparison validators for two members but if you required comparisons against multiple members then
     * using an EntityValidator directly would solve that issue.
     
 */ 
public class The_EntityValidator_Delegate
{
    public static async Task Run()
    {
        EntityValidator<ContactDto> contactValidator = async (entity, path, context, cancellationToken) =>
        {
            var failures = new List<InvalidEntry>();

            if (string.IsNullOrEmpty(entity.Email)) failures.Add(new InvalidEntry("Email is required", String.Concat(path, ".", "Email"), "Email", "Email"));
               
            if (entity.Age < 18 || entity.Age > 120) failures.Add(new InvalidEntry("Age must be between 18 and 120", String.Concat(path, ".", "Age"), "Age", "Age"));

            var titleValidator = ContactValidators.TitleValidator;

            var validatedTitle = await titleValidator(entity.Title, String.Concat(path, ".", nameof(ContactDto.Title)), null, cancellationToken);

            if (validatedTitle.IsInvalid) failures.AddRange(validatedTitle.Failures);

            return  failures.Count == 0 ? Validated<ContactDto>.Invalid(failures) : Validated<ContactDto>.Valid(entity);
           
        };

        var contactData = StaticData.CreateContactObjectGraph();
        /*
            * Let set everything thing to fail 
        */ 
        contactData.Email = "";
        contactData.Title = "D";
        contactData.Age   = 17;

        var validated = await contactValidator(contactData, nameof(ContactDto));
    }
}
