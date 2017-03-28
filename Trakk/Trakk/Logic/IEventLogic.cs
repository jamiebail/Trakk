using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trakk.Models;

namespace Trakk.Logic
{
    interface IEventLogic
    {
        List<Event> GetPrimaryEvents(List<Event> events);
    }
}
