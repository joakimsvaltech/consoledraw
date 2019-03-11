using ConsoleDraw.Core.Interaction;
using System;

namespace ConsoleDraw.Interaction.Operations
{
    public interface IApplyable<TShape> : IExecutable, IApplyable
    {
    }

    public interface IApplyable
    {
        event EventHandler<EventArgs> Deactivated;
        bool Apply();
        bool Reapply();
        bool Unapply();
        bool Deactivate();
    }
}