using ClassLibrary1;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatorDataStorageController : ControllerBase
    {
        private readonly IDataSource _dataSource;

        public CalculatorDataStorageController(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        [HttpPost("insert")]
        public async Task<JsonResult> InsertExpression(MathExpression expression)
        {
            
            try
            {
                await _dataSource.InsertAsync(expression);
                return new JsonResult(Ok(expression));
            }
            catch (Exception ex)
            {
                //throw new Exception("Invalid Insertion");
                return new JsonResult(StatusCode(500));
                //cannot replace status code 200
            }

        }

        [HttpGet("all/read")]
        public async Task<List<MathExpression>> Get()
        {
            try
            {
                List<MathExpression> list = await _dataSource.ReadAsync();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        

        [HttpGet("all/delete")]
        public async Task<JsonResult> DeleteAllExpressions()
        {
            try
            {
                await _dataSource.DropAsync();
                return new JsonResult(Ok());
            }
            catch(Exception ex)
            {
                return new JsonResult(StatusCode(500));
            }
            
            
        }
    }
}
