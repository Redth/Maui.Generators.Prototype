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


More experiments to come!
