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
            return @"..\..\..\data\worker_information\" + id + "/" + id + "_working_time.csv";
        }

        public static int checkLogIn(string userID, string userPWD, bool isLogIn)
        {
            LogInResult inputCorrect = LogInResult.IDNotFound;

            foreach (string line in File.ReadLines(idPwdPath))
            {
                string[] data = line.Split(';');
                string ID = data[0];
                string pwd = data[1];
                if (ID == userID)
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
        public static bool addAbsenceToAbsenceCSV(string ID, string startDate, string endDate)
        {
            bool IDFound = false;
            foreach (string line in File.ReadLines(idPwdPath))
            {
                string[] data = line.Split(';');
                string id = data[0];
                if (id == ID)
                {
                    IDFound = true;
                    string[] textForCSV = { ID + ';' + startDate + ';' + endDate + ';' +  "Krankheit" };
                    File.AppendAllLines(@"..\..\..\data\admin\absences.csv", textForCSV, Encoding.UTF8);
                    break;
                }
            }
            return IDFound;
        }
        public static void addUserToWorkerInformationCSV(string id, string lastName, string firstName, string DateOfBirth, string residence) 
        {
            string[] data = { lastName + ";" + firstName + ";" + DateOfBirth + ";" + residence + ";" + "Abgemeldet" };
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
                if (ID == id)
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
            string workerInformationPath = getWorkerInformationPath(id);
            return File.ReadLines(workerInformationPath).First();
        }
        public static void deleteUserPwdInCSV(string id)            // id und newPwd muss aus editUser.xaml.cs übergeben werden
        {                                                                       
            string[] allLines = File.ReadAllLines(idPwdPath);       // gesamte CSV auslesen
            string newCSV = "";                                              
            for (int i = 0; i < allLines.Length; i++)                
            {                                                                                                                     
                string[] data = allLines[i].Split(';');                               
                string ID = data[0];
                string pwd = data[1];
                if (ID != id)                                       // wenn ID ungleich der aktuellen id ist, wird die ID und das Passwort in newCSV geschrieben
                {
                    newCSV += allLines[i] + "\n";
                }                                                   // wenn ID gleich der aktuellen ID ist, wird die ID und das Passwort einfach weggelassen und somit gelöscht
            }
            File.WriteAllText(idPwdPath, newCSV, Encoding.UTF8);    // alte CSV-Datei überschreieben -> Wert der weggelassen wurde nun nicht mehr enthalten
        }
        public static void addWorkingTimeCSV(string id)
        {
            string[] data = { "Datum;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit" +
                    ";Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit" };
            File.WriteAllLines(getUserPathWorkingTimeCSV(id), data);
        }
        public static void writeComeInCSV(string id, DateTime checkIn)
        {
            string checkInString = checkIn.ToString("HH:mm");
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
            if(dateFound)
            {
                string data =  ";" + checkInString;
                File.AppendAllText(getUserPathWorkingTimeCSV(id), data, Encoding.UTF8);
            }
            else 
            {
                string data = "\r\n" +  currentDate + ";" + checkInString;
                File.AppendAllText(getUserPathWorkingTimeCSV(id), data, Encoding.UTF8);
            }
            
        }
        public static void writeGoInCSV(string id, DateTime checkOut)
        {
            DateTime today = DateTime.Now;
            string checkOutString = checkOut.ToString("HH:mm");
            string currentDate = today.ToString("dd.MM.yyyy");
            string lastDate = currentDate;
            bool dateFound = false;
            string workingTimeString ="00:00";
            int totalHours = 0;
            int totalMinutes = 0;
            string totalWorkingTimeString = "00:00";
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
                        string currentWorkingTimeString = data[data.Length - 2];
                        totalHours = int.Parse(currentWorkingTimeString.Split(":")[0]);
                        totalMinutes = int.Parse(currentWorkingTimeString.Split(":")[1]);
                    }
                    string[] timeInformation = lastCheckIn.Split(':');
                    int hours = int.Parse(timeInformation[0]);
                    int minutes = int.Parse(timeInformation[1]);
                    DateTime lastCHeckInDateTime = new DateTime(today.Year, today.Month, today.Day, hours, minutes, 0);
                    TimeSpan workingTime = checkOut.Subtract(lastCHeckInDateTime);
                    workingTimeString = workingTime.ToString();
                    TimeSpan totalWorkingTime = new TimeSpan(totalHours, totalMinutes, 0).Add(workingTime);
                    totalWorkingTimeString = totalWorkingTime.ToString();
                }
                lastDate=date;
            }
            if (dateFound)
            {
                string data = ";" + checkOutString + ";" + totalWorkingTimeString.Split(":")[0] + ":" + totalWorkingTimeString.Split(":")[1];
                File.AppendAllText(getUserPathWorkingTimeCSV(id), data, Encoding.UTF8);
            }
            else //Error-Handling
            {
                if(lastDate != currentDate && lastDate != "Datum" && lastDate != "")
                {
                    int lastDateYear;
                    int lastDateMonth;
                    int lastDateDay;
                    lastDateYear = int.Parse(lastDate.Split('.')[2]);
                    lastDateMonth = int.Parse(lastDate.Split('.')[1]);
                    lastDateDay = int.Parse(lastDate.Split('.')[0]);
                    DateTime lastDateDateTime = new DateTime(lastDateYear, lastDateMonth, lastDateDay, 23, 59, 00);
                    writeGoInCSV(id,lastDateDateTime);                   
                }                                                               
                writeComeInCSV(id, new DateTime(today.Year, today.Month, today.Day, 0, 0, 0));
                writeGoInCSV(id, checkOut);
            }

        }
    }   
}
