using System.Data.SqlClient;
using System.Data;
using System.Xml.XPath;

namespace ClassLibrary1
{
    public class SqlSource : IDataSource
    {
        private string SqlDataSource = "192.168.4.70";
        private string SqlInitialCatalog = "kohei_db";
        private string SqlUserId = "kohei";
        private string SqlPassword = "kohei";
        private string SqlTable = "NewCalculatorResults";
        private string connectionString;
        public SqlSource() { connectionString = $"Data Source={SqlDataSource}; Initial Catalog={SqlInitialCatalog}; User id={SqlUserId};Password={SqlPassword};"; }

        public List<MathExpression> Read()
        {
            List<MathExpression> list = new List<MathExpression>();
            string queryString = $"SELECT * FROM {SqlTable} ;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    IDataRecord dataRecord = (IDataReader)reader;
                    MathExpression someExpression = new MathExpression();
                    someExpression.Left = dataRecord.GetDecimal(1);
                    someExpression.Operation = dataRecord.IsDBNull(2) ? null : dataRecord[2].ToString();
                    someExpression.Right = dataRecord.IsDBNull(3) ? null : dataRecord.GetDecimal(3);
                    someExpression.Result = dataRecord.IsDBNull(4) ? null : dataRecord.GetDecimal(4);
                    list.Add(someExpression);
                }
            }


            return list;
        }



        public void Insert(MathExpression mathExpression)
        {
            string leftString = mathExpression.Left == null ? "null" : mathExpression.Left?.ToString();
            string rightString = mathExpression.Right == null ? "null" : mathExpression.Right?.ToString();
            string operation = mathExpression.Operation == null ? "null" : $"'{mathExpression.Operation}'";
            string resultString = mathExpression.Result == null ? "null" : mathExpression.Result?.ToString();
            string dateTimeAdded = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string queryString = $"insert into {SqlTable} ( LeftOperand, Operator, RightOperand, Result, DateTimeAdded) " +
                $"values ({leftString},{operation},{rightString},{resultString}, '{dateTimeAdded}');";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                connection.Open();
                cmd.ExecuteNonQuery();
            }

        }



        public void Drop()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand($"truncate table {SqlTable}", connection);
                connection.Open();
                int numberOfRowsAffected = cmd.ExecuteNonQuery();

            }
        }

        public async Task<List<MathExpression>> ReadAsync()
        {
            List<MathExpression> list = new List<MathExpression>();
            string queryString = $"SELECT * FROM {SqlTable} ;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                await connection.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    IDataRecord dataRecord = (IDataReader)reader;
                    MathExpression someExpression = new MathExpression();
                    someExpression.Left = dataRecord.GetDecimal(1);
                    someExpression.Operation = dataRecord.IsDBNull(2) ? null : dataRecord[2].ToString();
                    someExpression.Right = dataRecord.IsDBNull(3) ? null : dataRecord.GetDecimal(3);
                    someExpression.Result = dataRecord.IsDBNull(4) ? null : dataRecord.GetDecimal(4);
                    list.Add(someExpression);
                }
            }


            return list;
        }

        public async Task InsertAsync(MathExpression mathExpression)
        {
            string leftString = mathExpression.Left == null ? "null" : mathExpression.Left?.ToString();
            string rightString = mathExpression.Right == null ? "null" : mathExpression.Right?.ToString();
            string operation = mathExpression.Operation == null ? "null" : $"'{mathExpression.Operation}'";
            string resultString = mathExpression.Result == null ? "null" : mathExpression.Result?.ToString();
            string dateTimeAdded = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string queryString = $"insert into {SqlTable} ( LeftOperand, Operator, RightOperand, Result, DateTimeAdded) " +
                $"values ({leftString},{operation},{rightString},{resultString}, '{dateTimeAdded}');";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                await connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task DropAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand($"truncate table {SqlTable}", connection);
                await connection.OpenAsync();
                int numberOfRowsAffected = await cmd.ExecuteNonQueryAsync();

            }
        }
    }
}
