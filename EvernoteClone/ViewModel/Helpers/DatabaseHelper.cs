﻿using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteClone.ViewModel.Helpers
{
    public class DatabaseHelper
    {
        private static string dbFile = Path.Combine(Environment.CurrentDirectory, "notesDB.db3");
        private static string dbPath = "https://notes-app-wpf-67cc7-default-rtdb.firebaseio.com/";

        public static async Task<bool> Insert<T>(T item)
        {
            //bool result = false;

            //using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            //{
            //    conn.CreateTable<T>();
            //    int rows = conn.Insert(item);
            //    if(rows > 0)
            //        result = true;
            //}

            //return result;

            string jsonBody = JsonConvert.SerializeObject(item);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            using(var client = new HttpClient())
            {
                var result = await client.PostAsync($"{dbPath}{item.GetType().Name.ToLower()}.json", content);

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool Update<T>(T item)
        {
            bool result = false;

            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                int rows = conn.Update(item);
                if (rows > 0)
                    result = true;
            }

            return result;
        }

        public static bool Delete<T>(T item)
        {
            bool result = false;

            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                int rows = conn.Delete(item);
                if (rows > 0)
                    result = true;
            }

            return result;
        }

        public static List<T> Read<T>() where T : new()
        {
            List<T> items;

            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                items = conn.Table<T>().ToList();
            }

            return items;
        }


    }
}
