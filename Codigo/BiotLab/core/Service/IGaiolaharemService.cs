using Core;
using System.Collections.Generic;

namespace Core.Service
{
    public interface IGaiolaharemService
    {
        void Create(Gaiolaharem gaiolaharem);
        void Update(Gaiolaharem gaiolaharem);
        void Delete(uint idGaiola, uint idHarem);
        IEnumerable<Gaiolaharem> GetAll();
        Gaiolaharem Get(uint idGaiola, uint idHarem);
    }
}
