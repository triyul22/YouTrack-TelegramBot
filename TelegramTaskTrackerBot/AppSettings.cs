using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramTaskTrackerBot
{
    class AppSettings
    {

        // my youtrack token
        //perm:cm9vdA==.NDUtMA==.pQFX0ftsPE0v8hznQJvtwGB9HVi409

        public static string Url { get; set; } = "https://telegrambot20200705141355.azurewebsites.net/{0}";
        public static string Name { get; set; } = "TaskTrackerManagingBot";
        public static string Token { get; set; } = "1358594021:AAGZ-gHy-sfLA-wendeLdtpFUTdU5HwcwRY";
        public static string YouTrackToken { get; set; } = "";
        public static string YouTrackUrl { get; set; } = "";
    }
}
