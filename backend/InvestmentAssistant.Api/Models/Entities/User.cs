namespace InvestmentAssistant.Api.Models.Entities
{
    /// <summary>
    /// Reprezentuje użytkownika aplikacji.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unikalny identyfikator użytkownika.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Adres e-mail użytkownika.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Imię i nazwisko użytkownika.
        /// </summary>
        public required string FullName { get; set; }

        /// <summary>
        /// Hasło użytkownika (przechowywane w formie zaszyfrowanej).
        /// </summary>
        public required string PasswordHash { get; set; }


        /// <summary>
        /// Czy konto jest aktywne.
        /// </summary>}
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Kiedy konto zostało utworzone.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Kiedy konto było ostatnio aktualizowane.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}