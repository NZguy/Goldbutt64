using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    public interface ISubscriber
    {

        /// <summary>
        /// Notifies a subscriber of an event.
        /// </summary>
        /// <example>
        /// Returning True / False
        /// Returning true unsubscribes sub/gameEvent from the Notifier.
        /// Useful if sub only cares about a finite number of event occurrences for a given event. 
        /// Alternatively, one could explicitly call unsubscribe when they
        /// </example>
        /// <param name="gameEvent">The event that just occurred.</param>
        /// <returns>True if notifier should remove event subscription. False otherwise.</returns>
        bool OnNotify(IGameEvent gameEvent);
    }
}
