using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using System.Diagnostics.Eventing.Reader;
using System.Security.Policy;
using System.Windows.Markup;
using System.ComponentModel.Design;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Working_time_management
{
    public static class ProcessingCSV
    {
        public enum LogInResult
        {
            IDNotFound,
            PwdIncorrect,
            UserCorrect,
            AdminCorrect,
            TimeDetectionIdFound,
            TimeDetectionIdNotFound,
        }
        public static readonly string idPwdPath = @"..\..\..\data\id_pwd\id_pwd.csv";

        public static string getWorkerInformationPath(string id)
        {
            return @"..\..\..\data\worker_information\" + id + @"\worker_information.csv";
        }

        public static string getUserRequestPath(string id)
        {
            return @"..\..\..\data\worker_information\" + id + @"\request.csv";
        }
        public static string getUserPathWorkingTimeCSV(string id)
        {
            return @"..\..\..\data\worker_information\" + id + @"\working_time.csv";
        }

        public static string getWorkingTimeInformationPath(string id)
        {
            return @"..\..\..\data\worker_information\" + id + @"\working_time_information.csv";
        }

        public static int checkLogIn(string userID, string userPWD, bool isLogIn)
        {
            LogInResult inputCorrect = LogInResult.IDNotFound;

            foreach (string line in File.ReadLines(idPwdPath))
            {
                string[] data = line.Split(';');
                string ID = data[0];
                string pwd = data[1];
                if (ID == userID && ID != "ID")
                {
                    if (isLogIn == false)
                    {
                        inputCorrect = LogInResult.TimeDetectionIdFound;

                        break;
                    }
                    else 
                    { 
                        if (string.Compare(pwd, userPWD) == 0)
                        {
                            if (string.Compare(userID, "123123") == 0)
                            {
                                inputCorrect = LogInResult.AdminCorrect;
                                break;
                            }
                            else
                            {
                                inputCorrect = LogInResult.UserCorrect;
                                break;
                            }

                        }
                        else
                        {
                            inputCorrect = LogInResult.PwdIncorrect;
                            break;
                        }
                    }
                }
                else
                {
                    inputCorrect = LogInResult.TimeDetectionIdNotFound;
                }
            }
            return (int) inputCorrect;
        }
        public static void addUserToID_PWDCSV(string[] pwdID)
        {
            File.AppendAllLines(idPwdPath, pwdID, Encoding.UTF8);
        }
        public static bool addAbsenceToAbsenceCSV(string ID, string startDate, string endDate, string reason)
        {
            bool IDFound = false;
            foreach (string line in File.ReadLines(idPwdPath))
            {
                string[] data = line.Split(';');
                string id = data[0];
                if (id == ID)
                {
                    IDFound = true;
                    string[] textForCSV = { ID + ';' + startDate + ';' + endDate + ';' +  reason };
                    File.AppendAllLines(@"..\..\..\data\admin\absences.csv", textForCSV, Encoding.UTF8);
                    break;
                }
            }
            return IDFound;
        }
        public static void addUserToWorkerInformationCSV(string id, string lastName, string firstName, string DateOfBirth, string residence) 
        {
            string[] data = { "Nachname" + ";" + "Vorname" + ";" + "Geburtsdatum" + ";" + "Wohnort" + ";" + "Status" + "\n" + lastName + ";" + firstName + ";" + DateOfBirth + ";" + residence + ";" + "Abgemeldet" };
            string workerInformationPath = getWorkerInformationPath(id);
            string userRequestPath = getUserRequestPath(id);
            File.WriteAllLines(workerInformationPath, data);
            File.Create(userRequestPath);
        }
        public static void editUserPwdToCSV(string id, string newPwd)           // id und newPwd muss aus editUser.xaml.cs übergeben werden
        { 
                string[] allLines = File.ReadAllLines(idPwdPath);
                for (int i = 0; i < allLines.Length; i++)
                {
                    string[] data = allLines[i].Split(';');
                    string ID = data[0];
                    string pwd = data[1];
                    if (ID == id && ID != "ID")
                    {
                        allLines[i] = ID + ";" + newPwd;
                        File.WriteAllLines(idPwdPath, allLines, Encoding.UTF8);
                        break;
                    }
                }
        }
        public static void editUserToWorkerInformationCSV(string id, string lastName, string firstName, string DateOfBirth, string residence, string status)
        {
            string[] data = { lastName + ";" + firstName + ";" + DateOfBirth + ";" + residence + ";" + status};
            string workerInformationPath = getWorkerInformationPath(id);
            File.WriteAllLines(workerInformationPath, data);
        }
        public static string GetWorkerInformation(string id)
        {
            string workerInformation = "";
            string workerInformationPath = getWorkerInformationPath(id);
            foreach (string line in File.ReadAllLines(workerInformationPath))
            {
                string[] data = line.Split(';');
                string lastName = data[0];
                if (lastName != "Nachname")
                {
                    workerInformation = line;
                }
            }
            return workerInformation; 
        }

        public static string[] getAllWorkerRequests(string id)
        {
            return File.ReadAllLines(getUserRequestPath(id));
        }

        public static void deleteUserPwdInCSV(string id)            // id und newPwd muss aus editUser.xaml.cs übergeben werden
        {                                                                       
            string[] allLines = File.ReadAllLines(idPwdPath);       // gesamte CSV auslesen
            string newCSV = "";                                              
            for (int i = 0; i < allLines.Length; i++)                
            {                                                                                                                     
                string[] data = allLines[i].Split(';');                               
                string ID = data[0];
                if (ID != id)                                       // wenn ID ungleich der aktuellen id ist, wird die ID und das Passwort in newCSV geschrieben
                {
                    newCSV += allLines[i] + "\n";
                }                                                   // wenn ID gleich der aktuellen ID ist, wird die ID und das Passwort einfach weggelassen und somit gelöscht
            }
            File.WriteAllText(idPwdPath, newCSV, Encoding.UTF8);    // alte CSV-Datei überschreieben -> Wert der weggelassen wurde, nun nicht mehr enthalten
        }
        public static void addWorkingTimeCSV(string id)
        {
            string[] data = { "Datum;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit" +
                    ";Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit" };
            File.WriteAllLines(getUserPathWorkingTimeCSV(id), data, Encoding.UTF8);
            string[] information = { "Pause;Überstunden;Resturlaub", "n;00:00;30"};
            File.WriteAllLines(getWorkingTimeInformationPath(id), information, Encoding.UTF8);

        }
        public static void writeComeInCSV(string id, DateTime checkIn)
        {
            if (MainWindow.timeRounding == 15)
            {
                checkIn = roundUp(checkIn, TimeSpan.FromMinutes(15));
            }
            else if (MainWindow.timeRounding == 5)
            {
                checkIn = roundUp(checkIn, TimeSpan.FromMinutes(5));
            }
            else
            {
                checkIn = roundUp(checkIn, TimeSpan.FromMinutes(1));
            }
            string checkInString = ";" + checkIn.ToString("HH:mm");
            string currentDate = DateTime.Now.ToString("dd.MM.yyyy");
            bool dateFound = false;
            foreach (string line in File.ReadLines(getUserPathWorkingTimeCSV(id)))
            {
                string[] data = line.Split(';');
                string date = data[0];
                if (currentDate == date)
                {
                    dateFound = true;
                    break;
                }
            }
            if(!dateFound)
            {
                //neue Zeille für Datum anlegen
                checkInString= "\r\n" +  currentDate + checkInString;
                string[] timeInformation = File.ReadAllLines(getWorkingTimeInformationPath(id))[1].Split(';');
                //Pause zurücksetzen und Überstunden vorbereiten
                timeInformation[0] = "n";
                string[] overtime = timeInformation[1].Split(':');
                string[] newovertime = new TimeSpan(int.Parse(overtime[0]) - 8, int.Parse(overtime[1]), 0).ToString().Split(':');
                timeInformation[1] = newovertime[0] + ":" + newovertime[1];
                File.WriteAllText(getWorkingTimeInformationPath(id), "Pause;Überstunden;Resturlaub\n" + timeInformation[0] + ";" + timeInformation[1] + ";" + timeInformation[2], Encoding.UTF8);
            }
            File.AppendAllText(getUserPathWorkingTimeCSV(id), checkInString, Encoding.UTF8);
        }
        public static void writeGoInCSV(string id, DateTime checkOut)
        {
            if (MainWindow.timeRounding == 15)
            {
                checkOut = roundDown(checkOut, TimeSpan.FromMinutes(15));
            }
            else if (MainWindow.timeRounding == 5)
            {
                checkOut = roundDown(checkOut, TimeSpan.FromMinutes(5));
            }
            else
            {
                checkOut = roundDown(checkOut, TimeSpan.FromMinutes(1));
            }
            checkOut = new DateTime(checkOut.Year, checkOut.Month, checkOut.Day, checkOut.Hour, checkOut.Minute, 0);
            DateTime today = DateTime.Now;
            string checkOutString = checkOut.ToString("HH:mm");
            string currentDate = today.ToString("dd.MM.yyyy");
            string lastDate = currentDate;
            bool dateFound = false;
            int totalHours = 0;
            int totalMinutes = 0;
            string[] totalWorkingTimeString = { "00", "00" };
            foreach (string line in File.ReadLines(getUserPathWorkingTimeCSV(id)))
            {
                string[] data = line.Split(';');
                string date = data[0];
                if (currentDate == date)
                {
                    dateFound = true;
                    string lastCheckIn = data[data.Length - 1];
                    if (data.Length >= 5)
                    {
                        string[] currentWorkingTimeString = data[data.Length - 2].Split(":");
                        
                        totalHours = int.Parse(currentWorkingTimeString[0]);
                        totalMinutes = int.Parse(currentWorkingTimeString[1]);
                        if (currentWorkingTimeString[0].StartsWith('-'))
                        {
                            totalMinutes *= -1;
                        }
                    }
                    string[] lastCheckInTime = lastCheckIn.Split(':');
                    DateTime lastCheckInDateTime = new DateTime(today.Year, today.Month, today.Day, int.Parse(lastCheckInTime[0]), int.Parse(lastCheckInTime[1]), 0);
                    TimeSpan workingTime = checkOut.Subtract(lastCheckInDateTime);
                    if (workingTime < new TimeSpan(0, 0, 0))
                    {
                        workingTime = new TimeSpan(0, 0, 0);
                    }
                    TimeSpan totalWorkingTime = new TimeSpan(totalHours, totalMinutes, 0).Add(workingTime);
                    string[] timeInformation = File.ReadAllLines(getWorkingTimeInformationPath(id))[1].Split(';');
                    string[] overtime = timeInformation[1].Split(':');
                    int overTimeHours = int.Parse(overtime[0]);
                    int overTimeMinutes = int.Parse(overtime[1]);
                    if (overtime[0].StartsWith('-'))
                    {
                        overTimeMinutes *= -1;
                    }
                    TimeSpan overtimeSpan = new TimeSpan(overTimeHours, overTimeMinutes, 0).Add(workingTime);
                    if (timeInformation[0] == "n" && MainWindow.breakAfterHours != 0 && totalWorkingTime.Hours >= MainWindow.breakAfterHours) // nach fixer Stundenzahl halbe Std abziehen
                    {
                        totalWorkingTime = totalWorkingTime.Subtract(new TimeSpan(0, 45, 0)); // nach fixer Uhrzeit 45 Minuten abziehen
                        overtimeSpan = overtimeSpan.Subtract(new TimeSpan(0, 45, 0)); // nach fixer Uhrzeit 45 Minuten abziehen
                        timeInformation[0] = "j";
                    }
                    else if (timeInformation[0] == "n" && MainWindow.fixBreakTime != null)
                    {
                        string[] breakTimeString = MainWindow.fixBreakTime.Split(":");
                        int breakHours = int.Parse(breakTimeString[0]);
                        int breakMinutes = int.Parse(breakTimeString[1]);
                        DateTime breakTime = new DateTime(today.Year, today.Month, today.Day, breakHours, breakMinutes, 0);
                        if (lastCheckInDateTime < breakTime && checkOut >= breakTime)
                        {
                            totalWorkingTime = totalWorkingTime.Subtract(new TimeSpan(0, 45, 0)); // nach fixer Uhrzeit 45 Minuten abziehen
                            overtimeSpan = overtimeSpan.Subtract(new TimeSpan(0, 45, 0)); // nach fixer Uhrzeit 45 Minuten abziehen
                            timeInformation[0] = "j";
                        }
                    }
                    string[] newovertime = overtimeSpan.ToString().Split(':');
                    if (newovertime[0].Contains('.'))
                    {
                        int fullHours = int.Parse(newovertime[0].Split('.')[0]) * 24;
                        if (newovertime[0].StartsWith('-'))
                        {
                            fullHours -= int.Parse(newovertime[0].Split('.')[1]);
                        }
                        else
                        {
                            fullHours += int.Parse(newovertime[0].Split('.')[1]);
                        }
                        newovertime[0] = fullHours.ToString();
                    }
                    timeInformation[1] = newovertime[0] + ":" + newovertime[1];
                    File.WriteAllText(getWorkingTimeInformationPath(id), "Pause;Überstunden;Resturlaub\n" + timeInformation[0] + ";" + timeInformation[1] + ";" + timeInformation[2], Encoding.UTF8);
                    totalWorkingTimeString = totalWorkingTime.ToString().Split(':');
                }
                lastDate=date;
            }
            if (dateFound)
            {
                string data = ";" + checkOutString + ";" + totalWorkingTimeString[0] + ":" + totalWorkingTimeString[1];
                File.AppendAllText(getUserPathWorkingTimeCSV(id), data, Encoding.UTF8);
            }
            else //Error-Handling
            {
                if(lastDate != currentDate && lastDate != "Datum" && lastDate != "")
                {
                    string[] lastDateDMY = lastDate.Split(".");
                    DateTime lastDateDateTime = new DateTime(int.Parse(lastDateDMY[2]), int.Parse(lastDateDMY[1]), int.Parse(lastDateDMY[0]), 23, 59, 00);
                    writeGoInCSV(id,lastDateDateTime);                   
                }                                                               
                writeComeInCSV(id, new DateTime(today.Year, today.Month, today.Day, 0, 0, 0));
                writeGoInCSV(id, checkOut);
            }

        }
        public static DateTime roundUp(DateTime roundTime, TimeSpan roundingFactor)
        {
            var modTicks = roundTime.Ticks % roundingFactor.Ticks;
            var delta = modTicks != 0 ? roundingFactor.Ticks - modTicks : 0;
            return new DateTime(roundTime.Ticks + delta, roundTime.Kind);
        }

        public static DateTime roundDown(DateTime roundTime, TimeSpan roundingFactor)
        {
            var delta = roundTime.Ticks % roundingFactor.Ticks;
            return new DateTime(roundTime.Ticks - delta, roundTime.Kind);
        }
    }   
}
