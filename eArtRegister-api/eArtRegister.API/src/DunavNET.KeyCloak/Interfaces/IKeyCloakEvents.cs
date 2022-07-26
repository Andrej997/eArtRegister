using KeyCloak.Models;
using System.Collections.Generic;

namespace KeyCloak.Interfaces
{
    public interface IKeyCloakEvents
    {
        IEnumerable<Event> GetEvents(EventFilter filter);
    }
}
