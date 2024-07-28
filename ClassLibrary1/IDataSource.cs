using System.Net;
using System.Net.Http.Json;
using System;


namespace ClassLibrary1
{
    public interface IDataSource
    {

        //User has his own responsibility to handle exceptions from IDataSource
        //List<MathExpression> Read();

        Task<List<MathExpression>> ReadAsync();
        //void Insert(MathExpression expression);
        Task InsertAsync(MathExpression expression);

        //void Drop();

        Task DropAsync();
    }
}
