using ArithmeticChat.Interfaces;

namespace ArithmeticChat.Helpers
{
    public class NumberToWordsHelper : INumberToWordsHelper
    {
        private readonly string[] Units = { "Zero","One","Two","Three","Four","Five","Six","Seven","Eight","Nine","Ten",
        "Eleven","Twelve","Thirteen","Fourteen","Fifteen","Sixteen","Seventeen","Eighteen","Nineteen" };
        private static readonly string[] Tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        public string Convert(decimal number)
        {
            number = decimal.Round(number, 2, MidpointRounding.AwayFromZero);
            var sign = number < 0 ? "Minus " : "";
            number = Math.Abs(number);
            var whole = (long)Math.Truncate(number);
            var fraction = (int)((number - whole) * 100);
            var words = $"{sign}{IntegerToWords(whole)}";
            if (fraction > 0) words += $" point {TwoDigitsToWords(fraction)}";
            return words.Trim();
        }

        private string IntegerToWords(long n)
        {
            if (n == 0) return "Zero";
            string Under1000(long num)
            {
                var parts = new List<string>();
                if (num >= 100) { parts.Add(Units[num / 100] + " Hundred"); num %= 100; }
                if (num >= 20) { parts.Add(Tens[num / 10]); if (num % 10 != 0) parts.Add(Units[num % 10]); }
                else if (num > 0) parts.Add(Units[num]);
                return string.Join(" ", parts);
            }
            var partsAll = new List<string>();
            var groups = new (long div, string name)[] { (1_000_000_000, "Billion"), (1_000_000, "Million"), (1_000, "Thousand"), (1, "") };
            foreach (var (div, name) in groups)
            {
                if (n >= div)
                {
                    var chunk = n / div; n %= div;
                    if (chunk > 0) { var s = Under1000(chunk); if (!string.IsNullOrEmpty(s)) { partsAll.Add(s); if (!string.IsNullOrEmpty(name)) partsAll.Add(name); } }
                }
            }
            return string.Join(" ", partsAll);
        }

        private string TwoDigitsToWords(int two)
        {
            var s = two.ToString("00");
            var map = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
            return string.Join(" ", s.Select(ch => map[ch - '0']));
        }
    }
}
