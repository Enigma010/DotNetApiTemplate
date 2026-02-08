# Db Restore
The following describes how to restore the data in the application.

# Creating the Backup
1. Open up a **WSL** command prompt.
1. Run the following the following commands to build the **mongo-tools:latest** container with the mongo tools to take the backup.

```
cd Db/MongoTools
./lbuild.sh
```
3. Run the command, where `<restoreName>` is the name of the of the directory under **Output/Db/backup** to restore

```
./lrestore.sh <restoreName>
```