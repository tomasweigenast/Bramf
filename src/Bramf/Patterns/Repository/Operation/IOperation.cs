using Bramf.Operation;
using System.Threading.Tasks;

namespace Bramf.Patterns.Repository
{
    /// <summary>
    /// Represents a simple operation that must be executed
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// Executes the task asynchronously
        /// </summary>
        Task<OperationResult> ExecuteAsync();
    }
}