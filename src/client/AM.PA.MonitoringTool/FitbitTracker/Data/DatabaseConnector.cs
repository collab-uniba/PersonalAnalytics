﻿// Created by Sebastian Mueller (smueller@ifi.uzh.ch) from the University of Zurich
// Created: 2017-01-24
// 
// Licensed under the MIT License.

using Shared;
using Shared.Data;
using System;
using FitbitTracker.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Globalization;
using System.ComponentModel;
using FitbitTracker.Data.FitbitModel;

namespace FitbitTracker.Data
{

    public class DatabaseConnector
    {

        //Database fields
        private const string ID = "id";
        private const string SAVE_TIME = "save_time";
        private const string AWAKE_COUNT = "awakeCount";
        private const string AWAKE_DURATION = "awakeDuration";
        private const string DATE_OF_SLEEP = "dateOfSleep";
        private const string DURATION = "duration";
        private const string IS_MAIN_SLEEP = "isMainSleep";
        private const string EFFICIENCY = "efficiency";
        private const string LOG_ID = "logID";
        private const string MINUTES_AFTER_WAKEUP = "minutesAfterWakeUp";
        private const string MINUTES_ASLEEP = "minutesAsleep";
        private const string MINUTES_AWAKE = "minutesAwake";
        private const string MINUTES_TO_FALL_ASLEEP = "minutesToFallAsleep";
        private const string RESTLESS_COUNT = "restlessCount";
        private const string RESTLESS_DURATION = "restlessDuration";
        private const string START_TIME = "startTime";
        private const string TIME_IN_BED = "timeInBed";
        private const string TOTAL_MINUTES_ASLEEP = "totalMinutesAsleep";
        private const string TOTAL_SLEEP_RECORDS = "totalSleepRecords";
        private const string TOTAL_TIME_IN_BED = "totalTimeInBed";
        private const string SLEEP_SUMMARY_ID = "sleepSummaryID";
        private const string DATA = "data";
        private const string DAY = "day";
        private const string DATE_OF_HR = "dateOfHR";
        private const string RESTING_HEARTRATE = "restingHeartrate";
        private const string CALORIES_OUT = "caloriesOut";
        private const string MAX = "max";
        private const string MIN = "min";
        private const string MINUTES_SPENT = "minutesSpent";
        private const string NAME = "name";
        private const string VALUE = "value";
        private const string DATE_OF_STEPS = "dateOfSteps";
        private const string ACTIVE_SCORE = "activeScore";
        private const string ELEVATION = "elevation";
        private const string FAIRLY_ACTIVE_MINUTES = "fairlyActiveMinutes";
        private const string FLOORS = "floors";
        private const string LIGHTLY_ACTIVE_MINUTES = "lightlyActiveMinutes";
        private const string SEDENTARY_MINUTES = "sendentaryMinutes";
        private const string STEPS = "steps";
        private const string VERY_ACTIVE_MINUTES = "veryActiveMinutes";
        private const string DATE_OF_ACTIVITY = "dateOfActivity";

        //Create queries
        private static readonly string CREATE_INDEX_FOR_HEARTRATE_DAY_TABLE = "CREATE UNIQUE INDEX IF NOT EXISTS idx ON " + Settings.HEARTRATE_DAY_TABLE_NAME + "(" + DATE_OF_HR + ", " + NAME + ")";

        private static readonly string CREATE_ACTIVITY_SUMMARY_TABLE_QUERY = "CREATE TABLE IF NOT EXISTS " + Settings.ACTIVITY_SUMMARY_TABLE_NAME + " ("
                                                                            + ID + " INTEGER PRIMARY KEY, "
                                                                            + DATE_OF_ACTIVITY + " TEXT UNIQUE, "
                                                                            + SAVE_TIME + " TEXT, "
                                                                            + ACTIVE_SCORE + " INTEGER, "
                                                                            + ELEVATION + " REAL, "
                                                                            + FAIRLY_ACTIVE_MINUTES + " INTEGER, "
                                                                            + FLOORS + " INTEGER, "
                                                                            + LIGHTLY_ACTIVE_MINUTES + " INTEGER, "
                                                                            + SEDENTARY_MINUTES + " INTEGER, "
                                                                            + STEPS + " INTEGER, "
                                                                            + VERY_ACTIVE_MINUTES + " INTEGER)";

        private static readonly string CREATE_STEPS_INTRA_DAY_TABLE_QUERY = "CREATE TABLE IF NOT EXISTS " + Settings.STEPS_INTRA_DAY_TABLE_NAME + " ("
                                                                            + ID + " INTEGER PRIMARY KEY, "
                                                                            + SAVE_TIME + " TEXT, "
                                                                            + DATE_OF_STEPS + " TEXT UNIQUE, "
                                                                            + VALUE + " INTEGER)";

        private static readonly string CREATE_HEARTRATE_INTRA_DAY_TABLE_QUERY = "CREATE TABLE IF NOT EXISTS " + Settings.HEARTRATE_INTRA_DAY_TABLE_NAME + " ("
                                                                            + ID + " INTEGER PRIMARY KEY, "
                                                                            + SAVE_TIME + " TEXT, "
                                                                            + DATE_OF_HR + " TEXT UNIQUE, "
                                                                            + VALUE + " INTEGER)";
        
        private static readonly string CREATE_HEARTRATE_DAY_TABLE_QUERY = "CREATE TABLE IF NOT EXISTS " + Settings.HEARTRATE_DAY_TABLE_NAME + " ("
                                                                        + ID + " INTEGER PRIMARY KEY,"
                                                                        + SAVE_TIME + " TEXT, "
                                                                        + DATE_OF_HR + " TEXT, "
                                                                        + RESTING_HEARTRATE + " INTEGER, "
                                                                        + CALORIES_OUT + " REAL, "
                                                                        + MAX + " INTEGER, "
                                                                        + MIN + "  INTEGER, "
                                                                        + MINUTES_SPENT + " INTEGER, "
                                                                        + NAME + " TEXT, "
                                                                        + "UNIQUE(" + DATE_OF_HR + ", " + NAME + ") ON CONFLICT REPLACE);";
        
        private static readonly string CREATE_SLEEP_TABLE_QUERY = "CREATE TABLE IF NOT EXISTS " + Settings.SLEEP_TABLE_NAME + " ("
                                                                + ID + " INTEGER PRIMARY KEY,"
                                                                + SLEEP_SUMMARY_ID + " INTEGER, "
                                                                + SAVE_TIME + " TEXT, "
                                                                + AWAKE_COUNT + " INTEGER, "
                                                                + AWAKE_DURATION + " INTEGER, "
                                                                + DATE_OF_SLEEP + " TEXT, "
                                                                + DURATION + " INTEGER, "
                                                                + EFFICIENCY + " INTEGER, "
                                                                + IS_MAIN_SLEEP + " INTEGER, "
                                                                + LOG_ID + " TEXT, "
                                                                + MINUTES_AFTER_WAKEUP + " INTEGER, "
                                                                + MINUTES_ASLEEP + " INTEGER, "
                                                                + MINUTES_AWAKE + " INTEGER, "
                                                                + MINUTES_TO_FALL_ASLEEP + " INTEGER, "
                                                                + RESTLESS_COUNT + " INTEGER, "
                                                                + RESTLESS_DURATION + " INTEGER, "
                                                                + START_TIME + " TEXT, "
                                                                + TIME_IN_BED + " INTEGER)";
        
        private static readonly string CREATE_SLEEP_SUMMARY_TABLE_QUERY = "CREATE TABLE IF NOT EXISTS " + Settings.SLEEP_SUMMARY_TABLE_NAME + " ("
                                                                + ID + " INTEGER PRIMARY KEY, "
                                                                + SAVE_TIME + " TEXT, "
                                                                + DATE_OF_SLEEP + " TEXT UNIQUE, "
                                                                + TOTAL_MINUTES_ASLEEP + " INTEGER, "
                                                                + TOTAL_SLEEP_RECORDS + " INTEGER, "
                                                                + TOTAL_TIME_IN_BED + " INTEGER)";

        private static readonly string CREATE_DOWNLOAD_TABLE_QUERY = "CREATE TABLE IF NOT EXISTS " + Settings.DOWNLOAD_TABLE_NAME + " ("
                                                                + ID + " INTEGER PRIMARY KEY, "
                                                                + SAVE_TIME + " TEXT, "
                                                                + DAY + " TEXT, "
                                                                + DATA + " TEXT)";

        //Update and insert queries
        private static readonly string INSERT_SYNCHRONIZED_DAY_QUERY = "INSERT INTO " + Settings.DOWNLOAD_TABLE_NAME
                                                                + "(" + SAVE_TIME + ", "
                                                                + DAY + ", "
                                                                + DATA + ") VALUES ({0}, {1}, {2})";
        
        private static readonly string INSERT_OR_IGNORE_SLEEP_SUMMARY_QUERY = "INSERT OR IGNORE INTO " + Settings.SLEEP_SUMMARY_TABLE_NAME
                                                                + "(" + SAVE_TIME + ", "
                                                                + DATE_OF_SLEEP + ", "
                                                                + TOTAL_MINUTES_ASLEEP + ", "
                                                                + TOTAL_SLEEP_RECORDS + ", "
                                                                + TOTAL_TIME_IN_BED
                                                                + ") VALUES ({0}, {1}, {2}, {3}, {4})";

        private static readonly string UPDATE_SLEEP_SUMMARY_QUERY = "UPDATE " + Settings.SLEEP_SUMMARY_TABLE_NAME
                                                                + " SET " + SAVE_TIME + " = {0}, " 
                                                                + DATE_OF_SLEEP + " = {1}, "
                                                                + TOTAL_MINUTES_ASLEEP + " = {2}, "
                                                                + TOTAL_SLEEP_RECORDS + " = {3}, "
                                                                + TOTAL_TIME_IN_BED + " = {4} "
                                                                + "WHERE " + DATE_OF_SLEEP + " = {1};";

        private static readonly string INSERT_SLEEP_QUERY = "INSERT INTO " + Settings.SLEEP_TABLE_NAME
                                                                + "(" + SLEEP_SUMMARY_ID + ", "
                                                                + SAVE_TIME + ", "
                                                                + AWAKE_COUNT + ", "
                                                                + AWAKE_DURATION + ", "
                                                                + DATE_OF_SLEEP + ", "
                                                                + DURATION + ", "
                                                                + EFFICIENCY + ", "
                                                                + IS_MAIN_SLEEP + ", "
                                                                + LOG_ID + ", "
                                                                + MINUTES_AFTER_WAKEUP + ", "
                                                                + MINUTES_ASLEEP + ", "
                                                                + MINUTES_AWAKE + ", "
                                                                + MINUTES_TO_FALL_ASLEEP + ", "
                                                                + RESTLESS_COUNT + ", "
                                                                + RESTLESS_DURATION + ", "
                                                                + START_TIME + ", "
                                                                + TIME_IN_BED
                                                                + ") VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16})";
        
        private static readonly string INSERT_OR_IGNORE_HR_SUMMARY = "INSERT OR IGNORE INTO " + Settings.HEARTRATE_DAY_TABLE_NAME
                                                    + "(" + SAVE_TIME + ", "
                                                    + DATE_OF_HR + ", "
                                                    + RESTING_HEARTRATE + ", "
                                                    + CALORIES_OUT + ", "
                                                    + MAX + ", "
                                                    + MIN + ", "
                                                    + MINUTES_SPENT + ", "
                                                    + NAME
                                                    + ") VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})";

        private static readonly string UPDATE_HR_SUMMARY = "UPDATE " + Settings.HEARTRATE_DAY_TABLE_NAME
                                                        + " SET " + SAVE_TIME + " = {0}, "
                                                        + DATE_OF_HR + " = {1}, "
                                                        + RESTING_HEARTRATE + " = {2}, "
                                                        + CALORIES_OUT + " = {3}, "
                                                        + MAX + " = {4}, "
                                                        + MIN + " = {5}, "
                                                        + MINUTES_SPENT + " = {6}, "
                                                        + NAME + " = {7} "
                                                        + "WHERE " + DATE_OF_HR + " = {1} AND " + NAME + " = {7};";

        private static readonly string INSERT_OR_IGNORE_HR_INTRADAY = "INSERT OR IGNORE INTO " + Settings.HEARTRATE_INTRA_DAY_TABLE_NAME
                                                                    + "(" + SAVE_TIME + ", "
                                                                    + DATE_OF_HR + ", "
                                                                    + VALUE
                                                                    + ") VALUES ({0}, {1}, {2})";

        private static readonly string INSERT_OR_IGNORE_STEPS_INTRADAY = "INSERT OR IGNORE INTO " + Settings.STEPS_INTRA_DAY_TABLE_NAME
                                                            + "(" + SAVE_TIME + ", "
                                                            + DATE_OF_STEPS + ", "
                                                            + VALUE
                                                            + ") VALUES ({0}, {1}, {2})";

        private static readonly string UPDATE_STEPS_INTRADAY = "UPDATE " + Settings.STEPS_INTRA_DAY_TABLE_NAME
                                                            + " SET " + SAVE_TIME + " = {0}, "
                                                            + DATE_OF_STEPS + " = {1}, "
                                                            + VALUE + " = {2} "
                                                            + "WHERE " + DATE_OF_STEPS + " = {1};";

        private static readonly string INSERT_OR_IGNORE_ACTIVITY_SUMMARY = "INSERT OR IGNORE INTO " + Settings.ACTIVITY_SUMMARY_TABLE_NAME
                                                                            + "(" + DATE_OF_ACTIVITY + ", " 
                                                                            + SAVE_TIME + ", "
                                                                            + ACTIVE_SCORE + ", "
                                                                            + ELEVATION + ", "
                                                                            + FAIRLY_ACTIVE_MINUTES + ", "
                                                                            + FLOORS + ", "
                                                                            + LIGHTLY_ACTIVE_MINUTES + ", "
                                                                            + SEDENTARY_MINUTES + ", "
                                                                            + STEPS + ", "
                                                                            + VERY_ACTIVE_MINUTES
                                                                            + ") VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9});";

        private static readonly string UPDATE_ACTIVITY_SUMMARY = "UPDATE " + Settings.ACTIVITY_SUMMARY_TABLE_NAME
                                                                + " SET " + DATE_OF_ACTIVITY + " = {0}, "
                                                                + SAVE_TIME + " = {1}, "
                                                                + ACTIVE_SCORE + " = {2}, "
                                                                + ELEVATION + " = {3}, "
                                                                + FAIRLY_ACTIVE_MINUTES + " = {4}, "
                                                                + FLOORS + " = {5}, "
                                                                + LIGHTLY_ACTIVE_MINUTES + " = {6}, "
                                                                + SEDENTARY_MINUTES + " = {7}, "
                                                                + STEPS + " = {8}, "
                                                                + VERY_ACTIVE_MINUTES + " = {9} "
                                                                + "WHERE " + DATE_OF_ACTIVITY + " = {0};";

        private static readonly string INSERT_OR_IGNORE_MULTIPLE_HEART_RATE_INTRA_DAY_VALUES = "INSERT OR IGNORE INTO " + Settings.HEARTRATE_INTRA_DAY_TABLE_NAME
                                                                                             + " SELECT null as " + ID + ", "
                                                                                             + "{0} AS " + SAVE_TIME + ", "
                                                                                             + "{1} AS " + DATE_OF_HR + ", "
                                                                                             + "{2} AS " + VALUE;

        private static readonly string INSERT_OR_INGORE_MULTIPLE_STEP_INTRA_DAY_VALUES = "INSERT OR IGNORE INTO " + Settings.STEPS_INTRA_DAY_TABLE_NAME
                                                                                        + " SELECT null as " + ID + ", "
                                                                                        + "{0} AS " + SAVE_TIME + ", "
                                                                                        + "{1} AS " + DATE_OF_SLEEP + ", "
                                                                                        + "{2} AS " + VALUE;
        #region create
        internal static void CreateFitbitTables()
        {
            try
            {
                Database.GetInstance().ExecuteDefaultQuery(CREATE_SLEEP_TABLE_QUERY);
                Database.GetInstance().ExecuteDefaultQuery(CREATE_SLEEP_SUMMARY_TABLE_QUERY);
                Database.GetInstance().ExecuteDefaultQuery(CREATE_DOWNLOAD_TABLE_QUERY);
                Database.GetInstance().ExecuteDefaultQuery(CREATE_HEARTRATE_DAY_TABLE_QUERY);
                Database.GetInstance().ExecuteDefaultQuery(CREATE_INDEX_FOR_HEARTRATE_DAY_TABLE);
                Database.GetInstance().ExecuteDefaultQuery(CREATE_HEARTRATE_INTRA_DAY_TABLE_QUERY);
                Database.GetInstance().ExecuteDefaultQuery(CREATE_STEPS_INTRA_DAY_TABLE_QUERY);
                Database.GetInstance().ExecuteDefaultQuery(CREATE_ACTIVITY_SUMMARY_TABLE_QUERY);
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
            }
        }
        #endregion

        #region insert
        internal static void SaveStepDataForDay(StepData stepData, DateTimeOffset day)
        {
            foreach (StepDataEntry dataEntry in stepData.IntraDay.Dataset)
            {
                string query = string.Empty;
                query += String.Format(UPDATE_STEPS_INTRADAY, "'" + DateTime.Now.ToString(Settings.FORMAT_DAY_AND_TIME) + "'", "'" + new DateTime(day.Year, day.Month, day.Day, dataEntry.Time.Hour, dataEntry.Time.Minute, dataEntry.Time.Second).ToString(Settings.FORMAT_DAY_AND_TIME) + "'", dataEntry.Value);
                Database.GetInstance().ExecuteDefaultQuery(query);
            }

            int start = 0;
            int end = stepData.IntraDay.Dataset.Count;

            while (start < end)
            {
                string query = string.Empty;
                if (stepData.IntraDay.Dataset.Count > 0)
                {
                    StepDataEntry dataEntry = stepData.IntraDay.Dataset[start];
                    string insertTime = "'" + DateTime.Now.ToString(Settings.FORMAT_DAY_AND_TIME) + "'";
                    string stepTime = "'" + new DateTime(day.Year, day.Month, day.Day, dataEntry.Time.Hour, dataEntry.Time.Minute, dataEntry.Time.Second).ToString(Settings.FORMAT_DAY_AND_TIME) + "'";
                    query += String.Format(INSERT_OR_INGORE_MULTIPLE_STEP_INTRA_DAY_VALUES, insertTime, stepTime, dataEntry.Value);

                    int count = 0;
                    for (int i = start; i < stepData.IntraDay.Dataset.Count && count < 499; i++)
                    {
                        query += String.Format(" UNION ALL SELECT null, {0}, {1}, {2}", "'" + DateTime.Now.ToString(Settings.FORMAT_DAY_AND_TIME) + "'", "'" + new DateTime(day.Year, day.Month, day.Day, stepData.IntraDay.Dataset[i].Time.Hour, stepData.IntraDay.Dataset[i].Time.Minute, stepData.IntraDay.Dataset[i].Time.Second).ToString(Settings.FORMAT_DAY_AND_TIME) + "'", stepData.IntraDay.Dataset[i].Value);
                        count++;
                    }
                    Database.GetInstance().ExecuteDefaultQuery(query);
                    start += count;
                }
            }
        }

        internal static void SaveActivityData(ActivityData activityData, DateTimeOffset day)
        {
            string query = string.Empty;
            query += String.Format(UPDATE_ACTIVITY_SUMMARY, "'" + new DateTime(day.Year, day.Month, day.Day).ToString(Settings.FORMAT_DAY) + "'", "'" + DateTime.Now.ToString(Settings.FORMAT_DAY) + "'", activityData.Summary.ActiveScore, activityData.Summary.Elevation, activityData.Summary.FairlyActiveMinutes, activityData.Summary.Floors, activityData.Summary.LightlyActiveMinutes, activityData.Summary.SedentaryMinutes, activityData.Summary.Steps, activityData.Summary.Steps, activityData.Summary.VeryActiveMinutes);
            Database.GetInstance().ExecuteDefaultQuery(query);

            query = string.Empty;
            query += String.Format(INSERT_OR_IGNORE_ACTIVITY_SUMMARY, "'" + new DateTime(day.Year, day.Month, day.Day).ToString(Settings.FORMAT_DAY) + "'", "'" + DateTime.Now.ToString(Settings.FORMAT_DAY) + "'", activityData.Summary.ActiveScore, activityData.Summary.Elevation, activityData.Summary.FairlyActiveMinutes, activityData.Summary.Floors, activityData.Summary.LightlyActiveMinutes, activityData.Summary.SedentaryMinutes, activityData.Summary.Steps, activityData.Summary.Steps, activityData.Summary.VeryActiveMinutes);
            Database.GetInstance().ExecuteDefaultQuery(query);
        }

        private static void InsertHRData(object sender, DoWorkEventArgs e)
        {
            object[] parameters = e.Argument as object[];
            List<HeartrateIntraDayData> hrData = (List<HeartrateIntraDayData>)parameters[0];
            
            int start = 0;
            int end = hrData.Count;

            while (start < end)
            {
                string query = string.Empty;
                if (hrData.Count > 0)
                {
                    HeartrateIntraDayData data = hrData[start];
                    query += String.Format(INSERT_OR_IGNORE_MULTIPLE_HEART_RATE_INTRA_DAY_VALUES, "'" + DateTime.Now.ToString(Settings.FORMAT_DAY_AND_TIME) + "'", "'" + new DateTime(data.Day.Year, data.Day.Month, data.Day.Day, data.Time.Hour, data.Time.Minute, data.Time.Second).ToString(Settings.FORMAT_DAY_AND_TIME) + "'", data.Value);

                    int count = 0;
                    for (int i = start; i < hrData.Count && count < 499; i++)
                    {
                        query += String.Format(" UNION ALL SELECT null, {0}, {1}, {2}", "'" + DateTime.Now.ToString(Settings.FORMAT_DAY_AND_TIME) + "'", "'" + new DateTime(hrData[i].Day.Year, hrData[i].Day.Month, hrData[i].Day.Day, hrData[i].Time.Hour, hrData[i].Time.Minute, hrData[i].Time.Second).ToString(Settings.FORMAT_DAY_AND_TIME) + "'", hrData[i].Value);
                        count++;
                    }
                    Database.GetInstance().ExecuteDefaultQuery(query);
                    start += count;
                }
            }
            e.Result = hrData.Count;
        }

        internal static void SaveHRIntradayData(List<HeartrateIntraDayData> hrData)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(InsertHRData);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(runWorkerCompleted);

            object paramObj = hrData;
            object[] parameters = new object[] { paramObj };

            worker.RunWorkerAsync(parameters);
        }

        private static void runWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Logger.WriteToConsole((int)e.Result + " HR data points imported.");
        }

        internal static void SaveHRData(List<HeartRateDayData> hrData)
        {
            DateTime insert = DateTime.Now;
            foreach (HeartRateDayData data in hrData)
            {
                string query = string.Empty;
                query += String.Format(UPDATE_HR_SUMMARY, "'" + insert.ToString(Settings.FORMAT_DAY_AND_TIME) + "'", "'" + data.Date.ToString(Settings.FORMAT_DAY) + "'", Double.IsNaN(data.RestingHeartrate) ? "null" : data.RestingHeartrate.ToString(), data.CaloriesOut, data.Max, data.Min, data.MinutesSpent, "'" + data.Name + "'");
                Database.GetInstance().ExecuteDefaultQuery(query);

                query = string.Empty;
                query += String.Format(INSERT_OR_IGNORE_HR_SUMMARY, "'" + insert.ToString(Settings.FORMAT_DAY_AND_TIME) + "'", "'" + data.Date.ToString(Settings.FORMAT_DAY) + "'", Double.IsNaN(data.RestingHeartrate) ? "null" : data.RestingHeartrate.ToString(), data.CaloriesOut, data.Max, data.Min, data.MinutesSpent, "'" + data.Name + "'");
                Database.GetInstance().ExecuteDefaultQuery(query);
            }
        }

        internal static void SaveSleepData(SleepData sleepData)
        {
            if (sleepData.Sleep.Count > 0)
            {
                SleepSummary summary = sleepData.Summary;

                string query = String.Empty;
                DateTime insert = DateTime.Now;

                query += String.Format(UPDATE_SLEEP_SUMMARY_QUERY, "'" + insert + "'", "'" + sleepData.Sleep[0].DateOfSleep.ToString(Settings.FORMAT_DAY) + "'", summary.TotalMinutesAsleep, summary.TotalSleepRecords, summary.TotalTimeInBed);
                Database.GetInstance().ExecuteDefaultQuery(query);

                query = String.Empty;
                query += String.Format(INSERT_OR_IGNORE_SLEEP_SUMMARY_QUERY, "'" + insert + "'", "'" + sleepData.Sleep[0].DateOfSleep.ToString(Settings.FORMAT_DAY) + "'", summary.TotalMinutesAsleep, summary.TotalSleepRecords, summary.TotalTimeInBed);
                Database.GetInstance().ExecuteDefaultQuery(query);

                //GET ID of previous insert
                string idquery = "SELECT * FROM " + Settings.SLEEP_SUMMARY_TABLE_NAME + " WHERE " + SAVE_TIME + " = '" + insert + "';";
                var table = Database.GetInstance().ExecuteReadQuery(idquery);

                string id = "";
                if (table.Rows.Count > 0)
                {
                    id = (table.Rows[0][0]).ToString();
                }

                foreach (SleepLog log in sleepData.Sleep)
                {
                    if (!DoesSleepLogAlreadyExists(log.LogID))
                    {
                        string sleepQuery = String.Empty;
                        sleepQuery += String.Format(INSERT_SLEEP_QUERY, id, "'" + DateTime.Now + "'", log.AwakeCount, log.AwakeDuration, "'" + log.DateOfSleep.ToString(Settings.FORMAT_DAY) + "'", log.Duration, log.Efficiency, log.IsMainSleep ? 1 : 0, log.LogID, log.MinutesAfterWakeup, log.MinutesAsleep, log.MinutesAwake, Double.IsNaN(log.MinutesToFallAsleep) ? "null" : log.MinutesToFallAsleep.ToString(), log.RestlessCount, log.RestlessDuration, "'" + log.StartTime.ToString(Settings.FORMAT_DAY_AND_TIME) + "'", log.TimeInBed);
                        Database.GetInstance().ExecuteDefaultQuery(sleepQuery);
                    }
                }
            }
        }

        internal static void SetSynchronizedDay(DateTimeOffset day, DataType datType)
        {
            string query = String.Empty;
            query += String.Format(INSERT_SYNCHRONIZED_DAY_QUERY, "'" + DateTime.Now.ToString(Settings.FORMAT_DAY_AND_TIME) + "'", "'" + day.ToString(Settings.FORMAT_DAY) + "'", "'" + datType + "'");
            Database.GetInstance().ExecuteDefaultQuery(query);
        }
        #endregion

        #region SELECT
        internal static bool DoesSleepLogAlreadyExists(string sleepLogId)
        {
            string query = "SELECT * FROM " + Settings.SLEEP_TABLE_NAME + " WHERE " + LOG_ID + " = '" + sleepLogId + "'";
            var table = Database.GetInstance().ExecuteReadQuery(query);
            return table.Rows.Count > 0;
        }

        internal static List<DateTimeOffset> GetDaysToSynchronize(DataType datType)
        {
            DateTimeOffset start;
            if (Database.GetInstance().HasSetting(Settings.DOWNLOAD_START_DATE))
            {
                start = DateTime.Parse(Database.GetInstance().GetSettingsString(Settings.DOWNLOAD_START_DATE, DateTimeOffset.Now.ToString(Settings.FORMAT_DAY)));
            }
            else
            {
                start = DateTimeOffset.Now;
                Database.GetInstance().SetSettings(Settings.DOWNLOAD_START_DATE, start.ToString(Settings.FORMAT_DAY));
            }
            
            List<DateTimeOffset> daysInDatabase = new List<DateTimeOffset>();
            string query = "SELECT " + DAY + " FROM " + Settings.DOWNLOAD_TABLE_NAME + " WHERE " + DATA + " = '" + datType + "'";
            var table = Database.GetInstance().ExecuteReadQuery(query);
            
            foreach (DataRow row in table.Rows)
            {
                daysInDatabase.Add(DateTime.ParseExact(row[0].ToString(), Settings.FORMAT_DAY, CultureInfo.InvariantCulture));
            }

            List<DateTimeOffset> days = new List<DateTimeOffset>();
            while (start < DateTimeOffset.Now)
            {
                days.Add(start);
                start = start.AddDays(1);
            }

            return days.Where(f => !daysInDatabase.Any(t => t.Day == f.Day && t.Year == f.Year && t.Month == f.Month)).ToList();  
        }

        public static double GetMinutesAsleep(DateTimeOffset start, DateTimeOffset end, VisType type)
        {
            string query = string.Empty;

            query += "Select SUM(" + TOTAL_MINUTES_ASLEEP + ") From " + Settings.SLEEP_SUMMARY_TABLE_NAME + " WHERE substr(" + DATE_OF_SLEEP + ", 7) || substr(" + DATE_OF_SLEEP + ", 4, 2) || substr(" + DATE_OF_SLEEP + ", 1, 2) between '" + start.ToString(Settings.FORMAT_SQLITE_DAY) + "' AND '" + end.ToString(Settings.FORMAT_SQLITE_DAY) + "'";
            Console.WriteLine(query);

            var table = Database.GetInstance().ExecuteReadQuery(query);

            if (table.Rows.Count > 0 && table.Rows[0][0] != null)
            {
                try
                {
                    DataRow row = table.Rows[0];
                    string col = row[0].ToString();
                    if (col.Equals(string.Empty))
                    {
                        return double.NaN;
                    }
                    else
                    {
                        return Double.Parse(col);
                    }
                }
                catch (Exception e)
                {
                    return double.NaN;
                }
            }
            else
            {
                return double.NaN;
            }
        }

        #endregion

    }

}