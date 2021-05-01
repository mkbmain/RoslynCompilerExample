using System.Text;

namespace Compute
{
    public static class CodeBuilder
    {
        public static string BaseSequenceFormulaBuilder(int baseValue, string sequence)
        {
            var formulaBuilder = new StringBuilder();
            var chars = sequence.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                var item = chars[i];
                var positionInReverse = chars.Length - i;

                if (positionInReverse > 1)
                {
                    formulaBuilder.Append(
                        $"{(formulaBuilder.Length > 0 ? "+" : "")}({item}*(System.Math.Pow({baseValue},{positionInReverse-1})))");
                    continue;
                }

                formulaBuilder.Append($"{(formulaBuilder.Length > 0 ? "+" : "")}({item})");
            }

            return formulaBuilder.ToString();
        }

        public static SourceCodeModel<T> BuildFunctionCode<T>(string returnValue)
        {
            return new SourceCodeModel<T>
            {
                Code =
                    $"namespace OnFlyCompiled{{public class Calculator{{public {typeof(T).Namespace}.{typeof(T).Name} Calc(){{return {returnValue};}}}}}}",
                NameSpace = "OnFlyCompiled",
                Class = "Calculator",
                Method = "Calc",
            };
        }
    }
}