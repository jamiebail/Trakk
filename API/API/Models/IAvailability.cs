using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Helpers;

namespace API.Models
{
    interface IAvailability
    {
        [Key]
         int Id { get; set; }
         TrakkEnums.UserAvailability Availability { get; set; }
         int UserId { get; set; }
         int EventId { get; set; }
    }
}
