using mvvm;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Controls;

namespace WindowControllers
{
    public partial class ViewModelNavigation : NotifyPropertyChanged
    {
        private Dock selectedMenu;
        /// <summary>
        /// Выбранное меню
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
                        IsMenuOn = false;
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
                    TabsInit(redItem);
                }
            }
            if (TabsList.Count == 0)
            {
                Mode = MenuMode.None;
            }
            else if (TabsList.Count == 1 && TabsList.First().Pages == null)
            {
                Mode = MenuMode.None;
                SelectedPage = TabsList.First().PageContent;
            }
            else
                Mode = savedMode;
        }

        /// <summary>
        /// Рекурсивный обход по всем вложенным menu Для инициализации комманд
        /// </summary>
        /// <param name="item"></param>
        private void TabsInit(MenuItem item)
        {
            if (item.Pages == null || item.Pages.Count == 0)
            {
                item.Navigate = new Command
                (
                    (obj) => SelectedPage = item.PageContent
                );
                return;
            }

            for (int i = 0; i < item.Pages.Count; i++)
                TabsInit(item.Pages[i]);
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

        private UserControl pageContent;
        /// <summary>
        /// Страница
        /// </summary>
        public UserControl PageContent
        {
            get
            {
                if (ContentPageType != null && pageContent == null)
                    pageContent = (UserControl)Activator.CreateInstance(ContentPageType);

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

        /// <summary>
        /// Тип страницы
        /// </summary>
        private Type ContentPageType;

        /// <summary>
        /// Элементы вкладки
        /// </summary>
        public ObservableCollection<MenuItem> Pages { get; set; }

        public MenuItem(string Name, Type contentPageType)
        {
            this.Name = Name;
            ContentPageType = contentPageType;
        }

        public MenuItem(string Name, ObservableCollection<MenuItem> Pages, Type contentPageType = null) : this (Name, contentPageType) =>
            this.Pages = Pages;
    }

    public enum MenuMode
    { 
        VerticalMenu,
        HorizontalMenu,
        None
    }
}