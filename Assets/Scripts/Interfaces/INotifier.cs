using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    public interface INotifier
    {
        /// <summary>
        /// Adds sub to a list of a subscribers to notified when gaemEvent occurs.
        /// </summary>
        /// <param name="gameEvent"></param>
        /// <param name="sub"></param>
        void Subscribe(IGameEvent gameEvent, ISubscriber sub);

        /// <summary>
        /// I don't remember why unsubscribeFromMaster is an optional parameter instead of it's own method. 
        /// Will probably refactor this soon... probably best to just leave it as true.
        /// </summary>
        /// <param name="gameEvent">The GameEvent to subscribe to</param>
        /// <param name="sub">The subscriber that's subscribing to GameEvent events</param>
        /// <param name="unsubscribeFromMaster">Removes all of subs subscriptions of type gameEvent from all Notifiers </param>
        void UnSubscribe(IGameEvent gameEvent, ISubscriber sub, bool unsubscribeFromMaster = true);

        /// <summary>
        /// Notifies all subscribers of gameevent
        /// </summary>
        /// <param name="gameEvent"></param>
        void Notify(IGameEvent gameEvent);
    }
}
