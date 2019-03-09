using System;

namespace ConsoleDraw.Core.Commands.Operations
{

    public interface IApplyable
    {
        event EventHandler<EventArgs> Deactivated;
        bool Apply();
        bool Deactivate();
    }
}