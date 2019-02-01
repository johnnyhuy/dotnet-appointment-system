using System;

namespace Rmit.Asr.Application.Providers
{
    public interface IDateTimeProvider
    {
        DateTime Now();
    }
}