# Maui.Generators.Prototype
A playground with some conceptual generators that could be used in Maui / Single Project


# Experiments


## Shared Fonts

The generator can take items from your project such as:

```xml
<ItemGroup>
  <SharedFont Include="OpenSans-Regular.ttf" Alias="OpenSans" />
</ItemGroup>
```

Your font will automatically be included as an EmbeddedResource, and in addition, the source generator
will automatically generate code to register the font with Xamarin.Forms' registrar:

```csharp
using System.CodeDom.Compiler;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Maui.Generated
{
  [GeneratedCode("Maui.Generators", "1.0.0.0")]
  public class MauiGeneratedRegistrar
  {
    public static void Register()
    {
      FontRegistrar.Register(new ExportFontAttribute("OpenSans-Regular.ttf")
      {
        Alias = "OpenSans"
      }, Assembly.GetExecutingAssembly());
    }
  }
}
```

The expectation is that then Maui will attempt to find and invoke the `Maui.Generated.MauiGeneratedRegistrar` types in referenced assemblies and invoke the `Register()` method on them at runtime, causing the registrations to occur.

It would be ideal if we had a module initializer (this was proposed for C# 9 but hasn't been included yet) to automatically invoke this code, however a quick type lookup on each assembly is still faster than scanning for all assembly attributes in every assembly.  Plus, no attributes need to be specified by the developer.

Finally, if we have `<SharedFont ` type items as the entry point, we can add some globbing pattern rules to the single project targets by default so fonts could be included simply by convention, for example:

```
<ItemGroup>
  <SharedFont Include="Resources/Fonts/*.ttf;Resources/Fonts/*.otf" />
<ItemGroup>
```

Which would allow any `.ttf` or `.otf` font inside the project's `Resources/Fonts/` directory to be automatically included in the build, and have registration code generated and executed at runtime.


### More experiments to come!
