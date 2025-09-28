using ValidatedExamples.Examples;

namespace ValidatedExamples;

internal class Program
{
    static async Task Main()
    {
        await The_Validated_Type.Run();

        await The_MemberValidator_Delegate.Run();

        await Custom_MemberValidator_Factory.Run();

        await The_AndThen_Extension.Run();

        await The_EntityValidator_Delegate.Run();

        await The_ValidationBuilder.Run();

        Console.ReadLine();
    }

}