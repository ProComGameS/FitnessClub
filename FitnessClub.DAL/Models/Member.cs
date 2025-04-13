namespace FitnessClub.DAL.Models
{
    /// <summary>
    /// Член клубу – містить основні дані про користувача та зв’язану клубну картку.
    /// </summary>
    public class Member
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        // Зв’язок 1:1, virtual для lazy loading
        public virtual MembershipCard MembershipCard { get; set; }
    }
}
