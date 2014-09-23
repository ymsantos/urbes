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
    // HOR_TIPO_DIA
    // 0 - Domingo e feriados
    // 1 - seg-sex
    // 2 - sabados
    // HOR_SENTIDO_LINHA
    // 0 - Bairro
    // 1 - Terminal
    public partial class InfoPage : PhoneApplicationPage
    {

        public string lineNumber;
        
        public InfoPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            lineNumber = "";

            if (NavigationContext.QueryString.TryGetValue("num", out lineNumber))
            {
                NextSchedule();
            }
        }

        
        //Corrigir quando tipo do dia seguinte não for o mesmo do atual no caso de ter ficado null no prox horario
        private async void NextSchedule()
        {
            Urbes.MainPage.HORARIO NextScheduleBairro = null;
            Urbes.MainPage.HORARIO NextScheduleTerminal = null;
            List<InfoPageData> NextHours = new List<InfoPageData>();

            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, "urbes_database.db"), true);

            DateTime now = DateTime.Now;
            int DayOfWeek = (int)now.DayOfWeek;
            int i = -1;
            string hour = DateTime.Now.ToString("HH:mm");

            if (DayOfWeek == 0)
            {
                Int32.TryParse(lineNumber, out i);
                var queryOne = conn.Table<Urbes.MainPage.HORARIO>().Where(x => (x.LIN_COD == i) && (x.HOR_SENTIDO_LINHA == "0") && (x.HOR_TIPO_DIA == "0")).OrderBy(m => m.HOR_HORARIO);
                var queryTwo = conn.Table<Urbes.MainPage.HORARIO>().Where(x => (x.LIN_COD == i) && (x.HOR_SENTIDO_LINHA == "1") && (x.HOR_TIPO_DIA == "0")).OrderBy(m => m.HOR_HORARIO);
                var resultOne = await queryOne.ToListAsync();
                var resultTwo = await queryTwo.ToListAsync();

                foreach (var item in resultOne)
                {
                    if (DateTime.ParseExact(item.HOR_HORARIO, "HH:mm", null) >= DateTime.ParseExact(hour, "HH:mm", null))
                    {
                        NextScheduleBairro = item;
                        break;
                    }
                }

                foreach (var item in resultTwo)
                {
                    if (DateTime.ParseExact(item.HOR_HORARIO, "HH:mm", null) >= DateTime.ParseExact(hour, "HH:mm", null))
                    {
                        NextScheduleTerminal = item;
                        break;
                    }
                }

                if(NextScheduleBairro == null) {
                    NextScheduleBairro = resultOne[0];
                }
                if(NextScheduleTerminal == null) {
                    NextScheduleTerminal = resultTwo[0];
                }
            }
            else if (DayOfWeek == 6)
            {
                Int32.TryParse(lineNumber, out i);
                var queryOne = conn.Table<Urbes.MainPage.HORARIO>().Where(x => (x.LIN_COD == i) && (x.HOR_SENTIDO_LINHA == "0") && (x.HOR_TIPO_DIA == "2")).OrderBy(m => m.HOR_HORARIO);
                var queryTwo = conn.Table<Urbes.MainPage.HORARIO>().Where(x => (x.LIN_COD == i) && (x.HOR_SENTIDO_LINHA == "1") && (x.HOR_TIPO_DIA == "2")).OrderBy(m => m.HOR_HORARIO);
                var resultOne = await queryOne.ToListAsync();
                var resultTwo = await queryTwo.ToListAsync();

                foreach (var item in resultOne)
                {
                    if (DateTime.ParseExact(item.HOR_HORARIO, "HH:mm", null) >= DateTime.ParseExact(hour, "HH:mm", null))
                    {
                        NextScheduleBairro = item;
                        break;
                    }
                }

                foreach (var item in resultTwo)
                {
                    if (DateTime.ParseExact(item.HOR_HORARIO, "HH:mm", null) >= DateTime.ParseExact(hour, "HH:mm", null))
                    {
                        NextScheduleTerminal = item;
                        break;
                    }
                }

                if(NextScheduleBairro == null) {
                    NextScheduleBairro = resultOne[0];
                }
                if(NextScheduleTerminal == null) {
                    NextScheduleTerminal = resultTwo[0];
                }
            }
            else
            {
                Int32.TryParse(lineNumber, out i);
                var queryOne = conn.Table<Urbes.MainPage.HORARIO>().Where(x => (x.LIN_COD == i) && (x.HOR_SENTIDO_LINHA == "0") && (x.HOR_TIPO_DIA == "1")).OrderBy(m => m.HOR_HORARIO);
                var queryTwo = conn.Table<Urbes.MainPage.HORARIO>().Where(x => (x.LIN_COD == i) && (x.HOR_SENTIDO_LINHA == "1") && (x.HOR_TIPO_DIA == "1")).OrderBy(m => m.HOR_HORARIO);
                var resultOne = await queryOne.ToListAsync();
                var resultTwo = await queryTwo.ToListAsync();

                foreach (var item in resultOne)
                {
                    if (DateTime.ParseExact(item.HOR_HORARIO, "HH:mm", null) >= DateTime.ParseExact(hour, "HH:mm", null))
                    {
                        NextScheduleBairro = item;
                        break;
                    }
                }

                foreach (var item in resultTwo)
                {
                    if (DateTime.ParseExact(item.HOR_HORARIO, "HH:mm", null) >= DateTime.ParseExact(hour, "HH:mm", null))
                    {
                        NextScheduleTerminal = item;
                        break;
                    }
                }

                if(NextScheduleBairro == null) {
                    NextScheduleBairro = resultOne[0];
                }
                if(NextScheduleTerminal == null) {
                    NextScheduleTerminal = resultTwo[0];
                }
            }
            
            InfoPageData temp = new InfoPageData();
            temp.SAIDA_BAIRRO = NextScheduleBairro.HOR_HORARIO;
            temp.SAIDA_TERMINAL = NextScheduleTerminal.HOR_HORARIO;
            //MessageBox.Show(now);
            NextHours.Add(temp);
            infoListBox.ItemsSource = NextHours;
        }

        private void GoToSchedulesPage_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SchedulesPage.xaml?num=" + lineNumber, UriKind.Relative));
        }

        private void details_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public class InfoPageData
        {
            public string SAIDA_BAIRRO { get; set; }
            public string SAIDA_TERMINAL { get; set; }
        }
    }
}