using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RecruitmentMega.Services;
using System.Threading.Tasks;

namespace RecruitmentMega.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowLocalhost")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.LoginAsync(request.Username, request.Password);
            if (user != null)
            {
                return Ok(new { message = "Login successful", user = user });
            }
            return Unauthorized(new { message = "Invalid credentials" });
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertOrUpdateAgreementData([FromBody] AgreementData request)
        {
            try
            {
                // Check if the AgreementNumber already exists
                bool exists = await _userService.AgreementExistsAsync(request.AgreementNumber);

                if (exists)
                {
                    // Update existing agreement
                    bool updateResult = await _userService.UpdateAgreementDataAsync(request);
                    if (updateResult)
                    {
                        return Ok(new { message = "Data updated successfully" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Failed to update data" });
                    }
                }
                else
                {
                    bool insertResult = await _userService.InsertAgreementDataAsync(request);
                    if (insertResult)
                    {
                        return Ok(new { message = "Data inserted successfully" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Failed to insert data" });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost("getlocation")]
        public async Task<IActionResult> GetLocations()
        {
            var data = await _userService.GetMsLocationAsync();
            if (data == null || data.Count == 0)
            {
                return NotFound("No data found");
            }
            return Ok(data);
        }

        [HttpPost("getdata")]
        public async Task<IActionResult> GetData()
        {
            var data = await _userService.GetTrBpkbAsync();
            if (data == null || data.Count == 0)
            {
                return NotFound("No data found");
            }
            return Ok(data);
        }

        [HttpPost("deletedata")]
        public async Task<IActionResult> DeleteData([FromBody] DeleteRequest request)
        {
            var isDeleted = await _userService.DeleteData(request.AgreementNumber);

            if (!isDeleted)
            {
                return NotFound("No data found or failed to delete.");
            }

            return Ok(new { message = "Data deleted successfully" });
        }

    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AgreementData
    {
        public string AgreementNumber { get; set; }
        public string BpkbNo { get; set; }
        public int BranchId { get; set; }
        public DateTime BpkbDate { get; set; }
        public string FakturNo { get; set; }
        public DateTime FakturDate { get; set; }
        public int LocationId { get; set; }
        public string PoliceNo { get; set; }
        public DateTime BpkbDateIn { get; set; }
        public string CreatedBy { get; set; }
    }
    public class DeleteRequest
    {
        public string AgreementNumber { get; set; }
    }
}
