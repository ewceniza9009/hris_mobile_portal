using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mhris.Services
{
    public class SettingRepos
    {
        private static Models.Setting Setting;

        public static Models.Setting GetSettings()
        {
            Models.Setting result;

            using (var conn = new SQLiteConnection(App.FilePath))
            {
                result = conn.Table<Models.Setting>().FirstOrDefault();
            }

            return result;
        }

        public static void CreateLocalSettingDB(string loginCode)
        {
            using (var conn = new SQLiteConnection(App.FilePath))
            {
                Setting = new Models.Setting
                {
                    LoginCode = loginCode,
                    IsSwipedIn = false,
                    IsSwipedBreakStart = false,
                    IsSwipedBreakEnd = false,
                    IsSwipedOut = false,
                };

                var tableExistsQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='Setting';";
                var result = conn.ExecuteScalar<string>(tableExistsQuery);

                if (string.IsNullOrEmpty(result))
                {
                    conn.CreateTable<Models.Setting>();
                    conn.Insert(Setting);
                }
            }
        }

        public static void UpdateLocalSettingDB(string loginCode)
        {
            using (var conn = new SQLiteConnection(App.FilePath))
            {
                Setting = conn.Query<Models.Setting>("SELECT * FROM Setting WHERE Id = ?", 1)
                    .FirstOrDefault();

                if (Setting != null)
                {
                    Setting.LoginCode = loginCode;
                    Setting.IsSwipedIn = false;
                    Setting.IsSwipedBreakStart = false;
                    Setting.IsSwipedBreakEnd = false;
                    Setting.IsSwipedOut = false;

                    conn.RunInTransaction(() =>
                    {
                        if (conn != null) 
                        {
                            conn.Update(Setting);
                        }
                    });
                }
            }
        }

		public static void UpdateLocalSettingDBLog(string logType)
		{
			using (var conn = new SQLiteConnection(App.FilePath))
			{
				Setting = conn.Query<Models.Setting>("SELECT * FROM Setting WHERE Id = ?", 1)
					.FirstOrDefault();

				if (Setting != null)
				{
                    if (logType == "IN") Setting.IsSwipedIn = true;
					if (logType == "BS") Setting.IsSwipedBreakStart = true;
					if (logType == "BE") Setting.IsSwipedBreakEnd = true;
					if (logType == "OT") Setting.IsSwipedOut = true;

                    if (logType == "CR") 
                    {
						Setting.IsSwipedIn = false;
						Setting.IsSwipedBreakStart = false;
						Setting.IsSwipedBreakEnd = false;
						Setting.IsSwipedOut = false;
					}

					conn.RunInTransaction(() =>
					{
						if (conn != null)
						{
							conn.Update(Setting);
						}
					});
				}
			}
		}
	}
}
