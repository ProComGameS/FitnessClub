using System.Collections.Generic;

namespace FitnessClub.DAL.Models
{
    /// <summary>
    /// Фітнес клуб – містить дані про назву, локацію та список занять.
    /// </summary>
    public class Club
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        // Використання virtual для lazy loading пов’язаних сесій
        public virtual ICollection<ClassSession> Sessions { get; set; }

        public Club()
        {
            Sessions = new HashSet<ClassSession>();
        }
    }
}
