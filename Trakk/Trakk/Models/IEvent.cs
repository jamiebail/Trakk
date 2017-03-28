using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Trakk.Models
{
    public interface IEvent
    {
        string Location { get; set; }
        string Title { get; set; }
        string Comments { get; set; }
    }
}
