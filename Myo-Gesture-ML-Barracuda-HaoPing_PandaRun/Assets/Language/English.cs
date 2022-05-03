// © 2021 ETH Zurich, Daniel G. Woolley

using System.Collections.Generic;

public static class English
{
    public static readonly Dictionary<string, string> dictionary = new Dictionary<string, string>
    {
        // home scene
        { "home_title",                 "<b>EMG</b> Training Platform" },                       // MainPanelStatus.cs
        { "home_familiarisation",       "Familiarisation"},                                     // MainPanelStatus.cs
        { "home_session",               "Session {0}"},                                         // MainPanelStatus.cs
        { "home_structured",            "Structured Training"},                                 // MainPanelStatus.cs
        { "home_unstructured",          "Unstructured Training" },                              // MainPanelStatus.cs
        { "home_status_enable",         "Enable Bluetooth" },                                   // ConnectionStatus.cs
        { "home_status_enabling",       "Enabling Bluetooth" },                                 // ConnectionStatus.cs
        { "home_status_searching",      "Searching for MYO" },                                  // ConnectionStatus.cs
        { "home_status_connecting",     "Connecting to MYO" },                                  // ConnectionStatus.cs
        { "home_status_connected",      "Connected" },                                          // ConnectionStatus.cs
        { "home_status_myo",            "MYO" },                                                // ConnectionStatus.cs
        { "home_status_restart",        "Restart App" },                                        // ConnectionStatus.cs
        { "home_forced_rest",           "Next task available in" },                             // ForcedRest.cs

        // task panels
        { "Training Assessment",        "Training Assessment" },                                // LoadTaskPanel.cs
        { "Flight Training",            "Flight Training" },                                    // LoadTaskPanel.cs
        { "Flight Training 1",          "Flight Training 1" },                                  // LoadTaskPanel.cs 
        { "Flight Training 2",          "Flight Training 2" },                                  // LoadTaskPanel.cs
        { "Flight Training 3",          "Flight Training 3" },                                  // LoadTaskPanel.cs
        { "Flight Training 4",          "Flight Training 4" },                                  // LoadTaskPanel.cs
        { "Flight Training 5",          "Flight Training 5" },                                  // LoadTaskPanel.cs
        { "Obstacle Training",          "Obstacle Training" },                                  // LoadTaskPanel.cs
        { "Pre Assessment",             "Pre Assessment" },                                     // LoadTaskPanel.cs
        { "Post Assessment",            "Post Assessment" },                                    // LoadTaskPanel.cs
        { "Norm Assessment",            "Norm Assessment" },                                    // LoadTaskPanel.cs
        { "signal_test",                "Signal Test" },                                        // SignalTestButton.cs
        { "start_experiment",           "Start Training" },                                     // StartExperimentButton.cs
        { "placement_popup_title",      "Confirm Myo placement" },                              // LoadTaskPanel.cs
        { "placement_popup_message",    "Please ensure that the Myo is on the " +
                                        "{0} forearm before continuing." },                     // LoadTaskPanel.cs
        { "side:",                      "Side: {0}" },                                          // LoadTaskPanel.cs
        { "score:",                     "Score: {0}" },                                         // LoadTaskPanel.cs
        { "flight_training_amount_1",   "Trials: {0}" },                                        // LoadTaskPanel.cs
        { "flight_training_amount_2",   " x {0}s" },                                            // LoadTaskPanel.cs
        { "flight_training_amount_3",   "  (Level {0})" },                                       // LoadTaskPanel.cs

        // create user scene
        { "user_id",                    "Patient ID" },                                         // UserInputField.cs
        { "user_id_error",              "Invalid Patient ID" },                                 // UserInputField.cs
        { "user_id_popup_title",        "Patient ID already in use!" },                         // SaveUser.cs
        { "user_id_popup_message",      "Please enter another Patient ID." },                   // SaveUser.cs
        { "user_side",                  "Training Side" },                                      // SideSegmentedSelection.cs
        { "user_side_error",            "Training side selection required" },                   // SideSegmentedSelection.cs 
        
        // load user scene
        { "load_id_popup_title",        "Please confirm patient data is correct:" },            // LoadUserId.cs

        // progress data scene
        { "no_data",                    "No Data" },                                            // LoadProgressData.cs
        { "experiment_progress",        "Experiment Progress" },                                // ProgressDataTitle.cs
        { "session",                    "Session" },                                            // LoadProgressData.cs
        { "score",                      "Score" },                                              // LoadProgressData.cs
        { "distance",                   "Distance" },                                           // LoadProgressData.cs
        { "time",                       "Time" },                                               // LoadProgressData.cs
        { "duration",                   "Duration" },                                           // LoadProgressData.cs
        { "trials",                     "Trials" },                                             // LoadProgressData.cs
        { "cc_max",                     "CC Max" },                                             // LoadProgressData.cs
        { "emg_range",                  "EMG Range" },                                          // LoadProgressData.cs

        // options scene
        { "options",                    "Options" },                                            // SyncData.cs, OptionsTitle.cs
        { "data_sync_complete",         "Data Sync Complete" },                                 // SyncData.cs
        { "data_sync_partial",          "Partial Data Sync" },                                  // SyncData.cs
        { "data_sync_fail",             "Data Sync Failed" },                                   // SyncData.cs

        // calibration scene
        { "extend",                     "Extend" },                                             // Calibrate.cs
        { "wrist",                      "Wrist" },                                              // Calibrate.cs

        // flight training scene
        { "relax",                      "Relax" },                                              // ExperimentController.cs
        { "level",                      "Level" },                                              // ExperimentController.cs
        { "level_n",                    "Level {0}" },                                          // ExperimentController.cs
        { "start",                      "Start When Ready" },                                   // ExperimentController.cs
        { "finished",                   "Trial Finished" },                                     // ExperimentController.cs

        // buttons
        { "button_load",                "Load" },                                               // LoadUserId.cs, LoadUserButton.cs
        { "button_new",                 "New" },                                                // NewUserButton.cs
        { "button_cancel",              "Cancel" },                                             // LoadTaskPanel.cs, CancelUser.cs, NavigationHome.cs, NavigationRecalibrate.cs
        { "button_save",                "Save" },                                               // SaveUser.cs
        { "button_clear",               "Clear" },                                              // ClearUserButton.cs
        { "button_data",                "Data" },                                               // LoadProgressDataButton.cs
        { "button_more",                "More" },                                               // LoadOptionsButton.cs
        { "button_confirm",             "Confirm" },                                            // LoadUserId.cs
        { "button_sync_data",           "Sync Data" },                                          // SyncData.cs
        { "button_recalibrate",         "Recalibrate" },                                        // NavidationRecalibrate.cs
        { "button_home",                "Home" },                                               // NavigationHome.cs
        { "button_continue",            "Continue" },                                           // LoadTaskPanel.cs

        // navigation popups
        { "nav_popup_title",            "Are you sure?" },                                      // NavigationHome.cs, NavigationRecalibrate.cs
        { "nav_recalibrate_message",    "Perform new calibration and restart current task." },  // NavigationRecalibrate.cs
        { "nav_home_message",           "Return to home screen." },                             // NavigationHome.cs

        // generic
        { "ok",                         "OK" },                                                 // SaveUser.cs
        { "left",                       "Left" },                                               // SideSegmentedSelection.cs
        { "right",                      "Right" },                                              // SideSegmentedSelection.cs
        { "m",                          "m" },                                                  // LoadProgressData.cs
        { "s",                          "s" }                                                   // LoadProgressData.cs  

    };

}
