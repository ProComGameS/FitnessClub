using System;
using System.Collections.Generic;

namespace FitnessClub.DAL.Models
{
    /// <summary>
    /// Перелік типів клубних карток.
    /// </summary>
    public enum CardType
    {
        OneTime,
        Subscription,
        NetworkSubscription
    }

    /// <summary>
    /// Клубна картка – містить дані про унікальний номер, тип та зв’язок із членом клубу.
    /// </summary>
    public class MembershipCard
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public CardType CardType { get; set; }

        // Зв’язок 1:1 з Member
        public int MemberId { get; set; }
        public virtual Member Member { get; set; }

       
         

        public virtual ICollection<Visit> Visits { get; set; }

        public MembershipCard()
        {
            Visits = new HashSet<Visit>();
        }
    }

}
