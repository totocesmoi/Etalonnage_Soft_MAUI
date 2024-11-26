using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IMachineService<T>
    {
        public Task<Pagination<T>> GetAsyncAllMachines(int index, int count);
        public Task<T> GetAsyncByReference(string reference);
        public Task<bool> CreateMachine(T machine);
        public Task<T> UpdateMachine(T machine);
        public Task<bool> DeleteMachine(string reference);

    }
}
