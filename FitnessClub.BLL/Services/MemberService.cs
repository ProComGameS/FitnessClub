using System.Collections.Generic;
using FitnessClub.DAL;
using FitnessClub.DAL.Models;

namespace FitnessClub.BLL.Services
{
    /// <summary>
    /// Сервіс для управління членами клубу (CRUD операції).
    /// </summary>
    public class MemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return _unitOfWork.MemberRepository.GetAll();
        }

        public Member GetMemberById(int id)
        {
            return _unitOfWork.MemberRepository.GetById(id);
        }

        public void CreateMember(Member member)
        {
            _unitOfWork.MemberRepository.Insert(member);
            _unitOfWork.Complete();
        }

        public void UpdateMember(Member member)
        {
            _unitOfWork.MemberRepository.Update(member);
            _unitOfWork.Complete();
        }

        public void DeleteMember(int memberId)
        {
            var member = _unitOfWork.MemberRepository.GetById(memberId);
            if (member != null)
            {
                _unitOfWork.MemberRepository.Delete(member);
                _unitOfWork.Complete();
            }
        }
    }
}
