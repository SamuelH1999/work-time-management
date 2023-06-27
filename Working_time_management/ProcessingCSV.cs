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

        private static string[] readPwdIDCSV()
        {
            string[] pwdIdCSV = File.ReadAllLines(@"..\..\..\data\id_pwd\id_pwd.csv");
            return pwdIdCSV;
        }
        public static int checkLogIn(string userID, string userPWD, bool isLogIn)
        {
            string[] CSV = readPwdIDCSV();

            LogInResult inputCorrect = LogInResult.IDNotFound;

            foreach (string line in CSV)
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
            File.AppendAllLines(@"..\..\..\data\id_pwd\id_pwd.csv", pwdID, Encoding.UTF8);
        }
        public static void addUserToWorkerInformationCSV(string id, string lastName, string firstName, string DateOfBirth, string residence)
        {
            string[] data = { lastName + ";" + firstName + ";" + DateOfBirth + ";" + residence };
            File.WriteAllLines(@"..\..\..\data\worker_information\" + id + @"\worker_information.csv", data); // Pfad muss geändert werden; Außerdem müssen neue Ordner erstellt werden
        }
        public static void editUserToID_PWDCSV(string id, string newPwd)            //id und newPwd muss aus editUser.xaml.cs übergeben werden
        {
            string[] editPwdID = { id + ";" + newPwd };
            string[] CSV = readPwdIDCSV();
            foreach (string line in CSV)
            {
                string[] data = line.Split(';');
                string ID = data[0];
                string pwd = data[1];
                if (id == ID)
                {
                    File.WriteAllLines(@"..\..\..\data\id_pwd\id_pwd.csv", editPwdID, Encoding.UTF8);
                }
            }
            
        }
        public static string GetWorkerInformation(string id)
        {
            return File.ReadLines(@"..\..\..\data\worker_information\" + id + @"\worker_information.csv").First();
        }

    }
}
