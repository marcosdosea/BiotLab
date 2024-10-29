using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IAnestesicosService
    {

        public void Update(Anestesico anestesico);
        public void Delete(uint id);
        public bool Validar(uint id);
        public Anestesico? Buscar(uint id);
        IEnumerable <Anestesico> GetAll();
        public uint Create (Anestesico anestesico);
    }
}
