using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using System.Diagnostics.Eventing.Reader;
using System.Security.Policy;

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
        public static void addUserToWorkerInformationCSV(string id, string lastName, string firstName, string DateOfBirth, string residence) 
        {
            string[] data = { lastName + ";" + firstName + ";" + DateOfBirth + ";" + residence };
            string workerInformationPath = getWorkerInformationPath(id);
            string userRequestPath = getUserRequestPath(id);
            File.WriteAllLines(workerInformationPath, data);
            File.Create(userRequestPath);
        }
        public static void editUserPwdToCSV(string id, string newPwd)           // id und newPwd muss aus editUser.xaml.cs übergeben werden
        {                                                                       // schwierig umzustezen, da ich nicht gleichzeitig auf csv-Datei zugreifen kann
            int lines = 0;                                                      // Lösung 1: Passwort in workerInformation speichern
            foreach (string line in File.ReadLines(idPwdPath))                  // Lösung 2: neue Liste anlegen, bearbeiten und dann Datei überschreiben
            {                                                                   // Wenn Passwort in worker_information ist, kann der Mitarbeiter, wenn dieser gelöscht wurde, 
                lines ++;                                                       // sich auch nicht mehr anmelden. Id ist zwar noch da, aber die wäre tot -> 
                string[] data = line.Split(';');                                // wir vergeben ja eh nur aufsteigende Ids, also nicht schlimm oder?
                string ID = data[0];
                string pwd = data[1];
                if (ID == id)
                {      
                    //File.WriteAllLines(idPwdPath,editPwdID, Encoding.UTF8)
                    break;
                }
            }

            string[] editPwdID = { id + ";" + newPwd };
            ;
        }
        public static void editUserToWorkerInformationCSV(string id, string lastName, string firstName, string DateOfBirth, string residence)
        {
            string[] data = { lastName + ";" + firstName + ";" + DateOfBirth + ";" + residence };
            string workerInformationPath = getWorkerInformationPath(id);
            File.WriteAllLines(workerInformationPath, data);
        }
        public static string GetWorkerInformation(string id)
        {
            string workerInformationPath = getWorkerInformationPath(id);
            return File.ReadLines(workerInformationPath).First();
        }
    }
}
