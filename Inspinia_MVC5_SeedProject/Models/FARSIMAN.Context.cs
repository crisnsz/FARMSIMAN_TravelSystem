﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERP_GMEDINA.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class FARSIMANEntities : DbContext
    {
        public FARSIMANEntities()
            : base("name=FARSIMANEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbEmployee> tbEmployees { get; set; }
        public virtual DbSet<tbEmployeesSubsidiary> tbEmployeesSubsidiaries { get; set; }
        public virtual DbSet<tbPosition> tbPositions { get; set; }
        public virtual DbSet<tbSubsidiary> tbSubsidiaries { get; set; }
        public virtual DbSet<tbTransporter> tbTransporters { get; set; }
        public virtual DbSet<tbTravelDetail> tbTravelDetails { get; set; }
        public virtual DbSet<tbTravel> tbTravels { get; set; }
        public virtual DbSet<tbAccess> tbAccesses { get; set; }
        public virtual DbSet<tbObject> tbObjects { get; set; }
        public virtual DbSet<tbUser> tbUsers { get; set; }
    
        public virtual ObjectResult<UDP_Gral_tbEmployees_Update_Result> UDP_Gral_tbEmployees_Update(Nullable<int> employee_ID, string employee_Name, string employee_Direction, Nullable<int> position_ID)
        {
            var employee_IDParameter = employee_ID.HasValue ?
                new ObjectParameter("employee_ID", employee_ID) :
                new ObjectParameter("employee_ID", typeof(int));
    
            var employee_NameParameter = employee_Name != null ?
                new ObjectParameter("employee_Name", employee_Name) :
                new ObjectParameter("employee_Name", typeof(string));
    
            var employee_DirectionParameter = employee_Direction != null ?
                new ObjectParameter("employee_Direction", employee_Direction) :
                new ObjectParameter("employee_Direction", typeof(string));
    
            var position_IDParameter = position_ID.HasValue ?
                new ObjectParameter("position_ID", position_ID) :
                new ObjectParameter("position_ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<UDP_Gral_tbEmployees_Update_Result>("UDP_Gral_tbEmployees_Update", employee_IDParameter, employee_NameParameter, employee_DirectionParameter, position_IDParameter);
        }
    
        public virtual ObjectResult<UDP_Gral_tbTravelDetail_Insert_Result> UDP_Gral_tbTravelDetail_Insert(Nullable<int> travel_ID, Nullable<int> employee_ID, Nullable<decimal> distance_Kilometers, Nullable<decimal> travel_Cost)
        {
            var travel_IDParameter = travel_ID.HasValue ?
                new ObjectParameter("travel_ID", travel_ID) :
                new ObjectParameter("travel_ID", typeof(int));
    
            var employee_IDParameter = employee_ID.HasValue ?
                new ObjectParameter("employee_ID", employee_ID) :
                new ObjectParameter("employee_ID", typeof(int));
    
            var distance_KilometersParameter = distance_Kilometers.HasValue ?
                new ObjectParameter("distance_Kilometers", distance_Kilometers) :
                new ObjectParameter("distance_Kilometers", typeof(decimal));
    
            var travel_CostParameter = travel_Cost.HasValue ?
                new ObjectParameter("travel_Cost", travel_Cost) :
                new ObjectParameter("travel_Cost", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<UDP_Gral_tbTravelDetail_Insert_Result>("UDP_Gral_tbTravelDetail_Insert", travel_IDParameter, employee_IDParameter, distance_KilometersParameter, travel_CostParameter);
        }
    
        public virtual ObjectResult<UDP_Gral_tbTravels_Insert_Result> UDP_Gral_tbTravels_Insert(Nullable<int> subsidiary_ID, Nullable<int> transporter_ID, Nullable<int> employee_ID, Nullable<System.DateTime> departure_Date_and_Time, Nullable<decimal> distance_Kilometers, Nullable<decimal> total_travel_Cost)
        {
            var subsidiary_IDParameter = subsidiary_ID.HasValue ?
                new ObjectParameter("subsidiary_ID", subsidiary_ID) :
                new ObjectParameter("subsidiary_ID", typeof(int));
    
            var transporter_IDParameter = transporter_ID.HasValue ?
                new ObjectParameter("transporter_ID", transporter_ID) :
                new ObjectParameter("transporter_ID", typeof(int));
    
            var employee_IDParameter = employee_ID.HasValue ?
                new ObjectParameter("employee_ID", employee_ID) :
                new ObjectParameter("employee_ID", typeof(int));
    
            var departure_Date_and_TimeParameter = departure_Date_and_Time.HasValue ?
                new ObjectParameter("departure_Date_and_Time", departure_Date_and_Time) :
                new ObjectParameter("departure_Date_and_Time", typeof(System.DateTime));
    
            var distance_KilometersParameter = distance_Kilometers.HasValue ?
                new ObjectParameter("distance_Kilometers", distance_Kilometers) :
                new ObjectParameter("distance_Kilometers", typeof(decimal));
    
            var total_travel_CostParameter = total_travel_Cost.HasValue ?
                new ObjectParameter("total_travel_Cost", total_travel_Cost) :
                new ObjectParameter("total_travel_Cost", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<UDP_Gral_tbTravels_Insert_Result>("UDP_Gral_tbTravels_Insert", subsidiary_IDParameter, transporter_IDParameter, employee_IDParameter, departure_Date_and_TimeParameter, distance_KilometersParameter, total_travel_CostParameter);
        }
    
        public virtual ObjectResult<UDP_Sec_Login_Result> UDP_Sec_Login(string user_Username, string user_password)
        {
            var user_UsernameParameter = user_Username != null ?
                new ObjectParameter("user_Username", user_Username) :
                new ObjectParameter("user_Username", typeof(string));
    
            var user_passwordParameter = user_password != null ?
                new ObjectParameter("user_password", user_password) :
                new ObjectParameter("user_password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<UDP_Sec_Login_Result>("UDP_Sec_Login", user_UsernameParameter, user_passwordParameter);
        }
    
        public virtual ObjectResult<UDP_Gral_tbEmployees_Insert_Result> UDP_Gral_tbEmployees_Insert(string employee_Name, string employee_Direction, Nullable<int> position_ID)
        {
            var employee_NameParameter = employee_Name != null ?
                new ObjectParameter("employee_Name", employee_Name) :
                new ObjectParameter("employee_Name", typeof(string));
    
            var employee_DirectionParameter = employee_Direction != null ?
                new ObjectParameter("employee_Direction", employee_Direction) :
                new ObjectParameter("employee_Direction", typeof(string));
    
            var position_IDParameter = position_ID.HasValue ?
                new ObjectParameter("position_ID", position_ID) :
                new ObjectParameter("position_ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<UDP_Gral_tbEmployees_Insert_Result>("UDP_Gral_tbEmployees_Insert", employee_NameParameter, employee_DirectionParameter, position_IDParameter);
        }
    
        public virtual ObjectResult<UDP_Gral_tbTravel_With_tbTravelDetail_Delete_Result> UDP_Gral_tbTravel_With_tbTravelDetail_Delete(Nullable<int> travel_ID)
        {
            var travel_IDParameter = travel_ID.HasValue ?
                new ObjectParameter("travel_ID", travel_ID) :
                new ObjectParameter("travel_ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<UDP_Gral_tbTravel_With_tbTravelDetail_Delete_Result>("UDP_Gral_tbTravel_With_tbTravelDetail_Delete", travel_IDParameter);
        }
    
        public virtual ObjectResult<UDP_Gral_tbTravelDetail_Delete_Result> UDP_Gral_tbTravelDetail_Delete(Nullable<int> travel_Detail_ID)
        {
            var travel_Detail_IDParameter = travel_Detail_ID.HasValue ?
                new ObjectParameter("travel_Detail_ID", travel_Detail_ID) :
                new ObjectParameter("travel_Detail_ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<UDP_Gral_tbTravelDetail_Delete_Result>("UDP_Gral_tbTravelDetail_Delete", travel_Detail_IDParameter);
        }
    
        public virtual ObjectResult<UDP_Gral_tbTravelDetail_Update_Result> UDP_Gral_tbTravelDetail_Update(Nullable<int> travel_Detail_ID, Nullable<int> travel_ID, Nullable<int> employee_ID, Nullable<decimal> distance_Kilometers, Nullable<decimal> travel_Cost)
        {
            var travel_Detail_IDParameter = travel_Detail_ID.HasValue ?
                new ObjectParameter("travel_Detail_ID", travel_Detail_ID) :
                new ObjectParameter("travel_Detail_ID", typeof(int));
    
            var travel_IDParameter = travel_ID.HasValue ?
                new ObjectParameter("travel_ID", travel_ID) :
                new ObjectParameter("travel_ID", typeof(int));
    
            var employee_IDParameter = employee_ID.HasValue ?
                new ObjectParameter("employee_ID", employee_ID) :
                new ObjectParameter("employee_ID", typeof(int));
    
            var distance_KilometersParameter = distance_Kilometers.HasValue ?
                new ObjectParameter("distance_Kilometers", distance_Kilometers) :
                new ObjectParameter("distance_Kilometers", typeof(decimal));
    
            var travel_CostParameter = travel_Cost.HasValue ?
                new ObjectParameter("travel_Cost", travel_Cost) :
                new ObjectParameter("travel_Cost", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<UDP_Gral_tbTravelDetail_Update_Result>("UDP_Gral_tbTravelDetail_Update", travel_Detail_IDParameter, travel_IDParameter, employee_IDParameter, distance_KilometersParameter, travel_CostParameter);
        }
    
        public virtual ObjectResult<UDP_Gral_tbTravels_Update_Result> UDP_Gral_tbTravels_Update(Nullable<int> travel_ID, Nullable<int> subsidiary_ID, Nullable<int> transporter_ID, Nullable<int> employee_ID, Nullable<System.DateTime> departure_Date_and_Time, Nullable<decimal> distance_Kilometers, Nullable<decimal> total_travel_Cost)
        {
            var travel_IDParameter = travel_ID.HasValue ?
                new ObjectParameter("travel_ID", travel_ID) :
                new ObjectParameter("travel_ID", typeof(int));
    
            var subsidiary_IDParameter = subsidiary_ID.HasValue ?
                new ObjectParameter("subsidiary_ID", subsidiary_ID) :
                new ObjectParameter("subsidiary_ID", typeof(int));
    
            var transporter_IDParameter = transporter_ID.HasValue ?
                new ObjectParameter("transporter_ID", transporter_ID) :
                new ObjectParameter("transporter_ID", typeof(int));
    
            var employee_IDParameter = employee_ID.HasValue ?
                new ObjectParameter("employee_ID", employee_ID) :
                new ObjectParameter("employee_ID", typeof(int));
    
            var departure_Date_and_TimeParameter = departure_Date_and_Time.HasValue ?
                new ObjectParameter("departure_Date_and_Time", departure_Date_and_Time) :
                new ObjectParameter("departure_Date_and_Time", typeof(System.DateTime));
    
            var distance_KilometersParameter = distance_Kilometers.HasValue ?
                new ObjectParameter("distance_Kilometers", distance_Kilometers) :
                new ObjectParameter("distance_Kilometers", typeof(decimal));
    
            var total_travel_CostParameter = total_travel_Cost.HasValue ?
                new ObjectParameter("total_travel_Cost", total_travel_Cost) :
                new ObjectParameter("total_travel_Cost", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<UDP_Gral_tbTravels_Update_Result>("UDP_Gral_tbTravels_Update", travel_IDParameter, subsidiary_IDParameter, transporter_IDParameter, employee_IDParameter, departure_Date_and_TimeParameter, distance_KilometersParameter, total_travel_CostParameter);
        }
    
        public virtual ObjectResult<UDP_Gral_tbEmployeesSubsidiaries_Delete_Result> UDP_Gral_tbEmployeesSubsidiaries_Delete(Nullable<int> employeeSubsidiary_ID)
        {
            var employeeSubsidiary_IDParameter = employeeSubsidiary_ID.HasValue ?
                new ObjectParameter("employeeSubsidiary_ID", employeeSubsidiary_ID) :
                new ObjectParameter("employeeSubsidiary_ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<UDP_Gral_tbEmployeesSubsidiaries_Delete_Result>("UDP_Gral_tbEmployeesSubsidiaries_Delete", employeeSubsidiary_IDParameter);
        }
    
        public virtual ObjectResult<UDP_Gral_tbEmployeesSubsidiaries_Insert_Result> UDP_Gral_tbEmployeesSubsidiaries_Insert(Nullable<int> employee_ID, Nullable<int> subsidiary_ID, Nullable<decimal> employeeSubsidiary_DistanceKM)
        {
            var employee_IDParameter = employee_ID.HasValue ?
                new ObjectParameter("employee_ID", employee_ID) :
                new ObjectParameter("employee_ID", typeof(int));
    
            var subsidiary_IDParameter = subsidiary_ID.HasValue ?
                new ObjectParameter("subsidiary_ID", subsidiary_ID) :
                new ObjectParameter("subsidiary_ID", typeof(int));
    
            var employeeSubsidiary_DistanceKMParameter = employeeSubsidiary_DistanceKM.HasValue ?
                new ObjectParameter("employeeSubsidiary_DistanceKM", employeeSubsidiary_DistanceKM) :
                new ObjectParameter("employeeSubsidiary_DistanceKM", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<UDP_Gral_tbEmployeesSubsidiaries_Insert_Result>("UDP_Gral_tbEmployeesSubsidiaries_Insert", employee_IDParameter, subsidiary_IDParameter, employeeSubsidiary_DistanceKMParameter);
        }
    
        public virtual ObjectResult<UDP_Gral_tbEmployeesSubsidiaries_Update_Result> UDP_Gral_tbEmployeesSubsidiaries_Update(Nullable<int> employeeSubsidiary_ID, Nullable<int> employee_ID, Nullable<int> subsidiary_ID, Nullable<decimal> employeeSubsidiary_DistanceKM)
        {
            var employeeSubsidiary_IDParameter = employeeSubsidiary_ID.HasValue ?
                new ObjectParameter("employeeSubsidiary_ID", employeeSubsidiary_ID) :
                new ObjectParameter("employeeSubsidiary_ID", typeof(int));
    
            var employee_IDParameter = employee_ID.HasValue ?
                new ObjectParameter("employee_ID", employee_ID) :
                new ObjectParameter("employee_ID", typeof(int));
    
            var subsidiary_IDParameter = subsidiary_ID.HasValue ?
                new ObjectParameter("subsidiary_ID", subsidiary_ID) :
                new ObjectParameter("subsidiary_ID", typeof(int));
    
            var employeeSubsidiary_DistanceKMParameter = employeeSubsidiary_DistanceKM.HasValue ?
                new ObjectParameter("employeeSubsidiary_DistanceKM", employeeSubsidiary_DistanceKM) :
                new ObjectParameter("employeeSubsidiary_DistanceKM", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<UDP_Gral_tbEmployeesSubsidiaries_Update_Result>("UDP_Gral_tbEmployeesSubsidiaries_Update", employeeSubsidiary_IDParameter, employee_IDParameter, subsidiary_IDParameter, employeeSubsidiary_DistanceKMParameter);
        }
    
        public virtual ObjectResult<UDP_Gral_tbEmployees_Delete_Result> UDP_Gral_tbEmployees_Delete(Nullable<int> employee_ID)
        {
            var employee_IDParameter = employee_ID.HasValue ?
                new ObjectParameter("employee_ID", employee_ID) :
                new ObjectParameter("employee_ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<UDP_Gral_tbEmployees_Delete_Result>("UDP_Gral_tbEmployees_Delete", employee_IDParameter);
        }
    }
}
