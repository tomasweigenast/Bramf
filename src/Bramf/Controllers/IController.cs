using Bramf.Patterns.MVVM;

namespace Bramf.Controllers
{
    /// <summary>
    /// Defines a base controller
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Loads the controller
        /// </summary>
        void Load();
    }

    /// <summary>
    /// Defines a base controller that implements <see cref="System.ComponentModel.INotifyPropertyChanged"/>
    /// to notify of any change if needed
    /// </summary>
    public abstract class BaseController : ViewModel, IController
    {
        /// <summary>
        /// Loads all needed to make the controller work
        /// </summary>
        public abstract void Load();
    }
}
