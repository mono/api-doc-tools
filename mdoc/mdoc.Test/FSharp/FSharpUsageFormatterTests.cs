using Microsoft.FSharp.Core;
using Mono.Documentation.Updater;
using NUnit.Framework;

namespace mdoc.Test
{
    [TestFixture]
    [Category("FSharp")]
    public class FSharpUsageFormatterTests : BasicFSharpFormatterTests<FSharpUsageFormatter>
    {
        private static readonly FSharpUsageFormatter fSharpUsageFormatter = new FSharpUsageFormatter();
        protected override FSharpUsageFormatter formatter => fSharpUsageFormatter;

        #region Usage
        [Test]
        [Category("Usage")]
        [Category("Properties")]
        public void PropertyUsage_0() =>
            TestPropertySignature(typeof(Properties.MyPropertyClass2),
@"Properties.MyPropertyClass2.Property1",
nameof(Properties.MyPropertyClass2.Property1));

        [Test]
        [Category("Usage")]
        [Category("Properties")]
        [Category("DiscriminatedUnions")]
        public void PropertyUsage_1() =>
            TestPropertySignature(
                typeof(DiscriminatedUnions.SizeUnion),
                "DiscriminatedUnions.SizeUnion.Small",
                nameof(DiscriminatedUnions.SizeUnion.Small));

        [Test]
        [Category("Usage")]
        [Category("Methods")]
        public void MethodUsage_0() =>
            TestMethodSignature(typeof(Methods.SomeType),
@"someType.SomeMethod (a, b, c)",
nameof(Methods.SomeType.SomeMethod));

        [Test]
        [Category("Usage")]
        [Category("Methods")]
        public void MethodUsage_1() =>
            TestMethodSignature(typeof(Methods.SomeType),
@"Methods.SomeType.SomeStaticMethod (a, b, c)",
nameof(Methods.SomeType.SomeStaticMethod));


        [Test]
        [Category("Usage")]
        [Category("Methods")]
        public void MethodUsage_2() =>
            TestMethodSignature(typeof(Methods.SomeType),
@"Methods.SomeType.SomeOtherStaticMethod2 a b c",
nameof(Methods.SomeType.SomeOtherStaticMethod2));

        [Test]
        [Category("Usage")]
        [Category("Methods")]
        public void MethodUsage_3() =>
            TestMethodSignature(typeof(Methods.SomeType),
@"Methods.SomeType.SomeOtherStaticMethod3 (a, b) c d",
nameof(Methods.SomeType.SomeOtherStaticMethod3));

        [Test]
        [Category("Usage")]
        [Category("Operators")]
        public void MethodUsage_4() =>
            TestMethodSignature(typeof(OperatorsOverloading.Vector),
"v * a",
"op_Multiply");

        [Test]
        [Category("Usage")]
        [Category("Operators")]
        public void MethodUsage_5() =>
            TestMethodSignature(typeof(OperatorsOverloading.Vector),
"- v",
"op_UnaryNegation");

        [Test]
        [Category("Usage")]
        [Category("Operators")]
        public void MethodUsage_6() =>
            TestMethodSignature(typeof(OperatorsOverloading.Vector),
"~~~ v",
"op_LogicalNot");

        [Test]
        [Category("Usage")]
        [Category("Operators")]
        public void MethodUsage_7() =>
            TestMethodSignature(typeof(OperatorsOverloading.Vector),
"start .. finish",
"op_Range");

        [Test]
        [Category("Usage")]
        [Category("Operators")]
        public void MethodUsage_8() =>
            TestMethodSignature(typeof(OperatorsOverloading.Vector),
"start .. step .. finish",
"op_RangeStep");

        [Test]
        [Category("Usage")]
        [Category("Operators")]
        public void MethodUsage_9() =>
            TestMethodSignature(typeof(OperatorsOverloading.Vector),
"a |+-+ v",
"op_BarPlusMinusPlus");

        [Test]
        [Category("Usage")]
        [Category("Operators")]
        public void MethodUsage_10() =>
            TestMethodSignature(typeof(Operators),
"(arg1, arg2, arg3) |||> func",
"op_PipeRight3");

        [Test]
        [Category("Usage")]
        [Category("Operators")]
        public void MethodUsage_11() =>
            TestMethodSignature(typeof(Operators),
"func <||| (arg1, arg2, arg3)",
"op_PipeLeft3");

        [Test]
        [Category("Usage")]
        [Category("Methods")]
        public void MethodUsage_12() =>
            TestMethodSignature(typeof(PatternMatching.PatternMatchingExamples),
"PatternMatching.PatternMatchingExamples.countValues list value",
"countValues");

        [Test]
        [Category("Usage")]
        [Category("Operators")]
        public void ConstructorUsage_0() =>
            TestMethodSignature(typeof(AbstractClasses.Circle),
"new AbstractClasses.Circle (x, y, radius)",
".ctor");
        #endregion

    }
}