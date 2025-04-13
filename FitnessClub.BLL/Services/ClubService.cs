using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FitnessClub.DAL;
using FitnessClub.DAL.Models;
using Microsoft.VisualBasic;

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
            // Завдяки lazy loading (властивості virtual) можна не використовувати Include тут.
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

            // Додаткові умови (перевірка графіку, локації тощо) можна додати тут.
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
        /// Купівля одноразового заняття через створення або використання існуючої клубної картки (тип OneTime).
        /// </summary>
        public bool BuySingleSession(int clubId, int memberId)
        {
            var member = _unitOfWork.MemberRepository.GetById(memberId);
            if (member == null)
                return false;

            MembershipCard card;
            if (member.MembershipCard != null)
            {
                card = member.MembershipCard;
            }
            else
            {
                card = new MembershipCard
                {
                    CardNumber = Guid.NewGuid().ToString(),
                    CardType = CardType.OneTime,
                    MemberId = memberId
                };
                _unitOfWork.MembershipCardRepository.Insert(card);
                _unitOfWork.Complete();
            }

            // Додаткові бізнес-операції (напр., створення запису Visit) можна реалізувати тут.
            return true;
        }

        /// <summary>
        /// Купівля абонементу – створення або оновлення клубної картки з відповідним типом.
        /// </summary>
        public bool BuySubscription(int clubId, int memberId, CardType cardType)
        {
            var member = _unitOfWork.MemberRepository.GetById(memberId);
            if (member == null)
                return false;

            if (member.MembershipCard != null)
            {
                member.MembershipCard.CardType = cardType;
                _unitOfWork.MembershipCardRepository.Update(member.MembershipCard);
                _unitOfWork.Complete();
            }
            else
            {
                MembershipCard card = new MembershipCard
                {
                    CardNumber = Guid.NewGuid().ToString(),
                    CardType = cardType,
                    MemberId = memberId
                };
                _unitOfWork.MembershipCardRepository.Insert(card);
                _unitOfWork.Complete();
            }

            return true;
        }
    }
}
