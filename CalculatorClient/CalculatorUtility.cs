namespace SpecificLayout
{
    public class CalculatorUtility
    {
        public CalculatorUtility() { }

        /*
         * EvaluateExpression()         
         *  returns the evaluation of a MathExpression
         */
        public static decimal EvaluateExpression(decimal left, string operation, decimal right)
        {
            try
            {


                //need to look at what's empty to decide how to calculate.
                if (operation == "+") return left + right;
                else if (operation == "-") return left - right;
                else if (operation == "*") return left * right;
                else if (operation == "/")
                {
                    if (right == 0)
                        return 0;
                    else
                        return left / right;
                }
                else
                {
                    Console.WriteLine("Invalid calculation!");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Overflowing"!);
            }
        }
        public static string FormatNumber(decimal? number) => FormatNumber(number ?? 0);
        public static string FormatNumber(decimal number)
        {
            //return number.ToString("0.##");
            string result = number.ToString();
            if(result.Contains('.'))
            {
                result = result.TrimEnd('0');
                if (result.EndsWith("."))
                {
                    result = result.TrimEnd('.');
                }
            }
            else
            {
                result=number.ToString();
            }

            return result;
        }
    }


}