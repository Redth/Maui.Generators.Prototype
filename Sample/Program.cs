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
            RegisterRegistrarInAssemblies();

            var font = GetFont("OpenSans-Regular.ttf");

            Console.WriteLine($"Font: {font.attribute.FontFileName} Alias: {font.attribute.Alias}");
            
            Console.ReadLine();
        }

        static (global::Xamarin.Forms.ExportFontAttribute attribute, global::System.Reflection.Assembly assembly) GetFont(string name)
        {
            var fontRegistrarType = typeof(global::Xamarin.Forms.Internals.FontRegistrar);
            var embeddedFontsField = fontRegistrarType.GetField("EmbeddedFonts", BindingFlags.Static | BindingFlags.NonPublic);

            var embeddedFonts = (Dictionary<string, (global::Xamarin.Forms.ExportFontAttribute attribute, global::System.Reflection.Assembly assembly)>)embeddedFontsField.GetValue(null);

            return embeddedFonts[name];
        }

        static void RegisterRegistrarInAssemblies()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Skip System
                if (asm.FullName.StartsWith("System.", StringComparison.OrdinalIgnoreCase))
                    continue;

                var t = asm.GetType("Maui.Generated.MauiGeneratedRegistrar");

                var m = t?.GetMethod("Register");

                m?.Invoke(null, null);
            }
        }
    }
}
