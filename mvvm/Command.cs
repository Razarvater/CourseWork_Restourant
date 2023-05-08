using System;
using System.Windows.Input;

namespace mvvm
{
    public class Command : ICommand
    {
        private Action<object> execute;//метод выполняющийся при вызове команды
        private Func<object, bool> canExecute;//метод определяющий возможность выполнения команды

        public bool CanExecutes = true;
        //При изменении возможности выполнения команды
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="execute">Выполняющийся метод</param>
        /// <param name="canExecute">Метод отвечающий за возможность выполнения команды</param>
        public Command(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Может ли команда выполниться
        /// Если команда не назначена то да, дальше выполнить команду для проверки
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool CanExecute(object param) =>
            this.canExecute == null || this.canExecute(param);

        /// <summary>
        /// Выполнение команды
        /// </summary>
        /// <param name="param"></param>
        public void Execute(object param) =>
            this.execute?.Invoke(param);
    }
}