using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Pages_and_descriptions;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Events
{
    public interface IEvent
    {
        EventDescription GetEventDescription { get; }
        bool Prerequisites(GameWithEvents game);
        Task Appear(GameWithEvents game);
        bool EventHappening { get; }
        //public abstract void Disappear();

    }
}
