using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TwitchPlaysAnything.Models
{
    public class TwitchControl
    {
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
        public string Process_Name { get; set; } = "VisualBoyAdvance";
        // public string Process_Name { get; set; } = "Notepad";
        public WindowsAPI WindowsAPI { get; set; } = new WindowsAPI();
        public static void FocusWindow(string processName)
        {
            // System.Diagnostics.Process[] p = System.Diagnostics.Process.GetProcessesByName(processName);

            Process[] processes = Process.GetProcesses();
            try
            {
                foreach (Process process in processes)
                {
                    if (process.ProcessName == processName)
                    {
                        Console.WriteLine(processName);
                        IntPtr mainWindowHandle = process.MainWindowHandle;
                        if (mainWindowHandle != IntPtr.Zero)
                        {
                            // Bring the window to the front
                            NativeMethods.SetForegroundWindow(mainWindowHandle);
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        static void Sleep(int length = 25)
        {
            System.Threading.Thread.Sleep(length);
        }
        public static void RunIRC(string pass)
        {
            int port;
            string buf, nick, owner, server, chan;
            System.Net.Sockets.TcpClient sock = new System.Net.Sockets.TcpClient();
            System.IO.TextReader input;
            System.IO.TextWriter output;

            //Read .ini file
            string inifilepath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini");
            INIFile ini = new INIFile(inifilepath);
            string TUsername = ini.Read("Twitch", "Username");
            //--------------
            nick = TUsername;
            owner = TUsername;
            server = "irc.twitch.tv";
            port = 6667;
            chan = "#" + TUsername;

            // Console.Clear();

            //Connect to irc server and get input and output text streams from TcpClient.
            sock.Connect(server, port);
            if (!sock.Connected)
            {
                Console.WriteLine("Failed to connect!");
                return;
            }
            else
            {
                // Console.WriteLine("Conected to " + chan);
            }
            input = new System.IO.StreamReader(sock.GetStream());
            output = new System.IO.StreamWriter(sock.GetStream());

            //Starting USER and NICK login commands 
            output.Write(
               "USER " + nick + "\n" +
               "PASS " + pass + "\r\n" +
               "NICK " + nick + "\r\n"
            );
            output.Flush();

            //Process each line received from irc server
            while ((buf = input.ReadLine()) != null)
            {

                //Display received irc message
                //
                if (buf != null)
                {
                    //Console.WriteLine(buf);
                    checkCommands(buf);

                    //Send pong reply to any ping messages
                    if (buf.StartsWith("PING ") || (buf.Contains("PING") && buf.Contains("mwax321"))) { output.Write(buf.Replace("PING", "PONG") + "\r\n"); output.Flush(); }

                    if (buf.Contains("mwax321") && buf.Contains("clear"))
                    {
                        Console.Clear();
                    }

                    if (buf[0] != ':') continue;

                    /* IRC commands come in one of these formats:
                     * :NICK!USER@HOST COMMAND ARGS ... :DATA\r\n
                     * :SERVER COMAND ARGS ... :DATA\r\n
                     */

                    //After server sends 001 command, we can set mode to bot and join a channel
                    if (buf.Split(' ')[1] == "001")
                    {
                        output.Write(
                           "MODE " + nick + " +B\r\n" +
                           "JOIN " + chan + "\r\n"
                        );
                        output.Flush();
                    }
                }
                else
                {
                    // Console.WriteLine("Null detected... ");
                    RunIRC(pass);
                }
            }
            // Console.WriteLine("Null detected... ");
            RunIRC(pass);
        }
        public static void Button(char btn, int sleep = 50)
        {
            FocusWindow("Notepad");
            SimulateKeyPress(btn);
            //PressKey(btn, true);
            //Sleep(sleep);
            //PressKey(btn, false);
        }

        private static bool MapKeys(string command)
        {
            bool result = true;
            Button('A');
            return result;
        }
        private static void checkCommands(string input, int Player = 0)
        {
            //TODO: Refactor this into one regex. I got lazy and just piled crap upon crap

            //Look for messages from users and then get the user and command
            string regex = @":.* PRIVMSG.* :";
            string regexFullMatch = @"^:(\w+)(?:!(\w+)@([\w\.]+))? PRIVMSG (#\w+) :(.+)$";

            if (Regex.IsMatch(input, regex))
            {
                //User doesn't have a userID
                string user = "god";
                if (Regex.IsMatch(input, regexFullMatch))
                {
                    var m = Regex.Match(input, regexFullMatch);
                    user = m.Groups[1].Value;
                }
                string command = Regex.Replace(input, regex, "");
                bool commandReal = MapKeys(command);

                //Send command to log window and console
                //if (commandReal)
                //{
                //    Console.WriteLine(user + ": " + command);
                //}
            }
        }
        public static void SimulateKeyPress(char ch)
        {
            // Convert the character to a string
            // string key = ch.ToString();

            // Check if the character is a special key
            if (char.IsLetterOrDigit(ch) || char.IsPunctuation(ch))
            {
                // Simulate key press
                Key ke = Key.G;
                if (Keyboard.PrimaryDevice != null && Application.Current?.MainWindow != null)
                {
                    //KeyboardEventArgs args = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(Application.Current.MainWindow), 0, ke)
                    //{
                    //    RoutedEvent = Keyboard.KeyDownEvent
                    //};
                    //InputManager.Current.ProcessInput(args);

                    //// Introduce a delay using a different method
                    //Task.Delay(25).Wait(); // Note: Using Wait() is not recommended in production code.

                    //// Simulate key up event
                    //KeyboardEventArgs argsUp = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(Application.Current.MainWindow), 0, ke)
                    //{
                    //    RoutedEvent = Keyboard.KeyUpEvent
                    //};
                    //InputManager.Current.ProcessInput(argsUp);

                    const int VK_UP = 0x31; //a key
                    const int VK_DOWN = 0x28;  //down key
                    const int VK_LEFT = 0x25;
                    const int VK_RIGHT = 0x27;
                    const uint KEYEVENTF_KEYUP = 0x0002;
                    const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
                    int press()
                    {
                        //Press the key
                        keybd_event((byte)VK_UP, 0, KEYEVENTF_EXTENDEDKEY | 0, 0);
                        return 0;
                    }
                    press();
                }
            }
            //else
            //{
            //    // For special characters, use special key codes
            //    switch (ch)
            //    {
            //        case ' ':
            //            SendKeys.SendWait("{SPACE}");
            //            break;
            //        case '\t':
            //            SendKeys.SendWait("{TAB}");
            //            break;
            //        case '\n':
            //            SendKeys.SendWait("{ENTER}");
            //            break;
            //        case '\r':
            //            SendKeys.SendWait("{ENTER}");
            //            break;
            //        default:
            //            // For unsupported characters, do nothing
            //            Console.WriteLine("Unsupported character: " + ch);
            //            break;
            //    }
            //}
        }
    }
}
