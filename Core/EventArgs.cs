using System;

namespace ConsoleDraw.Core
{
    public class EventArgs<TModel> : EventArgs
    {
        public EventArgs(TModel model) => Model = model;
        public TModel Model { get; }

        public static implicit operator EventArgs<TModel>(TModel model) => new EventArgs<TModel>(model);
        public static implicit operator TModel(EventArgs<TModel> e) => e.Model;
    }
}