using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace akarov.Controls.MVVM.Commands
{
    /// <summary>
    /// Implements the ICommand and wraps up all the verbose stuff so that you can just pass 2 delegates 1 for the CanExecute and one for the Execute
    /// </summary>
    public class SimpleCommand<T> : ICommand
    {
        /// <summary>
        /// Gets or sets the Predicate to execute when the CanExecute of the command gets called
        /// </summary>
        public Predicate<T> CanExecuteDelegate { get; set; }

        /// <summary>
        /// Gets or sets the action to be called when the Execute method of the command gets called
        /// </summary>
        public Action<T> ExecuteDelegate { get; set; }

        #region ICommand Members

        public SimpleCommand(Action<T> ExecuteDelegate):this(ExecuteDelegate,null)
        {

        }

        public SimpleCommand(Action<T> ExecuteDelegate, Predicate<T> CanExecuteDelegate)
        {
            if (ExecuteDelegate == null)
                throw new ArgumentNullException("ExecuteDelegate");

            this.ExecuteDelegate = ExecuteDelegate;
            this.CanExecuteDelegate = CanExecuteDelegate;
        }
        /// <summary>
        /// Checks if the command Execute method can run
        /// </summary>
        /// <param name="parameter">THe command parameter to be passed</param>
        /// <returns>Returns true if the command can execute. By default true is returned so that if the user of SimpleCommand does not specify a CanExecuteCommand delegate the command still executes.</returns>
        public bool CanExecute(Object parameter)
        {
            if (CanExecuteDelegate != null)
                return CanExecuteDelegate((T)parameter);
            return true;// if there is no can execute default to true
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Executes the actual command
        /// </summary>
        /// <param name="parameter">THe command parameter to be passed</param>
        public void Execute(object parameter)
        {
            if (ExecuteDelegate != null && CanExecute(parameter))
                ExecuteDelegate((T)parameter);
        }

        #endregion
    }

    public class SimpleCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the Predicate to execute when the CanExecute of the command gets called
        /// </summary>
        public Predicate<object> CanExecuteDelegate { get; set; }

        /// <summary>
        /// Gets or sets the action to be called when the Execute method of the command gets called
        /// </summary>
        public Action ExecuteDelegate { get; set; }

        #region ICommand Members

        public SimpleCommand(Action ExecuteDelegate)
            : this(ExecuteDelegate, null)
        {

        }

        public SimpleCommand(Action ExecuteDelegate, Predicate<object> CanExecuteDelegate)
        {
            if (ExecuteDelegate == null)
                throw new ArgumentNullException("ExecuteDelegate");

            this.ExecuteDelegate = ExecuteDelegate;
            this.CanExecuteDelegate = CanExecuteDelegate;
        }
        /// <summary>
        /// Checks if the command Execute method can run
        /// </summary>
        /// <param name="parameter">THe command parameter to be passed</param>
        /// <returns>Returns true if the command can execute. By default true is returned so that if the user of SimpleCommand does not specify a CanExecuteCommand delegate the command still executes.</returns>
        public bool CanExecute(Object parameter)
        {
            if (CanExecuteDelegate != null)
                return CanExecuteDelegate(null);
            return true;// if there is no can execute default to true
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Executes the actual command
        /// </summary>
        /// <param name="parameter">THe command parameter to be passed</param>
        public void Execute(object parameter)
        {
            if (ExecuteDelegate != null && CanExecute(parameter))
                ExecuteDelegate();
        }

        #endregion
    }
}
