using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        [Required(ErrorMessage ="Please Eneter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Please Enter Address")]
        public string Address { get; set; }
        [Required(ErrorMessage ="Please Select Deparment")]
        [Display(Name= "Department")]
        public int DepartmentId { get; set; }
        [Display(Name = "Department")]
        public string? DepartmentName { get; set; }
    }
}
