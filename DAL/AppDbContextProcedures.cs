using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace DAL
{
    public partial class AppDbContext
    {
        private AppDbContextProcedures _procedures;

        public virtual AppDbContextProcedures Procedures 
        { 
            get 
            { 
                if (_procedures is null)
                    _procedures = new AppDbContextProcedures(this);
                return _procedures;
            } 
        }

    }
    public class AppDbContextProcedures
    {
        private readonly AppDbContext _context;
        public AppDbContextProcedures(AppDbContext context)
        {
            _context = context;
        }
        public IList<EmployeeModel> UspGetEmployees()
        {
            IList<EmployeeModel> employeelist = new List<EmployeeModel>();
            using(var command = _context.Database.GetDbConnection().CreateCommand()) 
            {
                command.CommandText = "usp_getemployees";
                command.CommandType = CommandType.StoredProcedure;
                _context.Database.OpenConnection();

                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EmployeeModel employee = new EmployeeModel();
                        employee.EmployeeId = reader.GetInt32("EmployeeId");
                        employee.Name = reader.GetString("Name");
                        employee.Address = reader.GetString("Address");
                        employee.DepartmentId = reader.GetInt32("DepartmentId");
                        employee.DepartmentName = reader.GetString("DepartmentName");
                        employeelist.Add(employee);
                    }
                }
                _context.Database.CloseConnection();
            }
            return employeelist;
        }
        public Employee UspGetEmployee(int EmployeeId)
        {
            Employee employee = new Employee();
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "usp_getemployee";
                command.CommandType= CommandType.StoredProcedure;
                var param = new SqlParameter("EmployeeId", EmployeeId);
                command.Parameters.Add(param);
                _context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employee.EmployeeId = reader.GetInt32("EmployeeId");
                        employee.Name = reader.GetString("Name");
                        employee.Address = reader.GetString("Address");
                        employee.DepartmentId = reader.GetInt32("DepartmentId");
                        break;
                    }
                }
                _context.Database.CloseConnection();
            }
            return employee;
        }
        public bool UspAddEmployee(Employee employee)
        {
            bool status = false;
            using(var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "usp_addemployee";
                command.CommandType = CommandType.StoredProcedure;
                var paramName = new SqlParameter("Name", employee.Name);
                var paramAddress = new SqlParameter("Address", employee.Address);
                var paramDepartmentId = new SqlParameter("DepartmentId", employee.DepartmentId);
                var paramStatus = new SqlParameter()
                {
                    ParameterName = "@Status",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(paramName);
                command.Parameters.Add(paramAddress);
                command.Parameters.Add(paramDepartmentId);
                command.Parameters.Add(paramStatus);
                _context.Database.OpenConnection();
                command.ExecuteNonQuery();
                _context.Database.CloseConnection();
                if((int)paramStatus.Value > 0)
                    status = true;
            }
            return status;
        }
    }
}
