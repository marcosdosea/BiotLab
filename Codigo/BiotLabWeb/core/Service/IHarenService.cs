namespace core.Service
{
    public interface IHarenService
    {
        public Haren Get(int  idHaren);
        public int Create(Haren haren);
        public int Edit(Haren haren);
        public int Delete(int idHaren);

        IEnumerable<Haren> GetAll();
        IEnumerable<Haren> GetByIdHaren(int id);

    }
}
