namespace OrderService.Domain.Exceptions;

/// <summary>
/// Thrown when an update operation fails because the document was modified
/// by another process since it was last read (optimistic concurrency conflict).
/// </summary>
public class ConcurrencyException : Exception
{
    /// <summary>
    /// Initializes a new instance of <see cref="ConcurrencyException"/>.
    /// </summary>
    /// <param name="message">Description of the concurrency conflict.</param>
    public ConcurrencyException(string message) : base(message) { }
}
