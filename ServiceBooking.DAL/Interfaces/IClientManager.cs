using ServiceBooking.DAL.Entities;
using System;

namespace ServiceBooking.DAL.Interfaces
{
    public interface IClientManager : IDisposable
    {
        void Create(ClientProfile item);
    }
}
