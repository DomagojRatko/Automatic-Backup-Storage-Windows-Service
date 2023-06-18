using System;
using System.ServiceProcess;
using System.Timers;
using System.IO;
using System.Configuration;
using System.Collections.Generic;

namespace BackupStorageWinService
{
    public partial class Service1 : ServiceBase
    {
        // Service files path.
        private string serviceFilesPath;
        // Service config file path.
        private string serviceConfigPath;
        // Time interval.
        private int backupTimeInterval;
        // Path location of files for backup copying.
        private string sourcePath;
        // Path location of storage for backup pasting.
        private string targetPath;

        private string sourceFileName;
        private string targetFileName;
        // Backup number count in time frame of service running started and end.
        private int backupCount;
        private Timer timer;
        private StreamReader readerObject;

        public Service1()
        {
            InitializeComponent();

            serviceFilesPath = ConfigurationManager.AppSettings["SERVICE_FILES_PATH"];
            serviceConfigPath = ConfigurationManager.AppSettings["SERVICE_CONFIG_PATH"];

            timer = new Timer();
            sourceFileName = string.Empty;
            targetFileName = string.Empty;
            backupCount = 0;
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("[OK] Service is started at " + DateTime.Now);
            readConfigFile();
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = backupTimeInterval;
            timer.Enabled = true;

            // On service starts running we going to skip
            // BACKUP_TIME_INTERVAL wait time to do first backup.
            if (backupCount == 0) 
            {
                FirstBackup();
            }
        }

        protected override void OnStop()
        {
            WriteToFile("[OK] Service is stoped at " + DateTime.Now);
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e) 
        {
            WriteToFile("[OK] Backup started at " + DateTime.Now);
            BackupFiles();
        }

        private void FirstBackup() 
        {
            WriteToFile("[OK] First backup on service running started at " + DateTime.Now);
            BackupFiles();
        }

        private void BackupFiles() 
        {
            if (Directory.Exists(sourcePath) && Directory.Exists(targetPath))
            {
                string[] files = Directory.GetFiles(sourcePath);

                // Copy the files and overwrite destination files if they already exist. 
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    sourceFileName = Path.GetFileName(s);
                    targetFileName = Path.Combine(targetPath, sourceFileName);
                    // Compare source and target files hash and skips copy if they are the same.
                    if (File.Exists(targetFileName))
                    {
                        File.Copy(s, targetFileName, true);
                        // Log copied file target path with target file name
                        WriteToFile("[OK][UPDATE] File source name: " + sourceFileName + ", copy to " + targetFileName);
                    }
                    else
                    {
                        File.Copy(s, targetFileName, true);
                        // Log first copied file target path with target file name
                        WriteToFile("[OK][COPY] First copy of file source name: " + sourceFileName + ", copy to " + targetFileName);
                    }
                }
                // Log end of backup with date and backup number.
                WriteToFile("[OK] Backup ended at " + DateTime.Now + ", Backup number: " + ++backupCount);
            }
            else
            {
                if (!Directory.Exists(sourcePath) && !Directory.Exists(targetPath))
                {
                    // Log source and target directory not found case.
                    WriteToFile("[ERROR] Source " + sourcePath + " and Target " + targetPath + " directory path could not be found!");
                }
                else if (!Directory.Exists(sourcePath))
                {
                    // Log source directory not found case.
                    WriteToFile("[ERROR] Source directory path " + sourcePath + " could not be found!");
                }
                else if (!Directory.Exists(targetPath))
                {
                    // Log target directory not found case.
                    WriteToFile("[ERROR] Target directory path " + targetPath + " could not be found!");                 
                }
                WriteToFile("[ERROR] Backup faild at " + DateTime.Now + ", Backup number: " + backupCount);
            }
        }

        private void WriteToFile(string Message) 
        {
            // Log folder path and name
            string path = serviceFilesPath + "\\Logs";
            if (!Directory.Exists(path)) 
            {
                Directory.CreateDirectory(path);
            }
            // Log file path and name
            string filepath = serviceFilesPath + "\\Logs\\BackupLogs.txt";
            if (!File.Exists(filepath))
            {
                // Create new log file and write message.
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                // Write message to file.
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }

        private void readConfigFile()
        {
            // Create directory if not exists.
            if (!Directory.Exists(serviceFilesPath))
            {
                Directory.CreateDirectory(serviceFilesPath);
                WriteToFile("[OK] Service directory folder created at " + DateTime.Now + " in location " + serviceFilesPath);
            }
            // Create directory files if not exists.
            if (!File.Exists(serviceConfigPath))
            {
                // Create new config file and write default settings.
                using (StreamWriter sw = File.CreateText(serviceConfigPath))
                {
                    sw.WriteLine("timeinterval=10000");
                    sw.WriteLine("sourcefolder=C:\\source_folder_not_set");
                    sw.WriteLine("targetfolder=C:\\source_folder_not_set");
                }
                WriteToFile("[OK] Service default files created at " + DateTime.Now + " in location " + serviceConfigPath);
            }
            // Read config file and set service settings.
            List<string> list = new List<string>();
            using (readerObject = new StreamReader(serviceConfigPath))
            {
                string line;
                while ((line = readerObject.ReadLine()) != null)
                {
                    line = line.Substring(13);
                    list.Add(line);
                }
                backupTimeInterval = int.Parse(list[0]);
                sourcePath = list[1];
                targetPath = list[2];
            }
            WriteToFile("[OK] Service settings set at " + DateTime.Now);
            WriteToFile("[OK] Service Backup time interval: " + backupTimeInterval);
            WriteToFile("[OK] Service source path: " + sourcePath);
            WriteToFile("[OK] Service target path: " + targetPath);
        }
    }
}
