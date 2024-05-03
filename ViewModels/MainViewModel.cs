using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TwitchPlaysAnything.Commands;
using TwitchPlaysAnything.Models;

namespace TwitchPlaysAnything.ViewModels
{
    public class MainViewModel
    {
        public ICommand StartTwitchControlCommand {  get; set; }
        public ObservableCollection<KeyBindingSet>? KeyBindingSets { get; set; }

        public MainViewModel() 
        {
            StartTwitchControlCommand = new RelayCommand(StartTwitchControl, CanStartTwitchControl);
            KeyBindingSets = new ObservableCollection<KeyBindingSet>();
        }

        private void StartTwitchControl(object obj)
        {
            // --- FOR LOCAL TESTING -----

            //Console.Write("Enter test command : ");
            //string btn = Console.ReadLine();

            //FocusWindow(Process_Name);
            //MapKeys(btn);

            //Main(args);
            // --- END LOCAL TESTING ----

            // ---  START PROGRAM ----
            //Popup the log window for commands
            //Thread formThread = new Thread(StartWindow);
            //formThread.Start();

            //Get your oauth info for IRC chat.  http://twitchapps.com/tmi/
            //TODO: Add OAuth automatically
            //Read .ini file
            string inifilepath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini");
            INIFile ini = new INIFile(inifilepath);
            string TOauth = ini.Read("Twitch", "Oauth");
            //--------------
            // Console.Write(TOauth);
            string pass = TOauth;

            //Start IRC process
            TwitchControl.RunIRC(pass);
        }

        private bool CanStartTwitchControl(object arg)
        {
            return true;
        }
    }
}
