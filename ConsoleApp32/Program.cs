// See https://aka.ms/new-console-template for more information
// 读取C#源代码文件
using System.Text;
using System.ComponentModel;

//// 获取所有已加载的程序集
//string sourceCodeFilePath = "C:\\Users\\lurud\\source\\repos\\ConsoleApp32\\ConsoleApp32\\MyClass.cs"; // 替换成你的源代码文件路径
//// 读取源代码文件
//string code = File.ReadAllText(sourceCodeFilePath);
//// 创建SyntaxTree
//SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
//// 获取根节点
//var root = (CompilationUnitSyntax)tree.GetRoot();
//// 添加 using System.ComponentModel; 引用空间
//var usingDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.ComponentModel"));
//root = root.AddUsings(usingDirective);
//// 查找所有类
//var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
//// 遍历类并生成Description属性
//foreach (var @class in classes)
//{
//    // 获取类的注释
//    var classSummary = @class.GetLeadingTrivia().FirstOrDefault(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) || t.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia));
//    if (classSummary != null)
//    {
//        string description = classSummary.ToFullString().Trim();
//        // 提取XML注释文本部分
//        string xmlCommentText = ExtractXmlCommentText(description);
//        // 生成Description属性
//        var descriptionAttribute = SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("Description")).WithArgumentList(SyntaxFactory.ParseAttributeArgumentList($"(\"{xmlCommentText}\")"));
//        // 添加Description属性到类
//        var updatedClass = @class.AddAttributeLists(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(descriptionAttribute)));
//        // 替换类节点
//        root = root.ReplaceNode(@class, updatedClass);
//    }
//}
//// 将修改后的语法树重新生成为源代码
//string modifiedCode = root.NormalizeWhitespace().ToFullString();
// 将修改后的源代码写回源代码文件，覆盖原始文件内容
//File.WriteAllText(sourceCodeFilePath, modifiedCode);
// 将修改后的语法树重新生成为源代码
// 保存生成的代码到内存流
//using (var memoryStream = new MemoryStream())
//using (var streamWriter = new StreamWriter(memoryStream))
//{
//    streamWriter.Write(modifiedCode);
//    streamWriter.Flush();
//}
Console.ReadLine();
// 提取XML注释的文本部分
string ExtractXmlCommentText(string xmlComment)
{
    // 使用StringBuilder来构建注释文本
    var stringBuilder = new StringBuilder();
    using (var reader = new StringReader(xmlComment))
    {
        string line;
        while ((line = reader.ReadLine()) != null)
        { // 去除<summary>和</summary>标记并添加到StringBuilder中
            string trimmedLine = line.TrimStart(' ', '/', '\t');
            if (!trimmedLine.StartsWith("<summary>") && !trimmedLine.EndsWith("</summary>"))
            {
                stringBuilder.AppendLine(trimmedLine);
            }
        }
    }

    // 返回去除注释标记的注释文本
    return stringBuilder.ToString().Trim();
}