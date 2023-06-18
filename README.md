# Automatic Backup Storage Windows Service

## What is this?
Automatic backup storage Windows service is a Windows service that runs automatically with a custom-set interval time.
That means copying all files from the source directory to the target directory.
This is used so that files could be automatically backed up with a custom-set interval time.

An example of this Windows service could be using it with Dropbox storage. 
We can set the source directory to contain some files that often require modification and need backup.
After every custom time interval, source files will be copied and replaced in the target directory.
Target directory can be a Dropbox folder that will automatically detect file changes, a backup will be made, 
and new and modified files will be stored in cloud storage.

Important to mention is that on every service run, Backup will be performed before the time interval starts.
That would mean that every time Windows boots up, a backup will be made on service start.

## How to setup and use.
* Download service file **[BackupStorageWinService.exe](https://github.com/DomagojRatko/Automatic-Backup-Storage-Windows-Service/blob/main/BackupStorageWinService.exe)**.

* Open the Command Prompt as **Administrator!**.

* Go locate in your Command Prompt directory, for example "C:\Windows\Microsoft.NET\Framework\v4.0.30319>" (Choose your last version; mine is v4.0.30319)

* Run the command in the Command Prompt  `InstallUtil.exe PATH TO DOWNLOAD BackupStorageWinService.exe FILE` 
- Example: `InstallUtil.exe C:\Downloads\BackupStorageWinService.exe`

* After successfully installing the service, you can check to see if it is running in your Windows Service window.
- Service name: Backup Storage Win Service

* Open File Explorer and go to the directory `C:\BackupStorageService`

* Open the config.txt file, set custom values, and save the file.
- Example `config.txt` file.
- **Attention! The time interval should not be less than 1000 and more than 2000000000**
- **1000 = 1 second**
- **60000 = 1 minute**
- **3600000 = 1 hour**
- **86400000 = 24 hours / 1 day**
```
timeinterval=3600000
sourcefolder=C:\Backup\PersonalStorageFiles
targetfolder=C:\User\Documents\Dropbox\upload
```

* **Important! Restart the service after every config file modification.**
![1](https://github.com/DomagojRatko/Automatic-Backup-Storage-Windows-Service/assets/62218857/3cc1c43a-fe13-4935-adc5-45e9fa614573)
* Check your log file for any error messages and new updates to see if everything works correctly.
- Example of an OK log file:
```
  [OK] Service is started at 18/06/2023 16:29:54
  [OK] Service default files created at 18/06/2023 16:29:54 in location C:\BackupStorageService\config.txt
  [OK] Service settings set at 18/06/2023 16:29:54
  [OK] Service Backup time interval: 10000
  [OK] Service source path: C:\source_folder_not_set
  [OK] Service target path: C:\source_folder_not_set
  [OK] First backup on service running started at 18/06/2023 16:29:54
  [ERROR] Source C:\source_folder_not_set and Target C:\source_folder_not_set directory path could not be found!
  [ERROR] Backup faild at 18/06/2023 16:29:54, Backup number: 0
  [OK] Backup started at 18/06/2023 16:30:04
  [ERROR] Source C:\source_folder_not_set and Target C:\source_folder_not_set directory path could not be found!
  [ERROR] Backup faild at 18/06/2023 16:30:04, Backup number: 0
  [OK] Service is stoped at 18/06/2023 16:32:19
  [OK] Service is started at 18/06/2023 16:32:21
  [OK] Service settings set at 18/06/2023 16:32:21
  [OK] Service Backup time interval: 10000
  [OK] Service source path: C:\one
  [OK] Service target path: C:\otwo
  [OK] First backup on service running started at 18/06/2023 16:32:21
  [OK][COPY] First copy of file source name: test.txt, copy to C:\otwo\test.txt
  [OK] Backup ended at 18/06/2023 16:32:21, Backup number: 1
  [OK] Backup started at 18/06/2023 16:32:31
  [OK][UPDATE] File source name: test.txt, copy to C:\otwo\test.txt
  [OK] Backup ended at 18/06/2023 16:32:31, Backup number: 2  
```
* It's recommended that if you plan to backup large file sizes, you increase the time interval to give enough time for files to copy and not put too much work on disk.
![2](https://github.com/DomagojRatko/Automatic-Backup-Storage-Windows-Service/assets/62218857/2aafdf61-4f17-48e6-92c0-18969443244a)

** How to uninstall service.
* Run the command in the Command Prompt  `InstallUtil.exe -u PATH TO DOWNLOAD BackupStorageWinService.exe FILE` 
- Example: `InstallUtil.exe C:\Downloads\BackupStorageWinService.exe`

** If you have any problems or suggestions, feel free to contact me.
