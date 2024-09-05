using System.Linq.Expressions;

namespace Patient.Data.Interfaces
{
    public interface IByExpressionSearcher<T>
    {
        Task<IEnumerable<T>> GetAllByDateExpressions(IEnumerable<Expression<Func<Models.Patient, bool>>> expressions);
    }
}
