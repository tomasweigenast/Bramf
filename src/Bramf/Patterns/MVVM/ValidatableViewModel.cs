using MvvmValidation;
using System;
using System.Collections;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Bramf.Patterns.MVVM
{
    /// <summary>
    /// A normal base view model class with validation helpers
    /// </summary>
    public class ValidatableViewModel : ViewModel, IValidatable, INotifyDataErrorInfo
    {
        #region Properties

        /// <summary>
        /// The validator to use
        /// </summary>
        protected ValidationHelper Validator { get; }

        #endregion

        #region Members

        private NotifyDataErrorInfoAdapter mNotifyDataErrorInfoAdapter;

        #endregion

        #region NotifyDataErrorInfo implementation

        /// <summary>
        /// Indicates if there is any error
        /// </summary>
        public bool HasErrors => mNotifyDataErrorInfoAdapter.HasErrors;

        /// <summary>
        /// The errors changed event
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add { mNotifyDataErrorInfoAdapter.ErrorsChanged += value; }
            remove { mNotifyDataErrorInfoAdapter.ErrorsChanged -= value; }
        }

        /// <summary>
        /// Gets the errors
        /// </summary>
        public IEnumerable GetErrors(string propertyName)
            => mNotifyDataErrorInfoAdapter.GetErrors(propertyName);

        #endregion

        #region IValidatable implementation

        /// <summary>
        /// Validate a property
        /// </summary>
        public Task<ValidationResult> Validate()
            => Validator.ValidateAllAsync();

        #endregion

        #region Constructors

        /// <summary>
        /// Default protected constructor
        /// </summary>
        protected ValidatableViewModel()
        {
            Validator = new ValidationHelper();

            mNotifyDataErrorInfoAdapter = new NotifyDataErrorInfoAdapter(Validator);
            mNotifyDataErrorInfoAdapter.ErrorsChanged += OnErrorsChanged;
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Fired when an error validation changes
        /// </summary>
        private void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
            => OnPropertyChanged(nameof(e.PropertyName));

        #endregion
    }
}
