using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBooking.DAL.Interfaces
{
    public interface IManyToManyResolver
    {
        void Update(int id, int[] selectedItems);
    }
}
