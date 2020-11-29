using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MauiSourceGenerators
{
    class Program
    {
        static void Main(string[] args)
        {
            // Not needed with module initializer in generated code
            // RegisterRegistrarInAssemblies();

            var font = GetFont("OpenSans-Regular.ttf");

            Console.WriteLine($"Font: {font.attribute.FontFileName} Alias: {font.attribute.Alias}");
            
            Console.ReadLine();
        }

        // Just some code to peek into the internal fonts and test if the registration actually happened
        // You wouln't actually have this code in your app
        static (global::Xamarin.Forms.ExportFontAttribute attribute, global::System.Reflection.Assembly assembly) GetFont(string name)
        {
            var fontRegistrarType = typeof(global::Xamarin.Forms.Internals.FontRegistrar);
            var embeddedFontsField = fontRegistrarType.GetField("EmbeddedFonts", BindingFlags.Static | BindingFlags.NonPublic);

            var embeddedFonts = (Dictionary<string, (global::Xamarin.Forms.ExportFontAttribute attribute, global::System.Reflection.Assembly assembly)>)embeddedFontsField.GetValue(null);

            return embeddedFonts[name];
        }

        // Not needed with module initializer in generated code
        // static void RegisterRegistrarInAssemblies()
        // {
        //     foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        //     {
        //         // Skip System
        //         if (asm.FullName.StartsWith("System.", StringComparison.OrdinalIgnoreCase))
        //             continue;

        //         var t = asm.GetType("Maui.Generated.MauiGeneratedRegistrar");

        //         var m = t?.GetMethod("Register");

        //         m?.Invoke(null, null);
        //     }
        // }
    }
}
