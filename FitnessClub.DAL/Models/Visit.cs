using System;

namespace FitnessClub.DAL.Models
{
    /// <summary>
    /// Відвідування заняття – містить дату відвідування та зв’язки з карткою і сесією.
    /// </summary>
    public class Visit
    {
        public int Id { get; set; }
        public DateTime VisitDate { get; set; }

        // Зовнішній ключ до MembershipCard
        public int MembershipCardId { get; set; }
        public virtual MembershipCard MembershipCard { get; set; }

        // Зовнішній ключ до ClassSession
        public int ClassSessionId { get; set; }
        public virtual ClassSession ClassSession { get; set; }
    }
}
