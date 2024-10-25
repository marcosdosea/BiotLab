using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Core.Service
{
    public interface IAnestesicosService
    {
        void Create(Anestesico anestesico);
        void Update(Anestesico anestesico);
        void Delete(uint id);
        bool IsValid(uint id);
        Anestesico? Get(uint id);
        IEnumerable<Anestesico> GetAll();
    }
}
