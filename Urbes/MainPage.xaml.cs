using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Urbes.Resources;
using SQLite;
using Windows.Storage;
using System.IO;
using System.IO.IsolatedStorage;

namespace Urbes
{
    public partial class MainPage : PhoneApplicationPage
    {
        private string dataRead;
        
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            CopyDatabase();
            //Gogo();
            GogoTwo();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            allLinesListBox.SelectedIndex = favLinesListBox.SelectedIndex = -1;

            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private async void Gogo()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, "urbes_database.db"), true);

            var query = conn.Table<HORARIO>().Where(x => x.HOR_COD == 14127);
            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                dataRead = string.Format("{0}", item.HOR_HORARIO);
                //Test.Text = dataRead;
            }
        }

        private async void GogoTwo()
        {
            List<LINHA> AllLines;
            List<LINHA> FavLines;
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, "urbes_database.db"), true);

            var queryOne = conn.Table<LINHA>();
            var queryTwo = conn.Table<LINHA>().Where(x => x.LIN_FAVORITA == "1");
            AllLines = await queryOne.ToListAsync();
            FavLines = await queryTwo.ToListAsync();

            allLinesListBox.ItemsSource = AllLines;
            favLinesListBox.ItemsSource = FavLines;
        }

        public class LINHA
        {
            [PrimaryKey]
            public int LIN_COD { get; set; }
            public string LIN_NUMERO { get; set; }
            public string LIN_DESCRICAO { get; set; }
            public string LIN_FAVORITA { get; set; }
            public int TAR_COD { get; set; }
            public int LIN_ORDEM_APRES { get; set; }
            public string LIN_SIGLA_BAIRRO { get; set; }
            public string LIN_SIGLA_TERMINAL { get; set; }
        }

        public class HORARIO
        {
            [PrimaryKey, AutoIncrement]
            public int HOR_COD { get; set; }
            public string HOR_HORARIO { get; set; }
            public string HOR_TIPO_DIA { get; set; }
            public string HOR_SENTIDO_LINHA { get; set; }
            public string HOR_ONIBUS_ADAPTADO { get; set; }
            public int LIN_COD { get; set; }
        }

        private void CopyDatabase()
        {

            IsolatedStorageFile ISF = IsolatedStorageFile.GetUserStoreForApplication();
            String DBFile = "urbes_database.db";
            if (!ISF.FileExists(DBFile)) CopyFromContentToStorage(ISF, "Assets/urbes_database.db", DBFile);

        }

        private void CopyFromContentToStorage(IsolatedStorageFile ISF, String SourceFile, String DestinationFile)
        {
            Stream Stream = Application.GetResourceStream(new Uri(SourceFile, UriKind.Relative)).Stream;
            IsolatedStorageFileStream ISFS = new IsolatedStorageFileStream(DestinationFile, System.IO.FileMode.Create, System.IO.FileAccess.Write, ISF);
            CopyStream(Stream, ISFS);
            ISFS.Flush();
            ISFS.Close();
            Stream.Close();
            ISFS.Dispose();
        }

        private void CopyStream(Stream Input, IsolatedStorageFileStream Output)
        {
            Byte[] Buffer = new Byte[5120];
            Int32 ReadCount = Input.Read(Buffer, 0, Buffer.Length);
            while (ReadCount > 0)
            {
                Output.Write(Buffer, 0, ReadCount);
                ReadCount = Input.Read(Buffer, 0, Buffer.Length);
            }
        }

        private void GoToInfoPage_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/InfoPage.xaml", UriKind.Relative));
        }

        private void lineSelected_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (allLinesListBox.SelectedIndex == -1 && favLinesListBox.SelectedIndex == -1)
                return;

            LINHA favSelected = favLinesListBox.SelectedItem as LINHA;
            LINHA allSelected = allLinesListBox.SelectedItem as LINHA;

            if (favSelected != null)
                NavigationService.Navigate(new Uri("/InfoPage.xaml?num=" + favSelected.LIN_COD, UriKind.Relative));
            else
                NavigationService.Navigate(new Uri("/InfoPage.xaml?num=" + allSelected.LIN_COD, UriKind.Relative));
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}