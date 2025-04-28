using FitnessClub.DAL.Models;

namespace FitnessClub.DAL
{
    /// <summary>
    /// Реалізація патерну Unit of Work.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FitnessClubContext _context;
        private IRepository<Club> _clubRepository;
        private IRepository<ClassSession> _classSessionRepository;
        private IRepository<Member> _memberRepository;
        private IRepository<MembershipCard> _membershipCardRepository;
        private IRepository<Visit> _visitRepository;

        public UnitOfWork(FitnessClubContext context)
        {
            _context = context;
        }
        
        public void EnsureDatabaseCreated()
        {
            _context.Database.EnsureCreated();
        }


        public IRepository<Club> ClubRepository => _clubRepository ?? (_clubRepository = new Repository<Club>(_context));
        public IRepository<ClassSession> ClassSessionRepository => _classSessionRepository ?? (_classSessionRepository = new Repository<ClassSession>(_context));
        public IRepository<Member> MemberRepository => _memberRepository ?? (_memberRepository = new Repository<Member>(_context));
        public IRepository<MembershipCard> MembershipCardRepository => _membershipCardRepository ?? (_membershipCardRepository = new Repository<MembershipCard>(_context));
        public IRepository<Visit> VisitRepository => _visitRepository ?? (_visitRepository = new Repository<Visit>(_context));

      
        
        // Застосування змін до бази
        public int Complete()
        {
            return _context.SaveChanges();
        }

        
    }
}
