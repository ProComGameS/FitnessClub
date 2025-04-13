using System;
using System.Collections.Generic;

namespace FitnessClub.DAL.Models
{
    /// <summary>
    /// Заняття у клубі: дата/час, місткість та зв’язок з клубом.
    /// </summary>
    public class ClassSession
    {
        public int Id { get; set; }
        public DateTime SessionDateTime { get; set; }
        public int Capacity { get; set; }

        // Зовнішній ключ до Club
        public int ClubId { get; set; }
        public virtual Club Club { get; set; }

        public virtual ICollection<Visit> Visits { get; set; }

        public ClassSession()
        {
            Visits = new HashSet<Visit>();
        }
    }
}
