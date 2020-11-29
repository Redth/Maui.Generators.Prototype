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
  internal class MauiGeneratedRegistrar
  {
    [ModuleInitializer]
    internal static void Register()
    {
      FontRegistrar.Register(new ExportFontAttribute("OpenSans-Regular.ttf")
      {
        Alias = "OpenSans"
      }, Assembly.GetExecutingAssembly());
    }
  }
}
```


Using the `[ModuleInitializer]` attribute on the generated `Register()`  method will cause it to be invoked automatically at runtime.  This should almost always work, however it should be noted that the module initializer marked methods are only guaranteed to run before any other code is invoked from its assembly.

If an assembly is doing some sort of handler registration, it most likely is doing the registration for implementations or resources it contains, therefore the module initializer will always be called before the implementations are used anyway.

If an assembly contains things that don't need to be referenced by code (ie: just some fonts and no actual code), it would need to use some sort of `Init()` call to avoid being linked out.  I haven't thought of any great scenario which causes module initializers to be a problem yet.

We need to consider more scenarios where the module initializer could be problematic for each use case of generated code.

Finally, if we have `<SharedFont ` type items as the entry point, we can add some globbing pattern rules to the single project targets by default so fonts could be included simply by convention, for example:

```
<ItemGroup>
  <SharedFont Include="Resources/Fonts/*.ttf;Resources/Fonts/*.otf" />
<ItemGroup>
```

Which would allow any `.ttf` or `.otf` font inside the project's `Resources/Fonts/` directory to be automatically included in the build, and have registration code generated and executed at runtime.


### More experiments to come!
