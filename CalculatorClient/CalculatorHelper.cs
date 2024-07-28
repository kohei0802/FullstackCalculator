using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using ClassLibrary1;

namespace SpecificLayout
{
    public class CalculatorHelper
    {
        //Constants
        const string STRINGFORMATTER = "0.################";

        //Core Variables
        public MathExpression currentExpression = new MathExpression();
        private string inputBuffer = "";

        //History Variables
        private List<MathExpression> historyList = new List<MathExpression>();


        //SQL variables
        const string SQL_DATA_SOURCE = "192.168.4.70";
        const string SQL_INITIAL_CATALOG = "kohei_db";
        const string SQL_USER_ID = "kohei";
        const string SQL_PASSWORD = "kohei";
        const string SQL_TABLE = "NewCalculatorResults";

        //Data Source
        IDataSource dataSource = null;

        public CalculatorHelper()
        {
            currentExpression.Left = 0;
            currentExpression.Operation = null;
            currentExpression.Right = null;
            currentExpression.Result = null;

            //dataSource = new SqlSource();
            dataSource = new CsvSource();
            //dataSource = new SqlEfSource();
            //dataSource = new RestApiSource();
            //dataSource = new SwaggerRestApi();
        }

        /*
         * Handle problematic input to inputBuffer, including input exceeding 16 digits 
         */
        public void Reset()
        {
            currentExpression.Operation = null;
            currentExpression.Right = null;
            currentExpression.Result = null;
        }

        public async Task AddHistory()
        {
            historyList.Add(currentExpression.GetCopy());
            await dataSource.InsertAsync(currentExpression);
        }
        public string HandleNumber(string input)
        {
            if (currentExpression.Result != null)
            {
                Reset();
            }

            if (inputBuffer.Length == 0)
            {
                if (input == ".")
                    inputBuffer = "0.";
                else
                    inputBuffer = input;
            }
            else if (!(input == "." && inputBuffer.Contains('.'))
                    && !(inputBuffer.Replace(".", "").Length >= 16 && input != "."))
            {
                if (inputBuffer == "0" && input != ".")
                    inputBuffer = input;
                else
                    inputBuffer += input;
            }

            return inputBuffer;
        }

        /*
         * Handle transferring inputBuffer to currentExpression.Left and clearing inputBuffer
         */
        public async Task<MathExpression> HandleOperation(string input)
        {
            MathExpression expressionToPrint = new MathExpression();

            if (currentExpression.Left == null)
            {
                Console.WriteLine("In HandleOperation(string input): Invalid currentExpression!");
            }
            else if (currentExpression.Operation == null && currentExpression.Right == null && currentExpression.Result == null)
            {
                inputBuffer = inputBuffer.Length > 0 ? inputBuffer : "0";
                currentExpression.Left = decimal.Parse(inputBuffer);
                currentExpression.Operation = input;
                expressionToPrint.Left = currentExpression.Left;
                expressionToPrint.Operation = currentExpression.Operation;
                expressionToPrint.Result = currentExpression.Left;
            }
            else if (currentExpression.Result == null)
            {
                if (inputBuffer.Length == 0)
                {
                    currentExpression.Operation = input;
                    expressionToPrint.Left = currentExpression.Left;
                    expressionToPrint.Operation = currentExpression.Operation;
                    expressionToPrint.Result = null;
                }
                else
                {
                    string left, right;
                    decimal result;
                    currentExpression.Right = decimal.Parse(inputBuffer);
                    result = CalculatorUtility.EvaluateExpression(currentExpression.Left ?? 0, currentExpression.Operation, currentExpression.Right ?? 0);
                    currentExpression.Result = result;

                    await AddHistory();
                    expressionToPrint.Left = currentExpression.Result;
                    expressionToPrint.Operation = input;
                    expressionToPrint.Result = currentExpression.Result;

                    currentExpression.Result = null;
                    currentExpression.Left = result;
                    currentExpression.Operation = input;
                    currentExpression.Right = null;
                    left = currentExpression.Left?.ToString(STRINGFORMATTER); right = currentExpression.Right?.ToString(STRINGFORMATTER);
                }
            }
            else
            {
                currentExpression.Left = currentExpression.Result;
                currentExpression.Operation = input;
                currentExpression.Right = null;
                currentExpression.Result = null;
                expressionToPrint.Left = currentExpression.Left;
                expressionToPrint.Operation = input;
            }

            inputBuffer = "";
            return expressionToPrint;
        }



        public async Task<MathExpression> HandleEqual()
        {
            //Cleaning inputBuffer is done at the end 

            MathExpression expressionToPrint = new MathExpression();

            if (currentExpression.Left == null)
            {
                Console.WriteLine("Invalid Equal_Handle case!");
                return null;
            }

            if (currentExpression.Operation == null && currentExpression.Right == null)
            {
                currentExpression.Left = decimal.Parse(inputBuffer.Length > 0 ? inputBuffer : "0");
                currentExpression.Result = currentExpression.Left; //experiment
                expressionToPrint.Left = currentExpression.Left;
                expressionToPrint.Result = currentExpression.Left;
            }
            else if (currentExpression.Result == null)
            {
                if (inputBuffer.Length == 0)
                {
                    decimal left = currentExpression.Left ?? 0m;
                    currentExpression.Right = left;
                    currentExpression.Result = CalculatorUtility.EvaluateExpression(left, currentExpression.Operation, left);
                    expressionToPrint.Left = left;
                    expressionToPrint.Operation = currentExpression.Operation;
                    expressionToPrint.Right = left;
                }
                else
                {
                    decimal left = currentExpression.Left ?? 0m, right;
                    currentExpression.Right = decimal.Parse(inputBuffer);
                    right = decimal.Parse(inputBuffer);
                    currentExpression.Result = CalculatorUtility.EvaluateExpression(left, currentExpression.Operation, right);
                    expressionToPrint.Left = left;
                    expressionToPrint.Operation = currentExpression.Operation;
                    expressionToPrint.Right = right;
                }
                expressionToPrint.Result = currentExpression.Result;
            }
            else
            {
                currentExpression.Left = currentExpression.Result;
                currentExpression.Result = CalculatorUtility.EvaluateExpression(currentExpression.Left ?? 0m, currentExpression.Operation, currentExpression.Right ?? 0m);
                expressionToPrint.Left = currentExpression.Left;
                expressionToPrint.Operation = currentExpression.Operation;
                expressionToPrint.Right = currentExpression.Right;
                expressionToPrint.Result = currentExpression.Result;
            }

            await AddHistory();
            inputBuffer = "";
            return expressionToPrint;
        }
        public void HandleClear()
        {
            //clean input buffer
            inputBuffer = "";
            //MathExpression modification
            currentExpression.Left = 0;
            currentExpression.Operation = null;
            currentExpression.Right = null;
            currentExpression.Result = null;
            //resultText modification
        }

        public void HandleDeleteHistory()
        {
            historyList.Clear();
        }

        public void HandleImportHistory()
        {
            CsvConfiguration csvIOConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false };
            historyList.Clear();
            try
            {
                using (var reader = new StreamReader("path\\to\\file.csv"))
                using (var csv = new CsvReader(reader, csvIOConfiguration))
                {
                    var records = csv.GetRecords<MathExpression>();
                    foreach (var record in records)
                    {
                        historyList.Add(record);
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        public void HandleExportHistory()
        {
            CsvConfiguration csvIOConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false };
            historyList.Clear();
            try
            {
                using (var writer = new StreamWriter("path\\to\\file.csv"))
                using (var csv = new CsvWriter(writer, csvIOConfiguration))
                {
                    csv.WriteRecords(historyList);
                }
            }
            catch (Exception ex)
            {
            }
        }


        public async Task HandleDataSourceClear()
        {
            try
            {
                await dataSource.DropAsync();
            }
            catch (Exception ex) { }
        }

        public async Task HandleDataSourceRead()
        {
            historyList.Clear();
            try
            {
                historyList = await dataSource.ReadAsync();
            }
            catch (Exception ex) { }
        }


        public List<MathExpression> GetHistoryList() { return historyList; }

        public MathExpression CopyMathExpression() { return currentExpression.GetCopy(); }
        public void SetCurrentExpression(decimal? left, string operation, decimal? right, decimal? result)
        {
            currentExpression.Left = left;
            currentExpression.Operation = operation;
            currentExpression.Right = right;
            currentExpression.Result = result;
        }
        public void SetInputBuffer(string newValue) { inputBuffer = newValue; }
    }


}