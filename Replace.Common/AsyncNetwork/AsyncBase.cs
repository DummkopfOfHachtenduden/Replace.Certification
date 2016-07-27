using System;
using System.Collections.Generic;

namespace Replace.Common.AsyncNetwork
{
    public abstract class AsyncBase
    {
        private List<AsyncState> states;

        public AsyncBase()
        {
            states = new List<AsyncState>();
        }

        public void Tick()
        {
            lock (states)
            {
                foreach (AsyncState state in states)
                {
                    try
                    {
                        state.Context.Interface.OnTick(state.Context);
                    }
                    catch (Exception) { }
                }
            }
        }

        public void AddState(AsyncState state)
        {
            lock (states)
            {
                states.Add(state);
            }
        }

        public void RemoveState(AsyncState state)
        {
            lock (states)
            {
                states.Remove(state);
            }
        }
    }
}