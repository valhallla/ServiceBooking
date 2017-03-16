using ServiceBooking.DAL.Entities;
using System;

namespace ServiceBooking.DAL.Interfaces
{
    public interface IClientManager : IDisposable
    {
        void Create(ClientUser item);
        void Delete(ClientUser item);
    }
}
