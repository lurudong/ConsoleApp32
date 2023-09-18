using System.ComponentModel;

namespace ConsoleApp32
{
    [DescriptionAttribute("这是一个示例枚举")]
    /// <summary>
    /// 这是一个示例枚举
    /// </summary>
    public enum MyEnum
    {
        [Description("枚举成员")]
        /// <summary>
        /// 枚举成员
        /// </summary>
        Node,
        [Description("一个二个三个")]
        /// <summary>
        /// 一个
        /// 二个
        /// 三个
        /// </summary>
        One,
        [Description("我的自己")]
        /// <summary>
        /// 我的自己
        /// </summary>
        My,
        [Description("瞧瞧")]
        /// <summary>
        /// 瞧瞧
        /// </summary>
        Ha,
        [Description("测试")]
        /// <summary>
        /// 测试
        /// </summary>
        AAA
    }

    [Description("测试枚举")]
    /// <summary>
    /// 测试枚举
    /// </summary>
    public enum TestEnum
    {
        [Description("测试")]
        /// <summary>
        /// 测试
        /// </summary>
        Test,
        [Description("测试1")]
        /// <summary>
        /// 测试1
        /// </summary>
        Test1
    }
}