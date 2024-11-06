

using MVCTask.Entities;

namespace MVCTask.Interfaces {
    public interface IPersonRepository {
        Task<Address> GetAddressByPersonId(int personId);
        Task<Person> GetPersonById(string id);
    }
}
