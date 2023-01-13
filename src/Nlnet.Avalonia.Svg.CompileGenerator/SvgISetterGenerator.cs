﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Nlnet.Avalonia.Svg.CompileGenerator
{
    [Generator]
    public class SvgISetterGenerator : ISourceGenerator
    {
        // 语法接收器，将在每次生成代码时被按需创建
        private class SyntaxReceiver : ISyntaxReceiver
        {
            public List<ClassDeclarationSyntax> CandidateClasses { get; } = new List<ClassDeclarationSyntax>();

            // 编译中在访问每个语法节点时被调用，我们可以检查节点并保存任何对生成有用的信息
            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                // 将具有至少一个 Attribute 的任何接口作为候选
                if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax && classDeclarationSyntax.AttributeLists.Count > 0)
                {
                    CandidateClasses.Add(classDeclarationSyntax);
                }
            }
        }

        // ReSharper disable once InconsistentNaming
        private const string SetterGeneratorAttributeCsFileName = "SetterGeneratorAttribute.g.cs";
        private const string SetterGeneratorAttributeFullName   = "Nlnet.Avalonia.Svg.CompileGenerator.SetterGeneratorAttribute";
        private const string SetterGeneratorAttributeText = @"// <auto-generated/>
using System;

namespace Nlnet.Avalonia.Svg.CompileGenerator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class SetterGeneratorAttribute : Attribute
    {
        public string PropertyName { get; set; }

        public string PropertyTypeName { get; set; }

        public bool IsNullable { get; set; }

        public SetterGeneratorAttribute(string propertyName, string propertyTypeName, bool isNullable = true)
        {
            PropertyName = propertyName;
            PropertyTypeName = propertyTypeName;
            IsNullable = isNullable;
        }
    }
}";

        public void Initialize(GeneratorInitializationContext context)
        {
            //Debugger.Launch();

            // 注册一个语法接收器，会在每次生成时被创建
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            // 添加 Attribute 文本
            context.AddSource(SetterGeneratorAttributeCsFileName, SourceText.From(SetterGeneratorAttributeText, Encoding.UTF8));

            // 获取先前的语法接收器 
            if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
            {
                return;
            }

            if (!(context.Compilation is CSharpCompilation csharpCompilation))
            {
                throw new Exception($"{nameof(SvgISetterGenerator)} only support C#.");
            }

            // 创建处目标名称的属性
            var options     = csharpCompilation.SyntaxTrees[0].Options as CSharpParseOptions;
            var compilation = context.Compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(SetterGeneratorAttributeText, Encoding.UTF8), options));

            // 获取新绑定的 Attribute
            var attributeSymbol = compilation.GetTypeByMetadataName(SetterGeneratorAttributeFullName);
            if (attributeSymbol == null)
            {
                throw new Exception($"Can not find the {SetterGeneratorAttributeFullName}");
            }

            // 遍历候选接口，只保留有 SetterGeneratorAttribute 标注的接口
            foreach (var @class in receiver.CandidateClasses)
            {
                //foreach (AttributeListSyntax attributeListSyntax in @class.AttributeLists)
                //{
                //    foreach (AttributeSyntax attribute in attributeListSyntax.Attributes)
                //    {
                //        var name = attribute.Name.ToFullString();
                //        var name2 = attribute.Name.ToString();
                //    }
                //}

                var model         = compilation.GetSemanticModel(@class.SyntaxTree);
                var typeSymbol    = model.GetDeclaredSymbol(@class);
                var attributeData = typeSymbol?.GetAttributes().FirstOrDefault(attr => attributeSymbol.Equals(attr.AttributeClass, SymbolEqualityComparer.Default));
                if (attributeData == null || attributeData.ConstructorArguments.Length != 3)
                {
                    continue;
                }

                var source = GenerateInterfaceAndFactory(typeSymbol, attributeData);
                context.AddSource($"I{typeSymbol.Name}.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        private static string GenerateInterfaceAndFactory(ISymbol classSymbol, AttributeData attributeData)
        {
            if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            {
                // 必须在顶层，产生诊断信息
                return null;
            }

            var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            var property      = attributeData.ConstructorArguments[0].Value?.ToString();
            var propertyType  = attributeData.ConstructorArguments[1].Value?.ToString();
            var isNullable    = attributeData.ConstructorArguments[2].Value?.ToString();

            propertyType = Capitalize(propertyType);
            var nullablePropertyType = isNullable == true.ToString() ? $"{propertyType}?" : propertyType;

            // 开始构建要生成的代码
            var source = new StringBuilder($@"// <auto-generated/>
using System;
using System.Xml;
using Avalonia.Media;
using Nlnet.Avalonia.Svg;

#nullable enable

namespace {namespaceName}
{{
    [Name(SvgProperties.{property})] public class {classSymbol.Name}Factory : AbstractSetterFactory<{classSymbol.Name}> {{ }}

    public interface I{classSymbol.Name} : IDeferredAdder
    {{
        public {propertyType}? {property} {{ get; set; }}

        public void Parse(XmlAttributeCollection attrs)
        {{
            this.ParseOrDefer<I{classSymbol.Name}, {nullablePropertyType}>(attrs, SvgProperties.{property}, Parsers.TryTo{propertyType}, (setter, value) => setter.{property} = value);
        }}
    }}
}}");

            return source.ToString();
        }

        private static string Capitalize(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return s[0].ToString().ToUpper() + s.Substring(1);
        }
    }
}
