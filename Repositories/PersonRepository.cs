using MVCTask.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using MVCTask.Entities;


namespace MVCTask.Repositories {
    public class PersonRepository: IPersonRepository {

        private readonly string connectionString;

        public PersonRepository(IConfiguration configuration) {
            connectionString = configuration.GetConnectionString("TaskDbContext");
        }

        public async Task<Person> GetPersonById(string id) {
            using var connection = new SqlConnection(connectionString);
            var sql = "SELECT * FROM Persons WHERE UserId = @Id";
            return await connection.QuerySingleOrDefaultAsync<Person>(sql, new { id });
        }

        public async Task<Address> GetAddressByPersonId(int personId) {
            using var connection = new SqlConnection(connectionString);
            var sql = "SELECT * FROM Addresses WHERE AddressId = (SELECT AddressId From Persons where personId = @PersonId)";
            return await connection.QuerySingleOrDefaultAsync<Address>(sql, new { personId });
        }
    }
}