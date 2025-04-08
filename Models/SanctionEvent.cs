using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicGames.Web.Models
{
    public enum EventType
    {
        Created = 1,
        Updated = 2,
        Deleted = 3
    }

    public class SanctionEvent
    {
        public Sanction Sanction { get; }
        public EventType Type { get; }

        public SanctionEvent(Sanction sanction, EventType type)
        {
            Sanction = sanction;
            Type = type;
        }
    }
}
