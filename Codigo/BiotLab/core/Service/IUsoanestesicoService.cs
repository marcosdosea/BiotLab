namespace Core.Service
{
    public interface IUsoanestesicoService
    {
        uint Create(Usoanestesico usoanestesico);
        void Update(Usoanestesico usoanestesico);
        void Delete(uint id);
        IEnumerable<Usoanestesico> GetAll();
        Usoanestesico? Get(uint id);
    }
}