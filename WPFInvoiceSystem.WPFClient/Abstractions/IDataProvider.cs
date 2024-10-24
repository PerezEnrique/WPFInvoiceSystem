using System.Threading.Tasks;

namespace WPFInvoiceSystem.WPFClient.Abstractions
{
    public interface IDataProvider<ReturnType, InputType>
    {
        Task<ReturnType> Create(InputType inputData);
        Task Delete(int id);
        Task<ReturnType> Get(int id);
        Task<ReturnType> Update(int id, InputType inputData);
    }
}
