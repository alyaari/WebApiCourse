using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
 
        // GET: api/Employees
        [HttpGet]
        public ActionResult<List<Employee>> GetEmployee()
        {
            return EmployeeService.GetAll();
        }

        [HttpGet]
        [Route("Search")]
        public ActionResult Search(string name) {

       return Ok("found");
        }

        // GET: api/Employees/{Id}
        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployee(int id)
        {
            var employee = EmployeeService.Get(id);

            if(employee==null)
                return StatusCode(400, new { message = "خطاء البيانات غير صحيحة" });


            return Ok(employee);

        }
        [HttpPost]
        public ActionResult<Employee> PostEmployee(Employee employee)
        {
            if (employee == null)
       
                return StatusCode(400, new {message="خطاء البيانات غير صحيحة"});
             
            EmployeeService.Add(employee);

            return Ok(employee);

        }

        [HttpPut("{id}")]
        public ActionResult<Employee> PutEmployee(int id,Employee employee)
        {
            if (employee == null)

                return StatusCode(400, new { message = "خطاء البيانات غير صحيحة" });


         var oldEmployee= EmployeeService.Get(id);
            if (oldEmployee==null)
                     return StatusCode(404, new { message = "الموظف غير موجود" });

          EmployeeService.Update(employee);


            return Ok(employee);

        }
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var employee =EmployeeService.Get(id);
            if (employee == null)
            {
                return NotFound();
            }

            EmployeeService.Delete(id);

            return NoContent();
        }

    }
}
