using Patient.Data.Models;

namespace Patient.Data.Interfaces
{
    public interface IByDateSearcher<T>
    {
        Task<IEnumerable<T>> GetAllByDates(IEnumerable<FHIRDate> fhirDates);
    }
}
