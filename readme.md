### What does it do?
1. removes "please register" pop-up window of SourceGear DiffMerge tool by updating it's "timer" so it never triggers
2. resets trial time of Beyond Compare tool

### How to use
1. build
2. install as service
3. configure service to run as your user (important because service operate on HKEY_CURRENT_USER registry hive)

### WIP
- Remove neccessity of runnig as your user by adding automatic search of registry keys throuht all users