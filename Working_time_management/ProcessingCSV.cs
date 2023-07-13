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
using System.Windows.Media.Media3D;

namespace Working_time_management
{
    public static class ProcessingCSV
    {
        public enum LogInResult                 // Ergebnis der Funktion checkLogIn()
        {
            IDNotFound,
            PwdIncorrect,
            UserCorrect,
            AdminCorrect,
            TimeDetectionIdFound,
            TimeDetectionIdNotFound,
        }
        public static readonly string idPwdPath = @"..\..\..\data\id_pwd\id_pwd.csv";           // Die Ordnerpfade werden definiert in den folgenden Funktionen und Variablen

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

        public static int checkLogIn(string userID, string userPWD, bool isLogIn)                       // ID und Passwort wird mit der Datenbank abgelichen
        {
            LogInResult inputCorrect = LogInResult.IDNotFound;

            foreach (string line in File.ReadLines(idPwdPath))                  // Die IDs und Passwörter werden aus der CSV-Datei geholt
            {
                string[] data = line.Split(';');
                string ID = data[0];
                string pwd = data[1];
                if (ID == userID && ID != "ID")                         // Die Überschrift "ID" wird nicht berücksichtigt
                {
                    if (isLogIn == false)                       //Wenn der Benutzer sich nicht anmelden will, sondern nur in das Zeiterfassungsmenü möchte
                    {
                        inputCorrect = LogInResult.TimeDetectionIdFound;

                        break;
                    }
                    else                                                //Passwort wird überprüft
                    { 
                        if (string.Compare(pwd, userPWD) == 0)
                        {
                            if (string.Compare(userID, "123123") == 0)      //ID des Admins ist "123123"
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
        public static void addUserToID_PWDCSV(string[] pwdID)       //Neue Zeile mit den neuen Zugangsdaten wird angehängt
        {
            File.AppendAllLines(idPwdPath, pwdID, Encoding.UTF8);
        }
        public static bool addAbsenceToAbsenceCSV(string ID, string startDate, string endDate, string reason)       //Abwesenheit wird in CSV-Datei eingetragen
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
        public static void addUserToWorkerInformationCSV(string id, string lastName, string firstName, string DateOfBirth, string residence)  //Die Mitarbeiterinformationen werden in eine neu erstellte CSV-Datei eingetragen. Außerdem wird auch die Datei "Requests" erstellt und die Überschriften werden eingefügt
        {
            string[] data = { "Nachname" + ";" + "Vorname" + ";" + "Geburtsdatum" + ";" + "Wohnort" + ";" + "Status" + "\n" + lastName + ";" + firstName + ";" + DateOfBirth + ";" + residence + ";" + "Abgemeldet" };
            string[] header = { "Abwesenheitsgrund" + ";" + "Von" + ";" + "Bis" + ";" + "Status" };
            string workerInformationPath = getWorkerInformationPath(id);
            string userRequestPath = getUserRequestPath(id);
            File.WriteAllLines(workerInformationPath, data);
            File.WriteAllLines(userRequestPath, header);
        }
        public static void editUserPwdToCSV(string id, string newPwd)           // id und newPwd muss aus editUser.xaml.cs übergeben werden
        {                                                                       //Gesamte CSV im Array "allLines" zwischenspeichern. ID in "allLines" finden, alte ID und neues Passwort in die ausgewählte Zeile schreiben und dann die gesamte CSV mit den Zwischegespeicherten Daten überschreiben
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
        public static void editUserToWorkerInformationCSV(string id, string lastName, string firstName, string DateOfBirth, string residence, string status)    //Datensatz aus worker_information.csv holen, bearbeitete Spalten einfügen und die alte CSV mit den neuen Daten überschreiben
        {
            string[] data = { "Nachname" + ";" + "Vorname" + ";" + "Geburtsdatum" + ";" + "Wohnort" + ";" + "Status" + "\n" + lastName + ";" + firstName + ";" + DateOfBirth + ";" + residence + ";" + status};
            string workerInformationPath = getWorkerInformationPath(id);
            File.WriteAllLines(workerInformationPath, data);
        }
        public static string GetWorkerInformation(string id)        //Die Mitarbeiterinformationen aus der CSV holen
        {
            string workerInformation = "";
            string workerInformationPath = getWorkerInformationPath(id);
            foreach (string line in File.ReadAllLines(workerInformationPath))
            {
                string[] data = line.Split(';');
                string lastName = data[0];
                if (lastName != "Nachname")     //Die überschrift soll nicht gelesen werden
                {
                    workerInformation = line;
                }
            }
            return workerInformation; 
        }

        public static string[] getAllWorkerRequests(string id)          //Gibt alle Anträge dieses Mitarbiters zurück
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
        public static void addWorkingTimeCSV(string id)         //CSV für die Arbeitszeiterfassung wird erstellt
        {
            string data =  "Datum;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit" +
                    ";Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit;Kommen;Gehen;Arbeitszeit" ; // Der Mitarbeiter soll sich unbegrenzt oft anmelden können, kann er auch, irgendwann hören nur die Überschriften auf
            File.WriteAllText(getUserPathWorkingTimeCSV(id), data, Encoding.UTF8);
            string[] information = { "Pause;Überstunden;Resturlaub", "n;00:00;30"};
            File.WriteAllLines(getWorkingTimeInformationPath(id), information, Encoding.UTF8);          //CSV zur Eintragung der Überstunden, der Urlaubstage und, ob eine Pause genommen wurde, wird erstellt

        }
        public static void writeComeInCSV(string id, DateTime checkIn)      // Verarbeitet einstempeln des Mitarbeiters
        {
            if (MainWindow.timeRounding == 15)                  // Aufrundung der Arbeitszeit, je nachdem, ob die Rundung nach 1, 5 oder 15 Minuten eingestellt ist
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
            string checkInString = ";" + checkIn.ToString("HH:mm");     // Eingangsuhrzeit formartieren als Uhrzeit und für CSV
            string currentDate = checkIn.ToString("dd.MM.yyyy");   // Eingangsdatum speichern
            bool dateFound = false;
            foreach (string line in File.ReadLines(getUserPathWorkingTimeCSV(id)))      // Suchen, ob aktuelles Datum schon in CSV existiert
            {
                string[] data = line.Split(';');
                string date = data[0];
                if (currentDate == date)
                {
                    dateFound = true;       // Wenn Datum vorhanden: Flag setzen
                    break;
                }
            }
            if(!dateFound)                  // Wenn Datum nicht vorhanden: Neue Zeile für aktuellen Tag in CSV anlegen
            {
                checkInString= "\r\n" +  currentDate + checkInString;
                string[] timeInformation = File.ReadAllLines(getWorkingTimeInformationPath(id))[1].Split(';');
                timeInformation[0] = "n";       // Neuer Tag --> Pause zurücksetzen
                string[] overtime = timeInformation[1].Split(':');
                int overTimeHoursCome = 0;
                if (overtime[0].Contains('.'))              // Wenn "overtime" die Zeit als TimeSpan mit Tagen und nicht mehr in Stunden gespeichert hat, wird diese hier wieder in Stunden umgewandelt
                {
                    overTimeHoursCome = int.Parse(overtime[0].Split('.')[0]) * 24 + int.Parse(overtime[0].Split('.')[1]);
                }
                else
                {
                    overTimeHoursCome = int.Parse(overtime[0]);
                }
                timeInformation[1] = overTimeHoursCome - 8 + ":" + int.Parse(overtime[1]);     // Neuer Tag --> Einmnalig 8 Std abziehen, die im Laufe des Tages wieder gearbeitet werden müssen
                File.WriteAllText(getWorkingTimeInformationPath(id), "Pause;Überstunden;Resturlaub\n" + timeInformation[0] + ";" + timeInformation[1] + ";" + timeInformation[2], Encoding.UTF8);
            }
            File.AppendAllText(getUserPathWorkingTimeCSV(id), checkInString, Encoding.UTF8); // Wenn Tag schon vorhanden: nur CheckIn Uhrzeit in CSV anhängen, ohne neue Zeile
        }
        public static void writeGoInCSV(string id, DateTime checkOut)   // Verarbeitet einstempeln des Mitarbeiters
        {
            if (MainWindow.timeRounding == 15)                      //Abrundung der Arbeitszeit, je nachedem, ob 1, 5 oder 15 Minuten eingestellt wurden
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
            foreach (string line in File.ReadLines(getUserPathWorkingTimeCSV(id)))      // Auslesen der Arbeitszeiten
            {
                string[] data = line.Split(';');
                string date = data[0];
                if (currentDate == date)                                                // Datum abgleichen          
                {
                    dateFound = true;                                                   // Flag setzen
                    string lastCheckIn = data[data.Length - 1];                         // Letzten Eintrag einlesen
                    if (data.Length >= 5)
                    {
                        string[] currentWorkingTimeString = data[data.Length - 2].Split(":");
                        
                        totalHours = int.Parse(currentWorkingTimeString[0]);            // Tagesgesamtzeit auslesen
                        totalMinutes = int.Parse(currentWorkingTimeString[1]);
                        if (currentWorkingTimeString[0].StartsWith('-'))                // TimeSpan negativ? --> Minuten negieren für TimeSpan Konstruktor
                        {
                            totalMinutes *= -1;
                        }
                    }
                    string[] lastCheckInTime = lastCheckIn.Split(':');              
                    DateTime lastCheckInDateTime = new DateTime(today.Year, today.Month, today.Day, int.Parse(lastCheckInTime[0]), int.Parse(lastCheckInTime[1]), 0);   // Letztes Einstempeln als DateTime speichern
                    TimeSpan workingTime = checkOut.Subtract(lastCheckInDateTime);      // Arbeitszeit berechnen
                    if (workingTime < new TimeSpan(0, 0, 0))                            
                    {
                        workingTime = new TimeSpan(0, 0, 0);                            // negtiver Zeitabschnitt nullen (komnmt durch die Rundung)
                    }
                    TimeSpan totalWorkingTime = new TimeSpan(totalHours, totalMinutes, 0).Add(workingTime);         // Tagesgesamtzeit  mit Zeitabschnitt addiern
                    string[] timeInformation = File.ReadAllLines(getWorkingTimeInformationPath(id))[1].Split(';');  // Zeitinfrmationen auslesen
                    string[] overtime = timeInformation[1].Split(':');                                              // Überstunden auslesen
                    int overTimeHours = 0;
                    int overTimeMinutes = int.Parse(overtime[1]);
                    if (overtime[0].Contains('.')) //Wenn "overtime" die Zeit in Tagen und nicht mehr in Stunden gespeichert hat, wird diese hier wieder in Stunden umgewandelt
                    {
                        overTimeHours = int.Parse(overtime[0].Split('.')[0]) * 24;
                        if (overtime[0].StartsWith('-'))
                        {
                            overTimeHours -= int.Parse(overtime[0].Split('.')[0]);
                            overTimeMinutes *= -1;      // Überstunden negativ? --> Minuten negieren für TimeSpan Konstruktor
                        }
                        else
                        {
                            overTimeHours += int.Parse(overtime[0].Split('.')[0]);
                        }
                    }
                    else
                    {
                        overTimeHours = int.Parse(overtime[0]); 
                        if (overtime[0].StartsWith('-'))
                        {
                            overTimeMinutes *= -1;      // Überstunden negativ? --> Minuten negieren für TimeSpan Konstruktor
                        }
                    }
                    TimeSpan overtimeSpan = new TimeSpan(overTimeHours, overTimeMinutes, 0).Add(workingTime);   // Zeitabschnitt auf Überstunden-Konto addieren
                    if (timeInformation[0] == "n" && MainWindow.breakAfterHours != 0 && totalWorkingTime.Hours >= MainWindow.breakAfterHours) // nach fixer Stundenzahl 45 Min abziehen
                    {
                        totalWorkingTime = totalWorkingTime.Subtract(new TimeSpan(0, 45, 0)); // Gesamtzeit 45 Minuten abziehen
                        overtimeSpan = overtimeSpan.Subtract(new TimeSpan(0, 45, 0)); // Überstunden 45 Minuten abziehen
                        timeInformation[0] = "j";                                     // Pause für den Tag vermerken
                    }
                    else if (timeInformation[0] == "n" && MainWindow.fixBreakTime != null)
                    {
                        string[] breakTimeString = MainWindow.fixBreakTime.Split(":");
                        int breakHours = int.Parse(breakTimeString[0]);
                        int breakMinutes = int.Parse(breakTimeString[1]);
                        DateTime breakTime = new DateTime(today.Year, today.Month, today.Day, breakHours, breakMinutes, 0);
                        if (lastCheckInDateTime < breakTime && checkOut >= breakTime)   // Prüfung, ob Anmeldezeitraum über Pausenzeit ging
                        {
                            totalWorkingTime = totalWorkingTime.Subtract(new TimeSpan(0, 45, 0)); // Gesamtzeit 45 Minuten abziehen
                            overtimeSpan = overtimeSpan.Subtract(new TimeSpan(0, 45, 0)); // Überstunden 45 Minuten abziehen
                            timeInformation[0] = "j";   // Pause für den Tag vermerken
                        }
                    }
                    // nochmal Überstunden auf Negatives Vorzeichen und größer 1 Tag prüfen
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
                    timeInformation[1] = newovertime[0] + ":" + newovertime[1];  // Überstunden zu String
                    File.WriteAllText(getWorkingTimeInformationPath(id), "Pause;Überstunden;Resturlaub\n" + timeInformation[0] + ";" + timeInformation[1] + ";" + timeInformation[2], Encoding.UTF8);   // Überstunden abspeichern
                    totalWorkingTimeString = totalWorkingTime.ToString().Split(':');
                }
                lastDate=date;
            }
            if (dateFound)      // Flag gesetzt? 
            {
                string data = ";" + checkOutString + ";" + totalWorkingTimeString[0] + ":" + totalWorkingTimeString[1];     // Daten für CSV formatieren
                File.AppendAllText(getUserPathWorkingTimeCSV(id), data, Encoding.UTF8);                                     // Daten an CSV anhängen, ohne neue Zeile
            }
            else //Error-Handling und Tageswechsel
            {
                if(lastDate != currentDate && lastDate != "Datum" && lastDate != "")        // Prüfen, ob das letzte Einstempeln sinnvoll eingelesen wurde
                {
                    string[] lastDateDMY = lastDate.Split(".");                             // letztes Anmeldedatum auslesen
                    DateTime lastDateDateTime = new DateTime(int.Parse(lastDateDMY[2]), int.Parse(lastDateDMY[1]), int.Parse(lastDateDMY[0]), 23, 59, 00);  // DateTime erstellen von diesem mTag um 23:59
                    writeGoInCSV(id,lastDateDateTime);                                      // Checkout zu diesem Zeitpunkt --> Tageswechsel
                }                                                               
                writeComeInCSV(id, new DateTime(today.Year, today.Month, today.Day, 0, 0, 0));  //CheckIn am aktuellen Tag um 00:00 nachholen
                writeGoInCSV(id, checkOut);                                                     //CheckOut nochmal durchführen 
            }

        }
        public static DateTime roundUp(DateTime roundTime, TimeSpan roundingFactor)     //Aufrundung der Arbeitszeit
        {
            var modTicks = roundTime.Ticks % roundingFactor.Ticks;
            var delta = modTicks != 0 ? roundingFactor.Ticks - modTicks : 0;
            return new DateTime(roundTime.Ticks + delta, roundTime.Kind);
        }

        public static DateTime roundDown(DateTime roundTime, TimeSpan roundingFactor)       //Abrundung der Arbeitszeit
        {
            var delta = roundTime.Ticks % roundingFactor.Ticks;
            return new DateTime(roundTime.Ticks - delta, roundTime.Kind);
        }
    }   
}
