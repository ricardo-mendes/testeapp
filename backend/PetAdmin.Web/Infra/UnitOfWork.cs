namespace PetAdmin.Web.Infra
{
    public class UnitOfWork
    {
        private readonly PetContext _context;

        public UnitOfWork(PetContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
