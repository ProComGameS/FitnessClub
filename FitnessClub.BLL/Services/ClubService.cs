
using FitnessClub.DAL;
using FitnessClub.DAL.Models;


namespace FitnessClub.BLL.Services
{
    /// <summary>
    /// Сервіс для роботи з операціями клубу: CRUD, купівля занять/абонементів, реєстрація на заняття.
    /// </summary>
    public class ClubService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClubService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // CRUD операції для Club
        public IEnumerable<Club> GetAllClubs()
        {
            
            return _unitOfWork.ClubRepository.GetAll();
        }

        public Club GetClubById(int id)
        {
            return _unitOfWork.ClubRepository.GetById(id);
        }

        public void CreateClub(Club club)
        {
            _unitOfWork.ClubRepository.Insert(club);
            _unitOfWork.Complete();
        }

        public void UpdateClub(Club club)
        {
            _unitOfWork.ClubRepository.Update(club);
            _unitOfWork.Complete();
        }

        public void DeleteClub(int clubId)
        {
            var club = _unitOfWork.ClubRepository.GetById(clubId);
            if (club != null)
            {
                _unitOfWork.ClubRepository.Delete(club);
                _unitOfWork.Complete();
            }
        }

        /// <summary>
        /// Реєстрація на заняття з перевіркою місткості.
        /// </summary>
        public bool RegisterToSession(int sessionId, int membershipCardId)
        {
            var session = _unitOfWork.ClassSessionRepository.GetById(sessionId);
            var card = _unitOfWork.MembershipCardRepository.GetById(membershipCardId);
            if (session == null || card == null)
                return false;

            // Перевірка місткості заняття
            if (session.Visits.Count >= session.Capacity)
                return false;

            
            Visit newVisit = new Visit
            {
                VisitDate = DateTime.Now,
                ClassSessionId = sessionId,
                MembershipCardId = membershipCardId
            };

            _unitOfWork.VisitRepository.Insert(newVisit);
            _unitOfWork.Complete();
            return true;
        }

     

        /// <summary>
        /// Купівля абонементу – створення або оновлення клубної картки з відповідним типом.
        /// </summary>
        public bool BuySubscription(int clubId, int memberId, CardType cardType)
        {
            var existingMembership = _unitOfWork.MembershipCardRepository
                .GetAll()
                .FirstOrDefault(m => m.MemberId == memberId);

            if (existingMembership != null)
            {
                return false; // Користувач вже має абонемент
            }

            var newMembership = new MembershipCard
            {
                MemberId = memberId,
                CardType = cardType,
            };

            _unitOfWork.MembershipCardRepository.Insert(newMembership);
            _unitOfWork.Complete();
    
            return true;
        }
    }
}
