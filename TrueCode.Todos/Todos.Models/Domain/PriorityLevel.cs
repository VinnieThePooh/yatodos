namespace Todos.Models.Domain;

public enum PriorityLevel
{
    /// <summary>
    /// Несрочный
    /// </summary>
    Relaxed,
    /// <summary>
    /// Умеренно срочный
    /// </summary>
    Average,
    /// <summary>
    /// Срочные
    /// </summary>
    Urgent,
    /// <summary>
    ///Супер срочный, безотлагательный
    /// </summary>
    SuperUrgent
}