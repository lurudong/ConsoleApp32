using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SourceGenerator1
{
    /// <summary>
    /// 
    /// </summary>

    [Generator(LanguageNames.CSharp)]
    public sealed class EnumDescriptionAttributeGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {



            // 获取所有的 C# 语法树
            var syntaxTrees = context.Compilation.SyntaxTrees;

            foreach (var syntaxTree in syntaxTrees)
            {

                // 获取语法树的根节点
                var root = syntaxTree.GetRoot() as CompilationUnitSyntax;


                //不存在的话就添
                if (!root.Usings.Any(o => o.Name.ToFullString() == SyntaxFactory.ParseName("System.ComponentModel").ToFullString()))
                {
                    // 添加 using System.ComponentModel; 引用空间
                    var usingDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.ComponentModel"));
                    root = root.AddUsings(usingDirective);
                }

                // 查找枚举成员并添加 Description 特性
                var updatedRoot = AddDescriptionAttributes(root);
                string modifiedCode = updatedRoot.NormalizeWhitespace().ToFullString();
                File.WriteAllText(syntaxTree.FilePath, modifiedCode);
                // 将更新后的语法树添加到生成上下文
                //context.AddSource($"Updated.A.gc", updatedRoot.NormalizeWhitespace().ToFullString());
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            //if (!Debugger.IsAttached)
            //{
            //    Debugger.Launch();
            //}
        }

        private CompilationUnitSyntax AddDescriptionAttributes(CompilationUnitSyntax root)
        {
            // 使用语法遍历器找到枚举声明
            var rewriter = new EnumDescriptionRewriter();
            var updatedRoot = rewriter.Visit(root);

            return (CompilationUnitSyntax)updatedRoot;
        }
    }



}

public class EnumDescriptionRewriter : CSharpSyntaxRewriter
{

    List<string> _nameSyntaxes = new List<string>()
    {
        { SyntaxFactory.ParseName("Description").ToFullString()},
        { SyntaxFactory.ParseName("DescriptionAttribute").ToFullString()}

    };

    public override SyntaxNode VisitEnumDeclaration(EnumDeclarationSyntax node)
    {

        // 获取整个语法树的 SemanticModel


        var isAddAttributes = node.AttributeLists.SelectMany(o => o.Attributes).Where(o => _nameSyntaxes.Contains(o.Name.ToFullString())).Any();
        if (!isAddAttributes)
        {

            var enumSummary = GetEnumSummary(node);

            //ExtractXmlCommentText(enumSummary);
            if (enumSummary != default)
            {

                var commentText = enumSummary;
                // 创建 Description 特性
                var attribute = SyntaxFactory.Attribute(SyntaxFactory.ParseName("Description"))
                    .AddArgumentListArguments(SyntaxFactory.AttributeArgument(
                        SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(commentText))));

                // 将特性添加到枚举类型声明
                var updatedNode = node.AddAttributeLists(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(attribute)));
                return base.VisitEnumDeclaration(updatedNode);
            }


        }

        return base.VisitEnumDeclaration(node);

    }

    public override SyntaxNode VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
    {
        // 获取枚举成员的注释内容
        //var memberSummary = node.GetLeadingTrivia()
        //    .FirstOrDefault(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) ||
        //                         t.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia));


        var isAddAttributes = node.AttributeLists.SelectMany(o => o.Attributes).Where(o => _nameSyntaxes.Contains(o.Name.ToFullString())).Any();

        if (!isAddAttributes)
        {
            var memberSummary = GetEnumSummary(node);

            if (memberSummary != default)
            {
                // 获取注释文本
                var commentText = memberSummary;

                // 创建 Description 特性
                var attribute = SyntaxFactory.Attribute(SyntaxFactory.ParseName("Description"))
                    .AddArgumentListArguments(SyntaxFactory.AttributeArgument(
                        SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(commentText))));

                // 将特性添加到枚举成员声明
                var updatedNode = node.AddAttributeLists(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(attribute)));
                return base.VisitEnumMemberDeclaration(updatedNode);
            }


        }
        return base.VisitEnumMemberDeclaration(node);
    }


    /// <summary>
    /// 获取注释内容
    /// </summary>
    private Func<CSharpSyntaxNode, string> GetLeadingTrivia = (trivia) =>
    {
        var summarys = trivia.GetLeadingTrivia().Where(o => o.IsKind(SyntaxKind.SingleLineCommentTrivia)).Select(o =>
          o.ToFullString().TrimStart(' ', '/', '\t', '\r', '\n'));
        return GetEnumSummary(summarys);

    };

    /// <summary>
    /// 获取枚举的注释内容
    /// </summary>
    /// <param name="enumDeclaration"></param>
    /// <returns></returns>
    private string GetEnumSummary(EnumDeclarationSyntax enumDeclaration)
    {
        return GetLeadingTrivia(enumDeclaration);
    }

    /// <summary>
    /// 获取枚举成员的注释内容
    /// </summary>
    /// <param name="enumDeclaration"></param>
    /// <returns></returns>
    private string GetEnumSummary(EnumMemberDeclarationSyntax enumDeclaration)
    {
        return GetLeadingTrivia(enumDeclaration);
    }
    private static string GetEnumSummary(IEnumerable<string> enumSummarys)
    {

        var enumSummary = string.Join("", enumSummarys.ToArray());
        string pattern = @"<summary>(.*?)</summary>";
        Regex regex = new Regex(pattern, RegexOptions.Singleline);
        Match match = regex.Match(enumSummary);
        return match.Groups[1]?.Value.Trim(); ;
    }
}


