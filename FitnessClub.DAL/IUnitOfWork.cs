using FitnessClub.DAL.Models;

namespace FitnessClub.DAL
{
    /// <summary>
    /// Інтерфейс Unit of Work для координації роботи різних репозиторіїв.
    /// </summary>
    public interface IUnitOfWork
    {
        IRepository<Club> ClubRepository { get; }
        IRepository<ClassSession> ClassSessionRepository { get; }
        IRepository<Member> MemberRepository { get; }
        IRepository<MembershipCard> MembershipCardRepository { get; }
        IRepository<Visit> VisitRepository { get; }
        int Complete(); // Застосування змін (SaveChanges)
    }
}
