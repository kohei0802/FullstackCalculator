using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;


namespace ClassLibrary1
{
    public class CsvSource : IDataSource
    {
        //private CsvConfiguration csvIOConfiguration;
        //private string filePath;
        private CsvConfiguration csvIOConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false };
        private string filePath = "path\\to\\new3.csv";

        public CsvSource() { }
        public CsvSource(CsvConfiguration csvIOConfiguration, string filePath)
        {
            this.csvIOConfiguration = csvIOConfiguration;
            this.filePath = filePath;
        }

        public List<MathExpression> Read()
        {
            List<MathExpression> list = new List<MathExpression>();
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, csvIOConfiguration))
            {
                var records = csv.GetRecords<MathExpression>();
                foreach (var record in records)
                {
                    list.Add(record);
                }
            }

            return list;
        }



        public void Insert(MathExpression expression)
        {
            List<MathExpression> records = new List<MathExpression>();
            records.Add(expression);

            using (var stream = File.Open(filePath, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, csvIOConfiguration))
            {
                csv.WriteRecords(records);
            }
        }

        public async Task InsertAsync(MathExpression expression)
        {
            List<MathExpression> records = new List<MathExpression>();
            records.Add(expression);

            using (var stream = File.Open(filePath, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, csvIOConfiguration))
            {
                await csv.WriteRecordsAsync(records);
            }
        }

        public void Drop()
        {
            List<MathExpression> emptyRecords = new List<MathExpression>();
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, csvIOConfiguration))
            {
                csv.WriteRecords(emptyRecords);
            }
        }

        public async Task<List<MathExpression>> ReadAsync()
        {
            List<MathExpression> list = new List<MathExpression>();
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, csvIOConfiguration))
            {
                var records = csv.GetRecordsAsync<MathExpression>();
                // I don't know why 
                await foreach (var record in records) 
                {
                    list.Add(record);
                }
            }

            return list;
        }

        public async Task DropAsync()
        {
            List<MathExpression> emptyRecords = new List<MathExpression>();
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, csvIOConfiguration))
            {
                await csv.WriteRecordsAsync(emptyRecords);
            }
        }
    }
}
