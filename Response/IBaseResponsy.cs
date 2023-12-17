using Analiza_Risc.Models.Enum;

namespace Analiza_Risc.Response
{
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public string Description { get; set; }        

        public StatusCodeUser StatusCode { get; set; }
        
        public T Data { get; set; }
    }

    public interface IBaseResponse<T>
    {
        string Description { get; }
        StatusCodeUser StatusCode { get; }
        T Data { get; }
    } 
}