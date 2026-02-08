# Db Backup
The following describes how to take a backup of the data in the application.  Note that when you close Visual Studio the development containers
are deleted and with them all the data. When this occurs you may want to take snapshots of the data to be used in the future.  Here's how you do that.

# Creating the Backup
1. Open up a **WSL** command prompt.
1. Run the following the following commands to build the **mongo-tools:latest** container with the mongo tools to take the backup.

```
cd Db/MongoTools
./lbuild.sh
```
3. Run the command:

```
./lbackup.sh
```

4. The backup will be timestamped with the value **YYYYMMDD_HHMMSS** and can be found under **Output/Db/backup**