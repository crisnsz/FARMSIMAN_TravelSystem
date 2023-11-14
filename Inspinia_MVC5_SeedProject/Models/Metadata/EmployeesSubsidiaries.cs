using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_GMEDINA.Models
{
    [MetadataType(typeof(EmployeesSubsidiariesMetaData))]
    public partial class tbEmployeesSubsidiary
    {
        public Nullable<bool> EmployeeSubsidiary_Modified { get; set; }
    }
    public class EmployeesSubsidiariesMetaData
    {
        public int employeeSubsidiary_ID { get; set; }
        public Nullable<int> employee_ID { get; set; }
        public Nullable<int> subsidiary_ID { get; set; }
        public Nullable<decimal> employeeSubsidiary_DistanceKM { get; set; }

    }

    public class Comparar : IEqualityComparer<EmployeesSubsidiariesMetaData>
    {
        public bool Equals(EmployeesSubsidiariesMetaData x, EmployeesSubsidiariesMetaData y)
        {
            if (x == null || y == null)
                return false;

            return x.employeeSubsidiary_ID == y.employeeSubsidiary_ID && x.employeeSubsidiary_DistanceKM == y.employeeSubsidiary_DistanceKM;
        }

        public int GetHashCode(EmployeesSubsidiariesMetaData obj)
        {
            return obj.employeeSubsidiary_ID.GetHashCode();
        }
    }
}