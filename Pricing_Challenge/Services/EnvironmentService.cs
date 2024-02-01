using System;
using Pricing_Challenge.Interfaces;

namespace Pricing_Challenge.Services
{
    public class EnvironmentService : IEnvironment
    {
        #region IEnvironment Implementation

        public void Exit(int value)
        {
            Environment.Exit(value);
        }

        #endregion
    }
}