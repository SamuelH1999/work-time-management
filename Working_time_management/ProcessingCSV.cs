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

        public static int checkLogIn(string userID, string userPWD, bool isLogIn)
        {
            LogInResult inputCorrect = LogInResult.IDNotFound;

            foreach (string line in File.ReadLines(@"..\..\..\data\id_pwd\id_pwd.csv"))
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
            File.WriteAllLines(@"..\..\..\data\worker_information\" + id + @"\worker_information.csv", data); 
        }
        public static void editUserID_PWDToCSV(string id, string newPwd)            //id und newPwd muss aus editUser.xaml.cs übergeben werden
        {
            string[] editPwdID = { id + ";" + newPwd };
            foreach (string line in File.ReadLines(@"..\..\..\data\id_pwd\id_pwd.csv"))
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
        public static void editUserToWorkerInformationCSV(string id, string lastName, string firstName, string DateOfBirth, string residence)
        {
            string[] data = { lastName + ";" + firstName + ";" + DateOfBirth + ";" + residence };
            File.WriteAllLines(@"..\..\..\data\worker_information\" + id + @"\worker_information.csv", data);
        }
        public static string GetWorkerInformation(string id)
        {
            return File.ReadLines(@"..\..\..\data\worker_information\" + id + @"\worker_information.csv").First();
        }
    }
}
