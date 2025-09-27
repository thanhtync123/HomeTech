using System;

namespace HomeTechBlazor.Model
{
    public class Orders
    {
        public int Id { get; set; }               
        public int CustomerId { get; set; }         
        public int ServiceId { get; set; }          
        public int? TechnicianId { get; set; }      
        public string ScheduleTime { get; set; }   
        public string Status { get; set; }           
        public int? TotalPrice { get; set; }      
        public string CreatedAt { get; set; }      
        public string UpdatedAt { get; set; }

        public string CustomerName { get;set; }
        public string ServiceName { get; set; }
        public string TechnicianName { get;set;}
    }
}
