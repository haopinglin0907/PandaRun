// © 2021 ETH Zurich, Daniel G. Woolley, Xue Zhang

using System.Collections.Generic;

public static class Chinese
{
    public static readonly Dictionary<string, string> dictionary = new Dictionary<string, string>
    {
        // home scene
        { "home_title",                 "<b>肌电信号训练平台</b>" },                             // MainPanelStatus.cs
        { "home_familiarisation",       "热身训练"},                                            // MainPanelStatus.cs
        { "home_session",               "训练日 {0}"},                                         // MainPanelStatus.cs
        { "home_structured",            "常规训练"},                                            // MainPanelStatus.cs
        { "home_unstructured",          "自由训练" },                                           // MainPanelStatus.cs
        { "home_status_enable",         "请连接蓝牙" },                                          // ConnectionStatus.cs
        { "home_status_enabling",       "连接蓝牙中" },                                          // ConnectionStatus.cs
        { "home_status_searching",      "搜索MYO中" },                                         // ConnectionStatus.cs
        { "home_status_connecting",     "连接MYO中" },                                         // ConnectionStatus.cs
        { "home_status_connected",      "已连接" },                                            // ConnectionStatus.cs
        { "home_status_myo",            "MYO" },                                                // ConnectionStatus.cs
        { "home_status_restart",        "请重启程序" },                                          // ConnectionStatus.cs
        { "home_forced_rest",           "下一任务在倒数结束后开始:" },                           // ForcedRest.cs  ?

        // task panels
        { "Training Assessment",        "评估训练" },                                    // LoadTaskPanel.cs
        { "Flight Training",            "飞行训练" },                                    // LoadTaskPanel.cs
        { "Flight Training 1",          "飞行训练 1" },                                  // LoadTaskPanel.cs 
        { "Flight Training 2",          "飞行训练 2" },                                  // LoadTaskPanel.cs
        { "Flight Training 3",          "飞行训练 3" },                                  // LoadTaskPanel.cs
        { "Flight Training 4",          "飞行训练 4" },                                  // LoadTaskPanel.cs
        { "Flight Training 5",          "飞行训练 5" },                                  // LoadTaskPanel.cs
        { "Obstacle Training",          "Obstacle Training" },                                  // LoadTaskPanel.cs
        { "Pre Assessment",             "训练前评估" },                                     // LoadTaskPanel.cs
        { "Post Assessment",            "训练后评估" },                                    // LoadTaskPanel.cs
        { "Norm Assessment",            "健侧评估" },                                    // LoadTaskPanel.cs
        { "signal_test",                "信号测试" },                                        // SignalTestButton.cs
        { "start_experiment",           "开始正式训练" },                                   // StartExperimentButton.cs
        { "placement_popup_title",      "请确认MYO的位置" },                              // LoadTaskPanel.cs
        { "placement_popup_message",    "请先确认MYO在{0}侧前臂, 再开始训练。" },               // LoadTaskPanel.cs
        { "side:",                      "{0}侧" },                                          // LoadTaskPanel.cs  ?
        { "score:",                     "{0}分" },                                         // LoadTaskPanel.cs  ?
        { "flight_training_amount_1",   "共{0}局" },                                        // LoadTaskPanel.cs
        { "flight_training_amount_2",   ", 每局{0}秒" },                                            // LoadTaskPanel.cs
        { "flight_training_amount_3",   " ({0}级)" },                                       // LoadTaskPanel.cs

        // create user scene
        { "user_id",                    "患者编号" },                                         // UserInputField.cs
        { "user_id_error",              "无效患者编号" },                                 // UserInputField.cs
        { "user_id_popup_title",        "该患者编号已存在!" },                              // SaveUser.cs
        { "user_id_popup_message",      "请输入其他患者编号。" },                          // SaveUser.cs  ?
        { "user_side",                  "训练侧" },                                      // SideSegmentedSelection.cs
        { "user_side_error",            "请选择训练侧" },                                 // SideSegmentedSelection.cs 
        
        // load user scene
        { "load_id_popup_title",        "请确认患者信息是无误的：" },                           // LoadUserId.cs

        // progress data scene
        { "no_data",                    "无数据" },                                            // LoadProgressData.cs
        { "experiment_progress",        "试验中" },                                        // ProgressDataTitle.cs
        { "session",                    "训练日" },                                            // LoadProgressData.cs
        { "score",                      "分数" },                                              // LoadProgressData.cs
        { "distance",                   "距离" },                                           // LoadProgressData.cs
        { "time",                       "训练用时" },                                               // LoadProgressData.cs
        { "duration",                   "规定时长" },                                           // LoadProgressData.cs
        { "trials",                     "局" },                                             // LoadProgressData.cs
        { "cc_max",                     "最大代偿" },                                             // LoadProgressData.cs
        { "emg_range",                  "肌电信号范围" },                                          // LoadProgressData.cs
        { "level",                      "级" },                                              // LoadProgressData.cs

        // options scene
        { "options",                    "选项" },                                            // SyncData.cs, OptionsTitle.cs
        { "data_sync_complete",         "数据上传已结束" },                                 // SyncData.cs
        { "data_sync_partial",          "部分数据上传完毕" },                                  // SyncData.cs
        { "data_sync_fail",             "数据上传失败" },                                   // SyncData.cs

        // calibration scene
        { "extend",                     "背伸" },                                             // Calibrate.cs
        { "wrist",                      "腕" },                                              // Calibrate.cs

        // flight training scene
        { "relax",                      "放松" },                                              // ExperimentController.cs
        { "level_n",                    "{0}级" },                                          // ExperimentController.cs
        { "start",                      "准备好后, 请开始" },                                   // ExperimentController.cs
        { "finished",                   "任务已完成" },                                     // ExperimentController.cs

        // buttons
        { "button_load",                "登录" },                                               // LoadUserId.cs, LoadUserButton.cs
        { "button_new",                 "新增" },                                                // NewUserButton.cs
        { "button_cancel",              "取消" },                                             // LoadTaskPanel.cs, CancelUser.cs, NavigationHome.cs, NavigationRecalibrate.cs
        { "button_save",                "保存" },                                               // SaveUser.cs
        { "button_clear",               "清除" },                                              // ClearUserButton.cs
        { "button_data",                "数据" },                                               // LoadProgressDataButton.cs
        { "button_more",                "更多" },                                               // LoadOptionsButton.cs
        { "button_confirm",             "确认" },                                            // LoadUserId.cs
        { "button_sync_data",           "上传数据" },                                          // SyncData.cs
        { "button_recalibrate",         "重新较准" },                                        // NavidationRecalibrate.cs
        { "button_home",                "主页" },                                               // NavigationHome.cs
        { "button_continue",            "继续" },                                           // LoadTaskPanel.cs

        // navigation popups
        { "nav_popup_title",            "确定？" },                                            // NavigationHome.cs, NavigationRecalibrate.cs
        { "nav_recalibrate_message",    "请重新较准后，重新开始目前的训练任务。" },             // NavigationRecalibrate.cs
        { "nav_home_message",           "回到主页面。" },                                     // NavigationHome.cs

        // generic
        { "ok",                         "好" },                                                 // SaveUser.cs
        { "left",                       "左" },                                                  // SideSegmentedSelection.cs
        { "right",                      "右" },                                                   // SideSegmentedSelection.cs
        { "m",                          "米" },                                                  // LoadProgressData.cs
        { "s",                          "秒" }                                                   // LoadProgressData.cs  

    };

}
