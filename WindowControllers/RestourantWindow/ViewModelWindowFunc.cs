using mvvm;
using System.ComponentModel;
using System.Windows;
using System;

namespace WindowControllers
{
    public partial class ViewModelNavigation : NotifyPropertyChanged
    {
        /// <summary>
        /// Комманда для закрытия окна
        /// </summary>
        private Command closeWindowCommand;
        public Command CloseWindowCommand
        {
            get => closeWindowCommand;
            set
            {
                closeWindowCommand = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Комманда для максимального размера окна
        /// </summary>
        private Command maxWindowCommand;
        public Command MaxWindowCommand
        {
            get => maxWindowCommand;
            set
            {
                maxWindowCommand = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Комманда для свёртывания окна
        /// </summary>
        private Command minWindowCommand;
        public Command MinWindowCommand
        {
            get => minWindowCommand;
            set
            {
                minWindowCommand = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Комманда для свёртывания окна
        /// </summary>
        private Command minMaxWindowCommand;
        public Command MinMaxWindowCommand
        {
            get => minMaxWindowCommand;
            set
            {
                minMaxWindowCommand = value;
                OnPropertyChanged();
            }
        }

        private double lft;
        public double Lft
        {
            get => lft;
            set
            {
                lft = value;
                OnPropertyChanged();
            }
        }
        private double tp;
        public double Tp
        {
            get => tp;
            set
            {
                tp = value;
                OnPropertyChanged();
            }
        }
        private double height = 450;
        public double Height
        {
            get => height;
            set
            {
                height = value;
                OnPropertyChanged();
            }
        }
        private double width = 800;
        public double Width
        {
            get => width;
            set
            {
                width = value;
                OnPropertyChanged();
            }
        }
        private double[] SavedWindowPos = new double[4] { -1, -1, -1, -1 };

        private WindowState realMode = WindowState.Normal;
        public WindowState RealMode
        {
            get => realMode;
            set
            {
                realMode = value;
                OnPropertyChanged();
            }
        }

        private ResizeMode resMode;
        public ResizeMode ResMode
        {
            get => resMode;
            set
            {
                resMode = value;
                OnPropertyChanged();
            }
        }

        private WindowState windowStateM;
        public WindowState WindowStateM
        {
            get => windowStateM;
            set
            {
                windowStateM = value;
                OnPropertyChanged();
            }
        }

        public void InitWindowProp()
        {
            RealMode = WindowState.Normal;
            WindowStateM = WindowState.Normal;
            ResMode = ResizeMode.CanResize;

            SystemParameters.StaticPropertyChanged += SystemParameters_StaticPropertyChanged;

            CloseWindowCommand = new Command((obj) => SystemCommands.CloseWindow((Window)obj));

            MaxWindowCommand = new Command
            (
                (obj) =>
                {
                    if (RealMode == WindowState.Maximized)
                    {
                        RealMode = WindowState.Normal;
                        ResMode = ResizeMode.CanResize;
                        Lft = SavedWindowPos[0];
                        Tp = SavedWindowPos[1];
                        Height = SavedWindowPos[2];
                        Width = SavedWindowPos[3];
                    }
                    else
                    {
                        SavedWindowPos[0] = Lft;
                        SavedWindowPos[1] = Tp;
                        SavedWindowPos[2] = Height;
                        SavedWindowPos[3] = Width;
                        RealMode = WindowState.Maximized;
                        ResMode = ResizeMode.NoResize;
                        Lft = SystemParameters.WorkArea.TopLeft.X;
                        Tp = SystemParameters.WorkArea.TopLeft.Y;
                        Height = SystemParameters.WorkArea.Height;
                        Width = SystemParameters.WorkArea.Width;
                    }
                }
            );

            MinMaxWindowCommand = new Command
            (
                (obj) =>
                {
                    Point c = (Point)obj;
                    RealMode = WindowState.Normal;
                    ResMode = ResizeMode.CanResize;
                    Lft = c.X - SavedWindowPos[3] / 2;
                    Tp = c.Y - 15;
                    Height = SavedWindowPos[2];
                    Width = SavedWindowPos[3];
                }
            );

            //Рандомная начальная позиция окна в Normal Mode
            Random rnd = new Random();
            Lft = rnd.Next(0, (int)SystemParameters.WorkArea.Width - 800);
            Tp = rnd.Next(0, (int)SystemParameters.WorkArea.Height - 450);

            MinWindowCommand = new Command
            (
                (obj) => WindowStateM = WindowState.Minimized
            );
        }

        private void SystemParameters_StaticPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (RealMode == WindowState.Maximized)
            {
                Lft = SystemParameters.WorkArea.TopLeft.X;
                Tp = SystemParameters.WorkArea.TopLeft.Y;
                Height = SystemParameters.WorkArea.Height;
                Width = SystemParameters.WorkArea.Width;
            }
        }
    }
}