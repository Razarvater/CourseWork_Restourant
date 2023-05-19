using mvvm;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WindowControllers
{
    public partial class ViewModelNavigation : NotifyPropertyChanged
    {
        private Dock selectedMenu;
        /// <summary>
        /// Выбранное меню Mode
        /// </summary>
        public Dock SelectedMenu
        {
            get => selectedMenu;
            set
            {
                selectedMenu = value;
                OnPropertyChanged();
            }
        }

        private bool isMenuOn = true;
        /// <summary>
        /// Включено ли меню
        /// </summary>
        public bool IsMenuOn
        { 
            get => isMenuOn;
            set
            { 
                isMenuOn = value;
                OnPropertyChanged();
            }
        }

        private MenuMode mode;
        /// <summary>
        /// Текущий режим меню
        /// </summary>
        public MenuMode Mode 
        {
            get => mode;
            set 
            {
                mode = value;
                OnPropertyChanged();

                switch (mode)
                {
                    case MenuMode.VerticalMenu:
                        IsMenuOn = true;
                        selectedMenu = Dock.Left;
                        break;

                    case MenuMode.HorizontalMenu:
                        IsMenuOn = true;
                        selectedMenu = Dock.Top;
                        break;

                    case MenuMode.None:
                        {
                            IsMenuOn = false;
                            if (tabsList.Count <= 0) return;

                            SelectedPage = tabsList.First().PageContent;
                        }
                        break;
                }
            }
        }
        private MenuMode savedMode;
        public MenuMode SavedMode
        {
            get => savedMode;
            set 
            {
                savedMode = value;
                Mode = savedMode;
            } 
        }

        private UserControl selectedPage;
        /// <summary>
        /// Текущая страница
        /// </summary>
        public UserControl SelectedPage
        {
            get => selectedPage;
            set
            {
                selectedPage = value;
                OnPropertyChanged();
            }
        }
        public MenuItem lastSelected = null;

        private ObservableCollection<MenuItem> tabsList = new ObservableCollection<MenuItem>();
        /// <summary>
        /// Список с вкладками меню (поддерживает сколько угодно уровней вложенности)
        /// </summary>
        public ObservableCollection<MenuItem> TabsList
        {
            get => tabsList;
            set
            {
                tabsList = value;
                OnPropertyChanged();
            }
        }

        public ViewModelNavigation()
        {
            //Инициализируем функционал окна
            InitWindowProp();

            //Дефолтное меню
            Mode = MenuMode.VerticalMenu;

            TabsList.CollectionChanged += TabsListNewElement;
        }

        /// <summary>
        /// Обработчик события добавления нового элемента в коллекцию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabsListNewElement(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {   
                for (int i = 0; i < e.NewItems.Count; i++)
                {
                    MenuItem redItem = (e.NewItems[i] as MenuItem);
                    redItem.Navigate = new Command
                    (
                        (obj) =>
                        {
                            if (lastSelected == redItem) return;
                            if(lastSelected != null) lastSelected.IsSelected = false;
                            lastSelected = redItem;

                            SelectedPage = redItem.PageContent;
                            redItem.IsSelected = true;
                        }
                    );
                }
            }
            if (TabsList.Count == 0)
                Mode = MenuMode.None;
            else
                Mode = savedMode;
        }
    }

    /// <summary>
    /// Вкладка меню
    /// </summary>
    public class MenuItem : NotifyPropertyChanged
    {
        private string name;
        /// <summary>
        /// Название вкладки
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private Icon iconSource;
        public ImageSource IconSource
        {
            get
            {
                //TODO: возможно отрефакторить, возможно
                if (iconSource == null) return null;
                Bitmap bmp = iconSource.ToBitmap();
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap
                (
                    bmp.GetHbitmap(),
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions()
                );
            }
        }

        private UserControl pageContent;
        /// <summary>
        /// Страница
        /// </summary>
        public UserControl PageContent
        {
            get
            {
                if (ContentPageType != null && pageContent == null)
                {
                    pageContent = (UserControl)Activator.CreateInstance(ContentPageType);
                    return pageContent;
                }

                return pageContent;
            }
            set
            {
                pageContent = value;
                OnPropertyChanged();
            }
        }

        private Command navigate;
        public Command Navigate
        { 
            get => navigate;
            set
            {   
                if(navigate == null)
                    navigate = value;
                OnPropertyChanged();
            }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            { 
                isSelected = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Тип страницы
        /// </summary>
        private Type ContentPageType;

        public MenuItem(string Name, Type contentPageType, Icon icon)
        {
            this.Name = Name;
            ContentPageType = contentPageType;
            iconSource = icon;
        }

    }

    public enum MenuMode
    { 
        VerticalMenu,
        HorizontalMenu,
        None
    }
}