using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using RecruitmentMega.Controllers;
using System;
using System.Data;
using System.Threading.Tasks;

namespace RecruitmentMega.Services
{
    public class UserService
    {
        private readonly string _connectionString;

        public UserService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Login method
        public async Task<User> LoginAsync(string username, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT user_id, user_name, is_active FROM ms_user WHERE user_name = @Username AND password = @Password";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Mengembalikan data pengguna jika login berhasil
                            if (reader.GetBoolean(reader.GetOrdinal("is_active")) == false)
                            {
                                return new User
                                {
                                    UserId = reader.GetInt64(reader.GetOrdinal("user_id")),
                                    Username = reader.GetString(reader.GetOrdinal("user_name")),
                                    isActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                                    Status = "Inactive"
                                };
                            }
                            else
                            {
                                return new User
                                {
                                    UserId = reader.GetInt64(reader.GetOrdinal("user_id")),
                                    Username = reader.GetString(reader.GetOrdinal("user_name")),
                                    isActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                                    Status = "Active"
                                };
                            }
                        }
                        return null;  // Jika tidak ada pengguna yang cocok
                    }
                }
            }
        }


        public async Task<bool> AgreementExistsAsync(string agreementNumber)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT COUNT(*) FROM tr_bpkb WHERE agreement_number = @AgreementNumber", connection);
                command.Parameters.AddWithValue("@AgreementNumber", agreementNumber);

                int count = (int)await command.ExecuteScalarAsync();
                return count > 0;
            }
        }

        // Insert agreement data
        public async Task<bool> InsertAgreementDataAsync(AgreementData request)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(@"INSERT INTO tr_bpkb 
                                            (agreement_number, bpkb_no, branch_id, bpkb_date, faktur_no, faktur_date, 
                                             location_id, police_no, bpkb_date_in, created_by, created_on, 
                                             last_updated_by, last_updated_on)
                                            VALUES (@AgreementNumber, @BpkbNo, @BranchId, @BpkbDate, @FakturNo, @FakturDate, 
                                                    @LocationId, @PoliceNo, @BpkbDateIn, @CreatedBy, @CreatedOn, 
                                                    @LastUpdatedBy, @LastUpdatedOn)", connection);

                command.Parameters.AddWithValue("@AgreementNumber", request.AgreementNumber);
                command.Parameters.AddWithValue("@BpkbNo", request.BpkbNo);
                command.Parameters.AddWithValue("@BranchId", request.BranchId);
                command.Parameters.AddWithValue("@BpkbDate", request.BpkbDate);
                command.Parameters.AddWithValue("@FakturNo", request.FakturNo);
                command.Parameters.AddWithValue("@FakturDate", request.FakturDate);
                command.Parameters.AddWithValue("@LocationId", request.LocationId);
                command.Parameters.AddWithValue("@PoliceNo", request.PoliceNo);
                command.Parameters.AddWithValue("@BpkbDateIn", request.BpkbDateIn);
                command.Parameters.AddWithValue("@CreatedBy", request.CreatedBy);
                command.Parameters.AddWithValue("@CreatedOn", DateTime.UtcNow);
                command.Parameters.AddWithValue("@LastUpdatedBy", request.CreatedBy);
                command.Parameters.AddWithValue("@LastUpdatedOn", DateTime.UtcNow);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAgreementDataAsync(AgreementData request)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(@"UPDATE tr_bpkb 
                                          SET bpkb_no = @BpkbNo, branch_id = @BranchId, bpkb_date = @BpkbDate, 
                                              faktur_no = @FakturNo, faktur_date = @FakturDate, location_id = @LocationId, 
                                              police_no = @PoliceNo, bpkb_date_in = @BpkbDateIn, last_updated_by = @LastUpdatedBy, 
                                              last_updated_on = @LastUpdatedOn
                                          WHERE agreement_number = @AgreementNumber", connection);

                command.Parameters.AddWithValue("@AgreementNumber", request.AgreementNumber);
                command.Parameters.AddWithValue("@BpkbNo", request.BpkbNo);
                command.Parameters.AddWithValue("@BranchId", request.BranchId);
                command.Parameters.AddWithValue("@BpkbDate", request.BpkbDate);
                command.Parameters.AddWithValue("@FakturNo", request.FakturNo);
                command.Parameters.AddWithValue("@FakturDate", request.FakturDate);
                command.Parameters.AddWithValue("@LocationId", request.LocationId);
                command.Parameters.AddWithValue("@PoliceNo", request.PoliceNo);
                command.Parameters.AddWithValue("@BpkbDateIn", request.BpkbDateIn);
                command.Parameters.AddWithValue("@LastUpdatedBy", request.CreatedBy);
                command.Parameters.AddWithValue("@LastUpdatedOn", DateTime.UtcNow);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<List<Location>> GetMsLocationAsync()
        {
            var locations = new List<Location>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM ms_storage_location", connection);

                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var location = new Location
                    {
                        LocationId = reader.GetString("location_id"),
                        LocationName = reader.GetString("location_name"),
                    };
                    locations.Add(location);
                }
            }

            return locations;
        }
        public class TrBpkb
        {
            public string AgreementNumber { get; set; }
            public string BpkbNo { get; set; }
            public string BranchId { get; set; }
            public DateTime BpkbDate { get; set; }
            public string FakturNo { get; set; }
            public DateTime FakturDate { get; set; }
            public string LocationId { get; set; }
            public string PoliceNo { get; set; }
            public DateTime BpkbDateIn { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public string LastUpdatedBy { get; set; }
            public DateTime LastUpdatedOn { get; set; }
        }
        public class Location
        {
            public string LocationId { get; set; }
            public string LocationName { get; set; }
        }
        public class User
        {
            public Int64 UserId { get; set; }
            public string Username { get; set; }
            public bool isActive { get; set; }
            public string Status { get; set; }
        }
    }
}
