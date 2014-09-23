using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SQLite;
using System.IO;
using Windows.Storage;

namespace Urbes
{
    public partial class SchedulesPage : PhoneApplicationPage
    {
        public string lineNumber;
        
        public SchedulesPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            lineNumber = "";

            if (NavigationContext.QueryString.TryGetValue("num", out lineNumber))
            {
                AllSchedules();
            }
        }

        private async void AllSchedules()
        {
            List<Urbes.MainPage.HORARIO> NextScheduleBairro;
            List<Urbes.MainPage.HORARIO> NextScheduleTerminal;

            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, "urbes_database.db"), true);

            DateTime now = DateTime.Now;
            int DayOfWeek = (int)now.DayOfWeek;
            int i = -1;

            if (DayOfWeek == 0)
            {
                Int32.TryParse(lineNumber, out i);
                var queryOne = conn.Table<Urbes.MainPage.HORARIO>().Where(x => (x.LIN_COD == i) && (x.HOR_SENTIDO_LINHA == "0") && (x.HOR_TIPO_DIA == "0")).OrderBy(m => m.HOR_HORARIO);
                var queryTwo = conn.Table<Urbes.MainPage.HORARIO>().Where(x => (x.LIN_COD == i) && (x.HOR_SENTIDO_LINHA == "1") && (x.HOR_TIPO_DIA == "0")).OrderBy(m => m.HOR_HORARIO);
                NextScheduleBairro = await queryOne.ToListAsync();
                NextScheduleTerminal = await queryTwo.ToListAsync();
            }
            else if (DayOfWeek == 6)
            {
                Int32.TryParse(lineNumber, out i);
                var queryOne = conn.Table<Urbes.MainPage.HORARIO>().Where(x => (x.LIN_COD == i) && (x.HOR_SENTIDO_LINHA == "0") && (x.HOR_TIPO_DIA == "2")).OrderBy(m => m.HOR_HORARIO);
                var queryTwo = conn.Table<Urbes.MainPage.HORARIO>().Where(x => (x.LIN_COD == i) && (x.HOR_SENTIDO_LINHA == "1") && (x.HOR_TIPO_DIA == "2")).OrderBy(m => m.HOR_HORARIO);
                NextScheduleBairro = await queryOne.ToListAsync();
                NextScheduleTerminal = await queryTwo.ToListAsync();
            }
            else
            {
                Int32.TryParse(lineNumber, out i);
                var queryOne = conn.Table<Urbes.MainPage.HORARIO>().Where(x => (x.LIN_COD == i) && (x.HOR_SENTIDO_LINHA == "0") && (x.HOR_TIPO_DIA == "1")).OrderBy(m => m.HOR_HORARIO);
                var queryTwo = conn.Table<Urbes.MainPage.HORARIO>().Where(x => (x.LIN_COD == i) && (x.HOR_SENTIDO_LINHA == "1") && (x.HOR_TIPO_DIA == "1")).OrderBy(m => m.HOR_HORARIO);
                NextScheduleBairro = await queryOne.ToListAsync();
                NextScheduleTerminal = await queryTwo.ToListAsync();
            }


            //MessageBox.Show(now);
            schedulesBairroListBox.ItemsSource = NextScheduleBairro;
            schedulesTerminalListBox.ItemsSource = NextScheduleTerminal;
        }

        private void hourBairroSelected_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void hourTerminalSelected_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void SegSex_Click(object sender, EventArgs e)
        {
            List<Urbes.MainPage.HORARIO> NextScheduleBairro;
            List<Urbes.MainPage.HORARIO> NextScheduleTerminal;

            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, "urbes_database.db"), true);

            int i = -1;
            string hour = DateTime.Now.ToString("HH:mm");

            Int32.TryParse(lineNumber, out i);
            var queryOne = conn.Table<Urbes.MainPage.HORARIO>().Where(x => (x.LIN_COD == i) && (x.HOR_SENTIDO_LINHA == "0") && (x.HOR_TIPO_DIA == "1")).OrderBy(m => m.HOR_HORARIO);
            var queryTwo = conn.Table<Urbes.MainPage.HORARIO>().Where(x => (x.LIN_COD == i) && (x.HOR_SENTIDO_LINHA == "1") && (x.HOR_TIPO_DIA == "1")).OrderBy(m => m.HOR_HORARIO);
            NextScheduleBairro = await queryOne.ToListAsync();
            NextScheduleTerminal = await queryTwo.ToListAsync();

            schedulesBairroListBox.ItemsSource = null;
            schedulesTerminalListBox.ItemsSource = null;
            
            schedulesBairroListBox.ItemsSource = NextScheduleBairro;
            schedulesTerminalListBox.ItemsSource = NextScheduleTerminal;
        }

        private async void Sab_Click(object sender, EventArgs e)
        {
            List<Urbes.MainPage.HORARIO> NextScheduleBairro;
            List<Urbes.MainPage.HORARIO> NextScheduleTerminal;

            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, "urbes_database.db"), true);

            int i = -1;
            string hour = DateTime.Now.ToString("HH:mm");

            Int32.TryParse(lineNumber, out i);
            var queryOne = conn.Table<Urbes.MainPage.HORARIO>().Where(x => (x.LIN_COD == i) && (x.HOR_SENTIDO_LINHA == "0") && (x.HOR_TIPO_DIA == "2")).OrderBy(m => m.HOR_HORARIO);
            var queryTwo = conn.Table<Urbes.MainPage.HORARIO>().Where(x => (x.LIN_COD == i) && (x.HOR_SENTIDO_LINHA == "1") && (x.HOR_TIPO_DIA == "2")).OrderBy(m => m.HOR_HORARIO);
            NextScheduleBairro = await queryOne.ToListAsync();
            NextScheduleTerminal = await queryTwo.ToListAsync();

            schedulesBairroListBox.ItemsSource = null;
            schedulesTerminalListBox.ItemsSource = null;

            schedulesBairroListBox.ItemsSource = NextScheduleBairro;
            schedulesTerminalListBox.ItemsSource = NextScheduleTerminal;
        }

        private async void DomFer_Click(object sender, EventArgs e)
        {
            List<Urbes.MainPage.HORARIO> NextScheduleBairro;
            List<Urbes.MainPage.HORARIO> NextScheduleTerminal;

            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, "urbes_database.db"), true);

            int i = -1;
            string hour = DateTime.Now.ToString("HH:mm");

            Int32.TryParse(lineNumber, out i);
            var queryOne = conn.Table<Urbes.MainPage.HORARIO>().Where(x => (x.LIN_COD == i) && (x.HOR_SENTIDO_LINHA == "0") && (x.HOR_TIPO_DIA == "0")).OrderBy(m => m.HOR_HORARIO);
            var queryTwo = conn.Table<Urbes.MainPage.HORARIO>().Where(x => (x.LIN_COD == i) && (x.HOR_SENTIDO_LINHA == "1") && (x.HOR_TIPO_DIA == "0")).OrderBy(m => m.HOR_HORARIO);
            NextScheduleBairro = await queryOne.ToListAsync();
            NextScheduleTerminal = await queryTwo.ToListAsync();

            schedulesBairroListBox.ItemsSource = null;
            schedulesTerminalListBox.ItemsSource = null;

            schedulesBairroListBox.ItemsSource = NextScheduleBairro;
            schedulesTerminalListBox.ItemsSource = NextScheduleTerminal;
        }
    }
}