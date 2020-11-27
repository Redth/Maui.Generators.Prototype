using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Maui.Generators
{
    [Generator]
    public class MauiRegistrarSourceGenerator : ISourceGenerator
    {
        List<IMauiRegistrarSourceGenerator> mauiRegistrarGenerators;

        public void Initialize(GeneratorInitializationContext context)
        {
            mauiRegistrarGenerators = new List<IMauiRegistrarSourceGenerator>
            {
                new MauiFontRegistrarSourceGenerator(),
            };
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var src = new StringBuilder();

            src.Append(@"
namespace Maui.Generated
{
    [global::System.CodeDom.Compiler.GeneratedCode(""Maui.Generators"", ""1.0.0.0"")]
    public static class MauiGeneratedRegistrar
    {
        public static void Register()
        {
");
            foreach (var g in mauiRegistrarGenerators)
                g.GenerateRegisterBodyCode(context, src);

            src.Append(
        @"}
    }
}");
            context.AddSource("Maui_Generated_MauiGeneratedRegistrar.cs", src.ToString());
        }
    }

    public interface IMauiRegistrarSourceGenerator
    {
        void GenerateRegisterBodyCode(GeneratorExecutionContext context, StringBuilder sourceBuilder);
    }

	public class MauiFontRegistrarSourceGenerator : IMauiRegistrarSourceGenerator
	{
		public void GenerateRegisterBodyCode(GeneratorExecutionContext context, StringBuilder sourceBuilder)
        {
            foreach (var file in context.AdditionalFiles)
            {
                var extension = Path.GetExtension(file.Path);

                if (extension.Equals(".ttf", StringComparison.InvariantCultureIgnoreCase)
                    || extension.Equals(".otf", StringComparison.InvariantCultureIgnoreCase))
                {
                    var alias = context.GetMSBuildItemMetadata(file, "FontAlias");

                    var filename = Path.GetFileName(file.Path);

                    var srcAlias = string.IsNullOrWhiteSpace(alias) ? "null" : $"\"{alias}\"";

                    sourceBuilder.AppendLine(
                        @"global::Xamarin.Forms.Internals.FontRegistrar.Register(
                            new global::Xamarin.Forms.ExportFontAttribute(""" + filename + @""")
                            { Alias = " + srcAlias + @" },
                            global::System.Reflection.Assembly.GetExecutingAssembly());");
                }
            }
        }
	}


    internal static class SourceGeneratorContextExtensions
    {
        public static string GetMSBuildItemMetadata(
            this GeneratorExecutionContext context,
            AdditionalText additionalText,
            string name,
            string defaultValue = default)
        {
            context.AnalyzerConfigOptions
                       .GetOptions(additionalText)
                       .TryGetValue($"build_metadata.AdditionalFiles.{name}", out var value);

            return value ?? defaultValue;
        }
    }
}